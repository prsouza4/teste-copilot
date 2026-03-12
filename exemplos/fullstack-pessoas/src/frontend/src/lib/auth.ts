import NextAuth, { DefaultSession } from "next-auth"
import type { NextAuthConfig, Session } from "next-auth"
import type { JWT } from "next-auth/jwt"

declare module "next-auth" {
  interface Session {
    accessToken?: string
    user: {
      id: string
    } & DefaultSession["user"]
  }
}

declare module "next-auth/jwt" {
  interface JWT {
    accessToken?: string
    refreshToken?: string
  }
}

export const authConfig: NextAuthConfig = {
  providers: [
    {
      id: "duende",
      name: "Duende IdentityServer",
      type: "oauth",
      wellKnown: "http://localhost:5001/.well-known/openid-configuration",
      authorization: {
        url: "http://localhost:5001/connect/authorize",
        params: {
          scope: "openid profile email api.cadastro:read api.cadastro:write",
        },
      },
      token: "http://localhost:5001/connect/token",
      userinfo: "http://localhost:5001/connect/userinfo",
      clientId: process.env.AUTH_DUENDE_ID,
      clientSecret: process.env.AUTH_DUENDE_SECRET,
      profile(profile) {
        return {
          id: profile.sub,
          name: profile.name,
          email: profile.email,
        }
      },
    },
  ],
  callbacks: {
    async jwt({ token, account }) {
      // Persist the OAuth access_token to the token right after signin
      if (account) {
        token.accessToken = account.access_token
        token.refreshToken = account.refresh_token
      }
      return token
    },
    async session({ session, token }: { session: Session; token: JWT }) {
      // Send properties to the client
      session.accessToken = token.accessToken as string
      if (session.user) {
        session.user.id = token.sub as string
      }
      return session
    },
  },
  pages: {
    signIn: "/login",
  },
  secret: process.env.NEXTAUTH_SECRET,
}

export const { handlers, auth, signIn, signOut } = NextAuth(authConfig)

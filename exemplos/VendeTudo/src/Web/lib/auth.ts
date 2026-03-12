import NextAuth from "next-auth";

export const { handlers, signIn, signOut, auth } = NextAuth({
  providers: [
    {
      id: "identidade",
      name: "VendeTudo",
      type: "oidc",
      issuer: process.env.IDENTIDADE_ISSUER,
      clientId: process.env.IDENTIDADE_CLIENT_ID,
      clientSecret: process.env.IDENTIDADE_CLIENT_SECRET,
      authorization: { params: { scope: "openid profile email catalogo cesta pedidos" } },
    },
  ],
  callbacks: {
    async jwt({ token, account }) {
      if (account) {
        token.accessToken = account.access_token;
      }
      return token;
    },
    async session({ session, token }) {
      (session as any).accessToken = token.accessToken;
      return session;
    },
  },
  pages: {
    signIn: "/api/auth/signin",
  },
});

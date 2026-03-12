import type { Metadata } from "next"
import { SessionProvider } from "next-auth/react"
import { Navbar } from "@/components/Navbar"
import "./globals.css"

export const metadata: Metadata = {
  title: "Gestão de Pessoas",
  description: "Sistema de cadastro de pessoas",
}

export default function RootLayout({
  children,
}: Readonly<{
  children: React.ReactNode
}>) {
  return (
    <html lang="pt-BR">
      <body>
        <SessionProvider>
          <Navbar />
          <main className="min-h-screen bg-gray-50">
            {children}
          </main>
        </SessionProvider>
      </body>
    </html>
  )
}

"use client"

import { signOut, useSession } from "next-auth/react"

export function Navbar() {
  const { data: session } = useSession()

  return (
    <nav className="bg-blue-600 text-white shadow-lg">
      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
        <div className="flex justify-between items-center h-16">
          <div className="flex items-center">
            <h1 className="text-xl font-bold">Gestão de Pessoas</h1>
          </div>
          <div className="flex items-center gap-4">
            {session?.user ? (
              <>
                <span className="text-sm">{session.user.email}</span>
                <button
                  onClick={() => signOut({ callbackUrl: "/login" })}
                  className="bg-blue-700 hover:bg-blue-800 px-4 py-2 rounded-md text-sm font-medium transition-colors"
                >
                  Sair
                </button>
              </>
            ) : (
              <a
                href="/login"
                className="bg-blue-700 hover:bg-blue-800 px-4 py-2 rounded-md text-sm font-medium transition-colors"
              >
                Entrar
              </a>
            )}
          </div>
        </div>
      </div>
    </nav>
  )
}

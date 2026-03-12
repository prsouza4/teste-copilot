import type { Metadata } from "next";
import "./globals.css";
import { BarraNavegacao } from "@/components/BarraNavegacao";

export const metadata: Metadata = {
  title: "VendeTudo",
  description: "Sua loja virtual completa",
};

export default function RootLayout({
  children,
}: {
  children: React.ReactNode;
}) {
  return (
    <html lang="pt-BR">
      <body>
        <BarraNavegacao />
        <main className="container mx-auto px-4 py-8">
          {children}
        </main>
      </body>
    </html>
  );
}

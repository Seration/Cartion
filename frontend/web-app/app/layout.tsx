import type { Metadata } from 'next'
import './globals.css'
import Navbar from './nav/Navbar'
import ToasterProvider from './providers/ToasterProvider'

export const metadata: Metadata = {
  title: 'Create Next App',
  description: 'Generated by create next app',
}

export default function RootLayout({
  children,
}: {
  children: React.ReactNode
}) {
  return (
    <html lang="en">
      <body>
      <ToasterProvider />
        <Navbar></Navbar>
        <main className='container mx-auto px-5 pt-10 pl-10'>
        {children}
        </main>
        </body>
    </html>
  )
}
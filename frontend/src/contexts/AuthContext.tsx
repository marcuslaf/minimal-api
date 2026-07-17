import { createContext, useContext, useState, useEffect, type ReactNode } from "react"
import { authService } from "@/services/services"
import type { AdministradorLogado } from "@/types"

interface AuthContextType {
  user: AdministradorLogado | null
  login: (email: string, senha: string) => Promise<void>
  logout: () => void
  isAuthenticated: boolean
  isAdm: boolean
}

const AuthContext = createContext<AuthContextType | undefined>(undefined)

export function AuthProvider({ children }: { children: ReactNode }) {
  const [user, setUser] = useState<AdministradorLogado | null>(() => {
    const saved = localStorage.getItem("user")
    return saved ? JSON.parse(saved) : null
  })

  useEffect(() => {
    if (user) localStorage.setItem("user", JSON.stringify(user))
    else localStorage.removeItem("user")
  }, [user])

  const login = async (email: string, senha: string) => {
    const data = await authService.login(email, senha)
    localStorage.setItem("token", data.token)
    setUser(data)
  }

  const logout = () => {
    localStorage.removeItem("token")
    localStorage.removeItem("user")
    setUser(null)
  }

  return (
    <AuthContext.Provider value={{ user, login, logout, isAuthenticated: !!user, isAdm: user?.perfil === "Adm" }}>
      {children}
    </AuthContext.Provider>
  )
}

export function useAuth() {
  const ctx = useContext(AuthContext)
  if (!ctx) throw new Error("useAuth must be used within AuthProvider")
  return ctx
}

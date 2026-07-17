import api from "./api"
import type { AdministradorLogado, Veiculo, Administrador } from "@/types"

export const authService = {
  async login(email: string, senha: string): Promise<AdministradorLogado> {
    const { data } = await api.post<AdministradorLogado>("/administradores/login", { email, senha })
    return data
  },
}

export const administradorService = {
  async listar(pagina?: number): Promise<Administrador[]> {
    const { data } = await api.get<Administrador[]>("/administradores", { params: { pagina } })
    return data
  },

  async buscarPorId(id: number): Promise<Administrador> {
    const { data } = await api.get<Administrador>(`/administradores/${id}`)
    return data
  },

  async criar(admin: { email: string; senha: string; perfil: string }): Promise<Administrador> {
    const { data } = await api.post<Administrador>("/administradores", admin)
    return data
  },
}

export const veiculoService = {
  async listar(pagina?: number, nome?: string, marca?: string): Promise<Veiculo[]> {
    const { data } = await api.get<Veiculo[]>("/veiculos", { params: { pagina, nome, marca } })
    return data
  },

  async buscarPorId(id: number): Promise<Veiculo> {
    const { data } = await api.get<Veiculo>(`/veiculos/${id}`)
    return data
  },

  async criar(veiculo: { nome: string; marca: string; ano: number }): Promise<Veiculo> {
    const { data } = await api.post<Veiculo>("/veiculos", veiculo)
    return data
  },

  async atualizar(id: number, veiculo: { nome: string; marca: string; ano: number }): Promise<Veiculo> {
    const { data } = await api.put<Veiculo>(`/veiculos/${id}`, veiculo)
    return data
  },

  async deletar(id: number): Promise<void> {
    await api.delete(`/veiculos/${id}`)
  },
}

export const healthService = {
  async check(): Promise<string> {
    const { data } = await api.get("/health")
    return data
  },
}

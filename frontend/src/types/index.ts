export interface Veiculo {
  id: number
  nome: string
  marca: string
  ano: number
}

export interface Administrador {
  id: number
  email: string
  perfil: string
}

export interface AdministradorLogado {
  email: string
  perfil: string
  token: string
}

export interface ErrosDeValidacao {
  mensagens: string[]
}

export interface Home {
  mensagem: string
  doc: string
  health: string
}

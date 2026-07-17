# Minimal API - Gestão de Veículos

API RESTful construída com ASP.NET Core 8 Minimal API para gestão de administradores e veículos, com autenticação JWT, MySQL e testes automatizados. Frontend React com interface responsiva e dark/light theme.

## Stack Tecnológica

### Backend

| Tecnologia | Versão | Descrição |
|------------|--------|-----------|
| .NET | 8.0 LTS | Framework principal (suporte até Nov/2026) |
| ASP.NET Core Minimal API | 8.0 | Framework web |
| Entity Framework Core | 8.0.11 | ORM para acesso a dados |
| Pomelo MySQL | 8.0.2 | Provider MySQL |
| BCrypt.Net | 4.0.3 | Hashing de senhas |
| FluentValidation | 11.9.0 | Validação de DTOs |
| Serilog | 8.0.3 | Logging estruturado |
| JWT Bearer | 8.0.11 | Autenticação por token |
| Swashbuckle | 6.9.0 | Documentação Swagger |
| MSTest | 3.6.3 | Framework de testes |

### Frontend

| Tecnologia | Versão | Descrição |
|------------|--------|-----------|
| React | 19.x | Biblioteca UI |
| TypeScript | 6.x | Tipagem estática |
| Vite | 8.x | Build tool |
| Tailwind CSS | 3.x | Utility-first CSS |
| shadcn/ui | - | Componentes UI |
| React Router | 7.x | Roteamento |
| Axios | 1.x | HTTP client |
| Lucide React | - | Ícones |

## Pré-requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) ou superior
- [MySQL 8.x](https://dev.mysql.com/downloads/mysql/) (ou use Docker)
- [Docker](https://www.docker.com/) e Docker Compose (opcional)

## Estrutura do Projeto

```
minimal-api/
├── Api/
│   ├── Dominio/
│   │   ├── DTOs/                    # Data Transfer Objects
│   │   ├── Entidades/               # Entidades de domínio
│   │   ├── Enuns/                   # Enumerações (Perfil: Adm, Editor)
│   │   ├── Interfaces/              # Contratos dos services (async)
│   │   ├── ModelViews/              # Models de resposta
│   │   └── Servicos/                # Implementação dos services
│   ├── Endpoints/                   # Endpoints extraídos por domínio
│   │   ├── AdministradoresEndpoints.cs
│   │   └── VeiculosEndpoints.cs
│   ├── Infraestrutura/Db/           # DbContext e configuração do EF Core
│   ├── Middleware/                   # Exception handling global
│   ├── Validators/                  # Validações FluentValidation
│   ├── Migrations/                  # Migrations do EF Core
│   ├── Program.cs                   # Ponto de entrada da aplicação
│   └── minimal-api.csproj
├── frontend/
│   ├── src/
│   │   ├── components/              # Componentes React (Layout, UI)
│   │   ├── contexts/                # AuthContext, ThemeContext
│   │   ├── pages/                   # Login, Dashboard, Veículos, Administradores
│   │   ├── services/                # API services (axios)
│   │   ├── types/                   # TypeScript types
│   │   └── lib/                     # Utilitários
│   ├── public/                      # Assets estáticos
│   ├── tailwind.config.js           # Configuração Tailwind + dark mode
│   ├── vite.config.ts               # Configuração Vite + proxy
│   └── package.json
├── Test/
│   ├── Domain/                      # Testes unitários de domínio
│   ├── Helpers/                     # Setup de testes com WebApplicationFactory
│   ├── Mocks/                       # Mocks para services
│   └── Requests/                    # Testes de integração (HTTP)
├── Dockerfile                       # Build multi-stage
├── docker-compose.yml               # Infraestrutura completa
└── .env.example                     # Variáveis de ambiente
```

## Como Rodar

### Opção 1: Manual (desenvolvimento local)

1. **Clonar o repositório:**
```bash
git clone https://github.com/marcuslaf/minimal-api.git
cd minimal-api
```

2. **Configurar o banco de dados:**

Certifique-se de que o MySQL está rodando. Crie o banco de dados:
```sql
CREATE DATABASE minimal_api;
```

Ou use o banco em memória (padrão em desenvolvimento) — não precisa de MySQL.

3. **Configurar variáveis de ambiente (opcional):**

Copie o `.env.example` para `.env` e ajuste conforme necessário:
```bash
cp .env.example .env
```

As configurações padrão em `appsettings.json` funcionam para desenvolvimento local.

4. **Rodar o Backend:**
```bash
cd Api
dotnet restore
dotnet run
```

5. **Rodar o Frontend (em outro terminal):**
```bash
cd frontend
npm install
npm run dev
```

6. **Acessar a aplicação:**

| URL | Descrição |
|-----|-----------|
| `http://localhost:3000` | Frontend (React) |
| `http://localhost:5004` | API |
| `http://localhost:5004/swagger` | Swagger UI |
| `http://localhost:5004/health` | Health Check |

### Opção 2: Docker Compose

1. **Subir tudo com Docker:**
```bash
docker-compose up --build
```

2. **Acessar:**
- API: `http://localhost:8080`
- Swagger: `http://localhost:8080/swagger`
- MySQL: `localhost:3306` (usuário: `root`, senha: `root`)

3. **Parar:**
```bash
docker-compose down
```

4. **Parar e limpar dados:**
```bash
docker-compose down -v
```

## Rodando os Testes

### Testes unitários (sem MySQL)

```bash
dotnet test
```

Os testes usam **InMemory Database**, então não precisam de MySQL rodando.

### Testes com cobertura de código

```bash
dotnet test --collect:"XPlat Code Coverage"
```

## Endpoints da API

### Autenticação

| Método | Endpoint | Descrição | Autenticação |
|--------|----------|-----------|--------------|
| POST | `/administradores/login` | Login e retorno de token JWT | Não |

**Requisição:**
```json
{
  "email": "administrador@teste.com",
  "senha": "123456"
}
```

**Resposta (200 OK):**
```json
{
  "email": "administrador@teste.com",
  "perfil": "Adm",
  "token": "eyJhbGciOiJIUzI1NiIs..."
}
```

### Administradores (requer perfil Adm)

| Método | Endpoint | Descrição | Autenticação |
|--------|----------|-----------|--------------|
| GET | `/administradores` | Listar todos (paginado) | JWT + Adm |
| GET | `/administradores/{id}` | Buscar por ID | JWT + Adm |
| POST | `/administradores` | Criar novo | JWT + Adm |

### Veículos (requer Adm ou Editor)

| Método | Endpoint | Descrição | Autenticação |
|--------|----------|-----------|--------------|
| GET | `/veiculos` | Listar todos (paginado, filtro nome/marca) | JWT |
| GET | `/veiculos/{id}` | Buscar por ID | JWT + Adm/Editor |
| POST | `/veiculos` | Criar novo | JWT + Adm/Editor |
| PUT | `/veiculos/{id}` | Atualizar | JWT + Adm |
| DELETE | `/veiculos/{id}` | Deletar | JWT + Adm |

### Exemplos de Uso com cURL

**Login:**
```bash
curl -X POST http://localhost:5004/administradores/login \
  -H "Content-Type: application/json" \
  -d '{"email":"administrador@teste.com","senha":"123456"}'
```

**Listar veículos (com token):**
```bash
curl http://localhost:5004/veiculos \
  -H "Authorization: Bearer <token>"
```

**Filtrar veículos por marca:**
```bash
curl "http://localhost:5004/veiculos?marca=honda" \
  -H "Authorization: Bearer <token>"
```

**Criar veículo:**
```bash
curl -X POST http://localhost:5004/veiculos \
  -H "Authorization: Bearer <token>" \
  -H "Content-Type: application/json" \
  -d '{"nome":"Civic","marca":"Honda","ano":2024}'
```

## Configuração

### Variáveis de Ambiente

| Variável | Padrão | Descrição |
|----------|--------|-----------|
| `ConnectionStrings__MySql` | `Server=localhost;Database=minimal_api;Uid=root;Pwd=root;` | String de conexão MySQL |
| `Jwt__Key` | - | Chave secreta JWT (mínimo 32 bytes/256 bits) |
| `Jwt__ExpiryDays` | `1` | Dias de validade do token |
| `Cors__AllowedOrigins__*` | `http://localhost:5004` | Origens permitidas no CORS |
| `RateLimiting__PermitLimit` | `10` | Máximo de requisições por janela |
| `RateLimiting__WindowSeconds` | `60` | Janela de tempo em segundos |

### Perfis de Execução

| Perfil | Ambiente | Swagger | CORS |
|--------|----------|---------|------|
| `http` | Development | Habilitado | AllowAnyOrigin |
| `https` | Development | Habilitado | AllowAnyOrigin |
| `Testing` | Testing | Desabilitado | AllowAnyOrigin |
| Production | Production | Desabilitado | Origens configuradas |

## Segurança

- **Senhas:** Hasheadas com BCrypt (nunca armazenadas em texto plano)
- **JWT:** Tokens assinados com HMAC-SHA256, expiração configurável
- **CORS:** Restrito a origens específicas em produção
- **Rate Limiting:** 10 requisições por minuto por IP no login
- **HSTS:** Habilitado em produção
- **HTTPS Redirection:** Middleware de redirecionamento HTTPS
- **Swagger:** Desabilitado fora do ambiente Development
- **Exception Handling:** Middleware global que não expõe stack traces em produção

### Usuário Padrão (Seed Data)

| Email | Senha | Perfil |
|-------|-------|--------|
| administrador@teste.com | 123456 | Adm |

> **IMPORTANTE:** Altere a senha padrão e a chave JWT antes de colocar em produção.

## Funcionalidades

### Backend

- CRUD completo de Administradores e Veículos
- Autenticação JWT com roles (Adm, Editor)
- Validação de dados com FluentValidation
- Paginação em listagens
- Filtros por nome e marca nos veículos
- Logging estruturado com Serilog (console + arquivo)
- Health checks (`/health`)
- Response compression (Brotli + Gzip)
- Rate limiting configurável
- Global exception handling
- Docker multi-stage build
- Testes unitários e de integração (28 testes)

### Frontend

- Interface responsiva com sidebar
- Dark/Light theme toggle (salvo no localStorage)
- Toast notifications com auto-dismiss (3 segundos)
- Autenticação JWT com redirecionamento automático
- Páginas: Login, Dashboard, Veículos, Administradores
- CRUD completo com dialogs de confirmação
- Filtros e paginação
- Proteção de rotas por perfil (Adm/Editor)

## Licença

Este projeto é para fins educacionais.

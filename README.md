# Minimal API — Vehicle Management | Gestão de Veículos

## 🇧🇷 Português

API RESTful construída com **ASP.NET Core 8 Minimal API** para gestão de administradores e veículos, com autenticação JWT, MySQL e testes automatizados. Frontend React com interface responsiva e tema dark/light.

## 🇺🇸 English

RESTful API built with **ASP.NET Core 8 Minimal API** for managing administrators and vehicles, featuring JWT authentication, MySQL, and automated tests. React frontend with responsive design and dark/light theme.

---

## 🔗 Production Links | Links de Produção

| Serviço / Service | URL |
|-------------------|-----|
| **Frontend** | [frontend-ruddy-seven.vercel.app](https://frontend-ruddy-seven-m6v9ufe95y.vercel.app) |
| **Backend API** | [minimal-api-backend.railway.app](https://minimal-api-backend-production.up.railway.app) |
| **Swagger** | [minimal-api-backend.up.railway.app/swagger](https://minimal-api-backend-production.up.railway.app/swagger) |
| **Health Check** | [minimal-api-backend.up.railway.app/health](https://minimal-api-backend-production.up.railway.app/health) |

### Default Credentials | Credenciais Padrão

| Email | Senha / Password | Perfil / Role |
|-------|------------------|---------------|
| administrador@teste.com | 123456 | Adm |

---

## 🛠️ Tech Stack | Pilha Tecnológica

### Backend

| Tecnologia / Technology | Versão | Descrição / Description |
|-------------------------|--------|------------------------|
| .NET | 8.0 LTS | Framework principal (suporte até Nov/2026) |
| ASP.NET Core Minimal API | 8.0 | Web framework |
| Entity Framework Core | 8.0.11 | ORM |
| Pomelo MySQL | 8.0.2 | MySQL provider |
| BCrypt.Net | 4.0.3 | Password hashing |
| FluentValidation | 11.9.0 | DTO validation |
| Serilog | 8.0.3 | Structured logging |
| JWT Bearer | 8.0.11 | Token authentication |
| Swashbuckle | 6.9.0 | Swagger docs |
| MSTest | 3.6.3 | Test framework |

### Frontend

| Tecnologia / Technology | Versão | Descrição / Description |
|-------------------------|--------|------------------------|
| React | 19.x | UI Library |
| TypeScript | 6.x | Static typing |
| Vite | 8.x | Build tool |
| Tailwind CSS | 3.x | Utility-first CSS |
| shadcn/ui | - | UI components |
| React Router | 7.x | Routing |
| Axios | 1.x | HTTP client |
| Lucide React | - | Icons |

---

## 📋 Prerequisites | Pré-requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [MySQL 8.x](https://dev.mysql.com/downloads/mysql/) (or use Docker)
- [Docker](https://www.docker.com/) (optional)

---

## 📁 Project Structure | Estrutura do Projeto

`
minimal-api/
├── Api/
│   ├── Dominio/
│   │   ├── DTOs/                    # Data Transfer Objects
│   │   ├── Entidades/               # Domain entities
│   │   ├── Enuns/                   # Enums (Perfil: Adm, Editor)
│   │   ├── Interfaces/              # Service contracts (async)
│   │   ├── ModelViews/              # Response models
│   │   └── Servicos/                # Service implementations
│   ├── Endpoints/                   # Domain endpoints
│   │   ├── AdministradoresEndpoints.cs
│   │   └── VeiculosEndpoints.cs
│   ├── Infraestrutura/Db/           # DbContext & EF Core config
│   ├── Middleware/                   # Global exception handling
│   ├── Validators/                  # FluentValidation
│   ├── Migrations/                  # EF Core migrations
│   ├── Program.cs                   # Entry point
│   └── minimal-api.csproj
├── frontend/
│   ├── src/
│   │   ├── components/              # React components
│   │   ├── contexts/                # AuthContext, ThemeContext
│   │   ├── pages/                   # Login, Dashboard, Veículos, Admins
│   │   ├── services/                # API services (axios)
│   │   ├── types/                   # TypeScript types
│   │   └── lib/                     # Utilities
│   ├── public/                      # Static assets
│   ├── tailwind.config.js
│   ├── vite.config.ts
│   └── package.json
├── Test/
│   ├── Domain/                      # Unit tests
│   ├── Helpers/                     # WebApplicationFactory setup
│   ├── Mocks/                       # Service mocks
│   └── Requests/                    # Integration tests (HTTP)
├── Dockerfile                       # Multi-stage build
├── docker-compose.yml               # Full infrastructure
└── .env.example
`

---

## 🚀 How to Run | Como Rodar

### Option 1: Manual (Development)

**Backend:**
`ash
cd Api
dotnet restore
dotnet run
`

**Frontend** (separate terminal):
`ash
cd frontend
npm install
npm run dev
`

| URL | Descrição / Description |
|-----|------------------------|
| http://localhost:3000 | Frontend (React) |
| http://localhost:5004 | API |
| http://localhost:5004/swagger | Swagger UI |
| http://localhost:5004/health | Health Check |

### Option 2: Docker Compose

`ash
docker-compose up --build
`

- API: http://localhost:8080
- Swagger: http://localhost:8080/swagger
- MySQL: localhost:3306 (user: oot, password: oot)

`ash
docker-compose down       # Stop
docker-compose down -v    # Stop and clear data
`

---

## 🧪 Running Tests | Testes

`ash
# Unit tests (InMemory DB, no MySQL needed)
dotnet test

# With code coverage
dotnet test --collect:"XPlat Code Coverage"
`

---

## 📡 API Endpoints

### Authentication | Autenticação

| Method | Endpoint | Description | Auth |
|--------|----------|-------------|------|
| POST | /administradores/login | Login, returns JWT | No |

**Request:**
`json
{ "email": "administrador@teste.com", "senha": "123456" }
`

**Response (200):**
`json
{
  "email": "administrador@teste.com",
  "perfil": "Adm",
  "token": "eyJhbGciOiJIUzI1NiIs..."
}
`

### Administrators (requires Adm role)

| Method | Endpoint | Description | Auth |
|--------|----------|-------------|------|
| GET | /administradores | List all (paginated) | JWT + Adm |
| GET | /administradores/{id} | Get by ID | JWT + Adm |
| POST | /administradores | Create new | JWT + Adm |

### Vehicles (requires Adm or Editor)

| Method | Endpoint | Description | Auth |
|--------|----------|-------------|------|
| GET | /veiculos | List all (paginated, filter by name/brand) | JWT |
| GET | /veiculos/{id} | Get by ID | JWT |
| POST | /veiculos | Create new | JWT |
| PUT | /veiculos/{id} | Update | JWT + Adm |
| DELETE | /veiculos/{id} | Delete | JWT + Adm |

### cURL Examples

`ash
# Login
curl -X POST http://localhost:5004/administradores/login \
  -H "Content-Type: application/json" \
  -d '{"email":"administrador@teste.com","senha":"123456"}'

# List vehicles
curl http://localhost:5004/veiculos \
  -H "Authorization: Bearer <token>"

# Filter by brand
curl "http://localhost:5004/veiculos?marca=honda" \
  -H "Authorization: Bearer <token>"

# Create vehicle
curl -X POST http://localhost:5004/veiculos \
  -H "Authorization: Bearer <token>" \
  -H "Content-Type: application/json" \
  -d '{"nome":"Civic","marca":"Honda","ano":2024}'
`

---

## 🔐 Security | Segurança

| Feature | Descrição / Description |
|---------|------------------------|
| **BCrypt** | Senhas hasheadas, nunca em texto plano / Hashed passwords, never plaintext |
| **JWT HMAC-SHA256** | Tokens assinados com expiração configurável / Signed tokens, configurable expiry |
| **CORS** | Restrito a origens específicas em produção / Restricted origins in production |
| **Rate Limiting** | 10 requisições/minuto por IP no login / 10 requests/minute per IP on login |
| **HSTS** | Habilitado em produção / Enabled in production |
| **HTTPS** | Redirecionamento automático / Automatic redirect |
| **Swagger** | Desabilitado fora de Development / Disabled outside Development |
| **Exception Handling** | Middleware global sem stack traces em produção / No stack traces in production |

---

## ⚙️ Configuration | Configuração

### Environment Variables | Variáveis de Ambiente

| Variable | Default | Description |
|----------|---------|-------------|
| ConnectionStrings__MySql | Server=localhost;Database=minimal_api;Uid=root;Pwd=root; | MySQL connection string |
| Jwt__Key | - | JWT secret key (min 32 bytes/256 bits) |
| Jwt__ExpiryDays | 1 | Token expiry in days |
| Cors__AllowedOrigins__* | http://localhost:5004 | Allowed CORS origins |
| RateLimiting__PermitLimit | 10 | Max requests per window |
| RateLimiting__WindowSeconds | 60 | Time window in seconds |

### Execution Profiles | Perfis de Execução

| Profile | Swagger | CORS |
|---------|---------|------|
| Development | Enabled | AllowAnyOrigin |
| Testing | Disabled | AllowAnyOrigin |
| Production | Disabled | Configured origins |

---

## 📬 Contact | Contato

**Marcus Lafaiete** — [GitHub](https://github.com/marcuslaf) · [LinkedIn](https://www.linkedin.com/in/marcuslaf)

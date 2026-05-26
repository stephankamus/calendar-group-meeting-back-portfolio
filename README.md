# A gente combina: API de Calendário para Reuniões em Grupo

API REST desenvolvida em ASP.NET Core 8 para gerenciamento de eventos e reuniões em grupo.

O projeto foi desenvolvido com foco em arquitetura limpa, containerização, segurança e facilidade de deploy, utilizando PostgreSQL, Docker e Minimal APIs.

---

## ✨ Features

* CRUD de eventos
* Minimal APIs com ASP.NET Core 8
* Entity Framework Core
* PostgreSQL
* Swagger/OpenAPI
* Middleware global de tratamento de erros
* Rate Limiting
* Health Checks
* CORS configurável
* Migrations automáticas
* Docker e Docker Compose

---

## 🛠️ Tecnologias

* ASP.NET Core 8
* Entity Framework Core 8
* PostgreSQL
* Docker
* Docker Compose
* Swagger / OpenAPI
* Npgsql
* AspNetCoreRateLimit

---

## 📦 Arquitetura

O projeto segue uma estrutura modular separando:

* API
* Application
* Infraestrutura
* Persistência
* Middlewares
* Dependency Injection

Além disso:

* Configurações via variáveis de ambiente
* Banco inicializado automaticamente com migrations
* Containers preparados para ambiente de produção

---

# 🚀 Como executar

## Pré-requisitos

* Docker
* Docker Compose

ou

* .NET 8 SDK
* PostgreSQL

---

# 🐳 Executando com Docker

Clone o projeto:

```bash
git clone https://github.com/stephankamus/calendar-group-meeting-back-portfolio.git
cd calendar-group-meeting-back-portfolio
```

Crie o arquivo `.env` seguindo o `.env.example`.

Suba os containers:

```bash
docker compose up --build
```

API disponível em:

```text
http://localhost:8080
```

---

# 💻 Executando localmente

## Restaurar dependências

```bash
dotnet restore
```

## Aplicar migrations

```bash
dotnet ef database update
```

## Executar aplicação

```bash
dotnet run
```

---

# 🔧 Variáveis de Ambiente

| Variável          | Descrição                  |
| ----------------- | -------------------------- |
| POSTGRES_USER     | Usuário do PostgreSQL      |
| POSTGRES_PASSWORD | Senha do PostgreSQL        |
| POSTGRES_DB       | Nome do banco              |
| POSTGRES_PORT     | Porta do PostgreSQL        |
| API_PORT          | Porta da API               |
| ALLOWED_ORIGINS   | Origins permitidas no CORS |

---

# 📚 Swagger

Em ambiente de desenvolvimento:

```text
http://localhost:8080/swagger
```

---

# ❤️ Health Check

Endpoint:

```text
GET /health
```

---

# 🐳 Docker

O projeto possui:

* Dockerfile multi-stage
* Execução com usuário não-root
* Containers isolados em network bridge
* Persistência de dados via volume Docker

---

# 📁 Estrutura do Projeto

```text
CalendarGroupMeeting/
 ├── API/
 ├── Application/
 ├── Domain/
 ├── Infra/
 └── Middleware/
```

---

# 🔒 Segurança

* Rate limiting por IP
* Containers executando sem privilégios root
* Tratamento global de exceções
* Configuração por variáveis de ambiente
* CORS configurável

---

# 📄 Licença

Este projeto está licenciado sob a MIT License.

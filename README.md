# proj-ageis-4
Projeto voltado para fabrica de projetos ageis

## Pré-requisitos

Antes de executar o projeto localmente, certifique-se de possuir:

- [.NET SDK 9.0](https://dotnet.microsoft.com/download/dotnet/9.0).
- [PostgreSQL](https://www.postgresql.org/download/).

## Execução local
### 1. Configure os User Secrets

```bash
cd backend/WarehouseManagementService/WarehouseManagementService.Api

dotnet user-secrets set "ConnectionStrings:Default" "Host=localhost;Port=5432;Database=WarehouseManagementDB;Username=SEU_USUARIO;Password=SUA_SENHA"
```

Substitua as credenciais pelos dados da sua instalação do PostgreSQL.

### 2. Execute o Seeder

A partir da pasta do Seeder, execute:

```bash
dotnet run
```

O processo realiza a aplicação das migrations do Entity Framework Core e executa a população inicial do banco de dados.

### 3. Execute os serviços

Abra dois terminais na raiz do repositório.

**Terminal 1 — Warehouse Management API**

```bash
dotnet run --project backend/WarehouseManagementService/WarehouseManagementService.Api
```

**Terminal 2 — Path Tracking Service**

```bash
dotnet run --project CAMINHO_DO_SERVICO_DE_ROTEIRIZACAO
```

Os serviços se comunicam por meio de gRPC, portanto, ambos devem estar disponíveis para o funcionamento completo da aplicação.

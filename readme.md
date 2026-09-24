# EquipmentInventory

> Automation system for inventory and accounting of computer equipment in an organization.

![preview](https://raw.githubusercontent.com/Switrue/my-assets/main/Projects/gif/preview/eq-inv-app-preview.gif)

---

## About the project
**EquipmentInventory** is a comprehensive solution for automating the processes of inventory and accounting of computer equipment. Consists of a desktop application (WPF), a server API and a set of supporting utilities.

### Features

- Accounting for computer hardware and equipment
- Inventory with reporting
- Differentiation of access rights (JWT-
authentication)
- Encryption of sensitive data
- Automatic installation of system components

---

## Solution structure

| Project | Type | Description |
|--------|-----|----------|
| **DataAccess.Postgres** | Library | PostgreSQL Database Migrations and Context |
| **EquipmentInventory.API** | ASP.NET Core API | REST API for connecting UI to database |
| **EquipmentInventory.UI** | WPF (.NET Framework 4.8) | Desktop Application |
| **EquipmentInventory.UI.Tests** | Unit tests | Desktop Application Tests |
| **SecurityToolkit** | Console (.NET 8) | Generating encryption keys and codes |
| **Setup** | Installer | Building the UI installer |
| **Setup.API** | Installer | Building the API Installer |

```text
EquipmentInventory.sln
├── src/
│ ├── DataAccess.Postgres/         # EF Core + PostgreSQL
│ ├── EquipmentInventory.API/      # ASP.NET Core Web API
│ ├── EquipmentInventory.UI/       # WPF client
│ ├── EquipmentInventory.UI.Tests/ # Unit tests
│ └── SecurityToolkit/             # Security utility
└── setup/
    ├── Setup/                     # Installer UI
    └── Setup.API/                 # API installer
```

___

## Technology stack

### Pivot table

| Project | Platform | Framework | Key packages |
|--------|----------|-----------|-----------------|
| **DataAccess.Postgres** | .NET 8.0, C# | EF Core | EF Core 8.0.1, Npgsql 8.0.0 |
| **EquipmentInventory.API** | .NET 8.0, C# | ASP.NET Core | JwtBearer, Swashbuckle, BCrypt |
| **EquipmentInventory.UI** | .NET Framework 4.8, C# | WPF + XAML | MaterialDesign 3 |
| **EquipmentInventory.UI.Tests** | .NET Framework 4.8, C# | Unit-test | — |
| **SecurityToolkit** | .NET 8.0, C# | Console | — |

### Project details

#### DataAccess.Postgres

- **Platform**: .NET 8.0, C#
- **Framework**: ASP.NET Core
- **DBMS**: PostgreSQL 15
- **NuGet**:
  - `Microsoft.EntityFrameworkCore` - 8.0.1
  - `Microsoft.EntityFrameworkCore.Design` - 8.0.0 (dev-only)
  - `Microsoft.EntityFrameworkCore.Tools` - 8.0.0 (dev-only)
  - `Npgsql.EntityFrameworkCore.PostgreSQL` - 8.0.0

#### EquipmentInventory.API

- **Platform**: .NET 8.0, C#
- **Framework**: ASP.NET Core Web API
- **NuGet**:
  - `BCrypt.Net-Next` - 4.0.3
  - `Microsoft.AspNetCore.Authentication.JwtBearer` - 7.0.2
  - `Swashbuckle.AspNetCore` - 6.6.2
  - `System.IdentityModel.Tokens.Jwt` - 7.1.2

#### EquipmentInventory.UI

- **Platform**: .NET Framework 4.8, C#
- **UI**: WPF, XAML
- **Design system**: MaterialDesign 3

#### EquipmentInventory.UI.Tests

- **Platform**: .NET Framework 4.8, C#
- **Purpose**: Unit tests of UI logic

#### SecurityToolkit

- **Platform**: .NET 8.0, C#
- **Type**: Console application
- **Purpose**: generation of encryption keys and codes

___

## Configuration

### Server component (API)

Create a file `appsettings.json` in the root of the `EquipmentInventory.API` project:
```json
{
  "ConnectionStrings": {
    "EquipmentInventoryDbContext": "Host=localhost;Port=5432;Database=eq_inv_new;Username=postgres;Password=12345;Timeout=15;"
  },
  "AppSettings": {
    "HostUrl": "https://localhost:7278;http://localhost:7279"
  },
  "Jwt": {
    "Key": "iE73yVTkJoNR7PSdNIJ8XnIh/mmz2c3r7gyhaFf0wzM=",
    "Expires": 30
  },
  "Codes": {
    "ResetAdminPassword": {
      "DefaultValue": "123456",
      "Code": "LI2L-UYYB-G6Y3-WWX9"
    },
    "ResetArchive": {
      "Code": "J2N5-M249-P2HM-6VII"
    }
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}

```

### Client component (UI)

Create a file `appsettings.json` in the root of the `EquipmentInventory.UI` project:
```json
{
  "ApiSettings": {
    "BaseUrl": "https://localhost:7278"
  }
}
```

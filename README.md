# 🔨 AuctionNow - Plataforma de Subastas en Tiempo Real

AuctionNow es una aplicación web de subastas desarrollada para la materia **Proyecto de Software**.

El sistema permite crear subastas, realizar pujas, administrar una billetera virtual, aplicar reglas de anti-sniping y actualizar la sala de subasta en tiempo real.

---

## 🏗️ Arquitectura y Tecnologías

### Backend
- .NET 8
- ASP.NET Core Web API
- Entity Framework Core
- SQL Server
- Clean Architecture / Arquitectura Hexagonal
- Repository Pattern
- Unit of Work
- DTOs y Mappers
- SignalR
- Background Worker
- Optimistic Locking con RowVersion

### Frontend
- Blazor WebAssembly
- MudBlazor
- Consumo de API REST
- SignalR para actualizaciones en tiempo real

El Frontend se encuentra desacoplado del Backend.

---

## 📁 Estructura

```text
AuctionNow
│
├── Backend
│   ├── Domain
│   ├── Application
│   ├── Infrastructure
│   └── AuctionNow
│
├── Frontend
│   └── Front_AuctionNow
│
├── AuctionNow.sln
└── README.md

🚀 Ejecución

1. Configurar Base de Datos

Verificar la cadena de conexión en appsettings.json.

2. Ejecutar Migraciones

Desde la Consola del Administrador de Paquetes:
Update-Database

3. Ejecutar Backend

API:
https://localhost:7287

Swagger:
https://localhost:7287/swagger

4. Ejecutar Frontend
https://localhost:7149

📋 Funcionalidades Principales

Catálogo de subastas con filtros y paginación
Creación de subastas
Sistema de pujas
Billetera virtual
Retención y liberación de fondos
Historial de movimientos
Anti-sniping
Actualización en tiempo real con SignalR
Worker para activar y finalizar subastas
Mis Pujas
Mis Publicaciones
Auditoría de eventos

💰 Billetera
La billetera administra:

Saldo Total
Saldo Retenido
Saldo Disponible

Cuando un usuario realiza la puja más alta, el monto queda retenido.
Si otro usuario lo supera, el saldo anterior se libera automáticamente.

⏱️ Anti-Sniping

Si una puja válida se realiza dentro de los últimos 60 segundos, la subasta se extiende automáticamente 2 minutos.

🔒 Concurrencia

El sistema utiliza concurrencia optimista mediante RowVersion.

Si dos operaciones intentan modificar el mismo registro al mismo tiempo, Entity Framework Core genera un conflicto y la API responde:

409 Conflict

Se realizó una prueba de concurrencia con dos pujas simultáneas, donde una operación fue aceptada y la otra recibió 409 Conflict.

🔄 Background Worker
El Worker se encarga de:

Activar subastas programadas
Finalizar subastas vencidas
Marcar subastas sin ofertas como Desierta
Liquidar fondos entre comprador y vendedor
Registrar auditoría


📡 SignalR
SignalR permite actualizar en tiempo real:

Puja actual
Historial de ofertas
Estado Liderando / Superado
Fecha de finalización
Temporizador de la subasta

✅ Conceptos Técnicos Implementados

Clean Architecture
Arquitectura Hexagonal
Entity Framework Core
Code First
Transacciones
Unit of Work
Optimistic Locking
RowVersion
CancellationToken
SignalR
Background Worker
DTOs
Mappers
Manejo de errores HTTP
Frontend desacoplado

👥 Integrantes
Agustín Firpo
Malena Belen Cadavid

Proyecto desarrollado para la materia Proyecto de Software.

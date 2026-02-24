# LegoStore

Tool set for a better BrickLink store experience.

# 🧱 BrickLink Storage Automation

## 📑 Table of Contents

- [BrickLink Storage Automation](#bricklink-storage-automation)
  - [Problem Statement](#problem-statement)
  - [Desired Outcome](#desired-outcome)
  - [Storage UI](#storage-ui)
    - [Container representation](#container-representation)
    - [Case group representation](#case-group-representation)
    - [Cabinet representation](#cabinet-representation)
 
  - [Storage Domain Models](#storage-domain-models)
    - [Sections](#sections)
    - [Container Types](#container-types)
    - [Cases](#cases)
    - [Case Groups](#case-groups)
    - [Cabinets](#cabinets)
    - [Store Storage](#store-storage)

---

## Problem Statement

Managing physical LEGO inventory for a BrickLink store requires precise tracking of where parts are stored and how quantities change after each order is picked.

Orders are processed using BrickStore, where each item in the order is represented as a **Lot**. After the picking process is completed, the exported order file reflects the exact quantities that were removed from storage.

However, updating the physical storage state based on this data is currently a manual process.

---

### Current Workflow

1. An order is received via the BrickLink store.
2. The order is imported into BrickStore as a `.bsx` (XML) file.
3. The order is physically picked from storage.
4. After picking is completed, the processed file is saved locally.
5. Empty containers must be updated manually.

---

### Data Available After Picking

From the exported `.bsx` file, the system can reliably extract:

* **LotId** — unique identifier of the lot
* **Quantity** — quantity removed from storage

---

### Key Pain Points

* ❌ Time-consuming spreadsheet updates
* ❌ Risk of human error in empty storage space tracking
* ❌ No automatic empty container detection
* ❌ Difficult restocking planning

As order volume grows, maintaining accurate storage data becomes increasingly inefficient and error-prone.

---

## Desired Outcome

An automated system that:

* Reads processed BrickStore `.bsx` files
* Extracts LotId and Quantity
* Deducts quantities from storage automatically
* Detects empty sections and containers
* Maintains an accurate digital model of the warehouse
* Provides a visual web interface of storage

---

## Storage UI

A simple UI representation should be added as part of this project. 
The main goal is to track empty containers/sections in the store to enable prediction of how many additional lots can be added to the cabinets.

<img width="409" height="347" alt="image" src="https://github.com/user-attachments/assets/bd0296f8-12a2-4fcd-9133-55f8546f337b" />

### Container representation

- Containers are displayed as rectangles.
- Aspect ratio: 3:1 (length : width).
- All container types have the same dimensions.
- Label: Each container has a label underneath, e.g., #1001, #1023, #9999.
- Color:
  - Full green for fully empty sections.
  - Partially green (proportional fill) for any container with at least one free section.
- Spacing: Containers are separated by a small margin for readability.

### Case group representation

- Case group should be shown as a flexible grid
- Maximum grid dimensions depend on nested container types:
  - PX12 : 12 columns x 9 rows
  - PX6 : 6 x 9
  - PX4 : 6 x 6
  - PX2 : 6 x 3
  
### Cabinet representation

- Each cabinet is displayed as a tab.
- Clicking a tab shows the Case groups/Containers within that cabinet.
# Storage Domain Models

## Overview

The storage system represents the physical organization of LEGO parts used in the BrickLink store.

It models the real warehouse structure:

```
Store → Cabinets → Groups → Cases → Containers → Sections
```

Each level reflects real physical constraints and capacity rules.

---

### Sections

A **Section** is the smallest storage unit.

**Properties**

* Holds exactly 1 **LotId** + **Quantity**
* Can be empty or occupied

---

### Container Types

Containers store LEGO parts.
Each container belongs to a **Container Type** which defines its section capacity.

**Types**

* **PX12** — Has 3 sections
* **PX6** — Has 1 section
* **PX4** — Has 1 section
* **PX2** — Has 1 section

---

### Cases

Containers are mounted inside **Cases**.

**Rules**

* All cases have identical physical size
* Capacity depends on container type

| Container Type | Containers per Case |
| -------------- | ------------------- |
| PX12           | 12                  |
| PX6            | 6                   |
| PX4            | 4                   |
| PX2            | 2                   |

A case can store **only one container type**.

---

### Case Groups

Cases are assembled into **Groups**.

**Group Size**

```
3 × 3 Cases Grid
```

Each group contains:

```
9 Cases
```

Example layout:

```
[ C ][ C ][ C ]
[ C ][ C ][ C ]
[ C ][ C ][ C ]
```

---

### Cabinets

Groups are placed into **Cabinets**.

**Capacity**

```
1 Cabinet = 4 Groups
```

Example:

```
Cabinet
 ├── Group A
 ├── Group B
 ├── Group C
 └── Group D
```

---

### Store Storage

The full warehouse consists of:

```
N Cabinets
```

Where **N** is configurable and depends on the shop size.

---

## Getting Started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [SQL Server Express](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) or SQL Server LocalDB (included with Visual Studio)
- [Node.js 18+](https://nodejs.org/) (for the React frontend)
- (Optional) [IIS or IIS Express](https://www.iis.net/) for hosting the API

---

### 1. Database Setup

The project uses **SQL Server Express / LocalDB** with EF Core migrations.

#### a) Using LocalDB (recommended for development)

The default connection string in `legostore/backend/Api/appsettings.json` targets LocalDB:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=LegoStore;Trusted_Connection=True;"
}
```

No additional SQL Server installation is needed if you have Visual Studio 2022 or the LocalDB standalone installer.

#### b) Using SQL Server Express

Update the connection string in `appsettings.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=.\\SQLEXPRESS;Database=LegoStore;Trusted_Connection=True;"
}
```

#### c) Apply EF Core Migrations

Open a terminal in `legostore/backend/` and run:

```bash
dotnet ef database update --project infrastructure --startup-project Api
```

This creates the **LegoStore** database with all required tables.

#### d) Seed Initial Data (optional)

You can use the API's `GET /api/storage` to verify the DB is ready. To pre-populate storage data, either write a seed script or use the `SaveAsync` method via a one-time console app.

---

### 2. Running the Backend API Locally

#### Using `dotnet run`

```bash
cd legostore/backend/Api
dotnet run
```

The API will be available at `http://localhost:5000` (or `https://localhost:5001`).  
Swagger UI: `http://localhost:5000/swagger`

#### Using IIS Express (Visual Studio)

1. Open `legostore/backend/LegoStore.sln` in Visual Studio.
2. Set **LegoStore.Api** as the startup project.
3. Press **F5** or click **IIS Express** in the toolbar.
4. Visual Studio will launch IIS Express and open the browser.

To configure the IIS Express port, edit `legostore/backend/Api/Properties/launchSettings.json`.

---

### 3. Running the React Frontend Locally

```bash
cd legostore/Frontend
npm install
npm run dev
```

The frontend runs at `http://localhost:5173` and proxies `/api` requests to `http://localhost:5000` (the backend API).

---

### 4. Incoming Orders Path

The **Update Storage State** button reads `.xml` files from:

```
C:\Lego\Bricklink\Incoming orders
```

You can override this path in `appsettings.json`:

```json
"Storage": {
  "IncomingOrdersPath": "C:\\Lego\\Bricklink\\Incoming orders"
}
```

Create the folder and place exported BrickStore `.bsx` (XML) files there before clicking the button.

---

# LegoStore

Tool set for a better BrickLink store experience.

# 🧱 BrickLink Storage Automation

## 📑 Table of Contents

- [BrickLink Storage Automation](#bricklink-storage-automation)
  - [Problem Statement](#problem-statement)
  - [Desired Outcome](#desired-outcome)
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

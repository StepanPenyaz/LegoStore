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
5. Empty containers must updated manually.

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

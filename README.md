# LegoStore
Tool set for a better BrickLink store experience.
# 🧱 BrickLink Storage Automation

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

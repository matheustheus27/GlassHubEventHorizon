# 🧩 Atomic Components - GlassHubEventHorizon

![Atomic Design Card](https://glass-hub-engine.vercel.app/api/card?title=Atomic+Design+Architecture&description=Atoms%2C+Molecules%2C+Organisms%2C+and+Templates+structuring+decoupled+pure+UI.&tag=ATOMIC+DESIGN&icon=sparkles&width=650&theme=glass-dark)

Faithful to the **GlassHub Ecosystem** guidelines, **GlassHubEventHorizon** structures its entire user interface and presentation layer using the **Atomic Design Methodology**.

Just as matter in the universe is built from fundamental subatomic particles that combine into molecules, cells, and complex living organisms, our interfaces are constructed by composing pure, modular UI components.

---

## 🔬 Atomic Design Hierarchy

```text
[Atoms] ──> [Molecules] ──> [Organisms] ──> [Templates]
```

---

## 🟢 1. Atoms

Atoms are the smallest indivisible UI building blocks. They contain no complex business logic.

| Atom | Description | CLI (`evh`) & GUI Representation |
| :--- | :--- | :--- |
| **`BadgeAtom`** | Renders status tags (`[SUCCESS]`, `[FAIL]`, `[INFO]`, `[GLASSHUB]`). | Formatted colored text in Console / Rounded glass badge in WPF. |
| **`DividerAtom`** | Renders visual separator rule dividers. | `───────────────────` terminal rule line. |
| **`HeaderAtom`** | Displays ASCII art logo, versioning, and ecosystem signature. | Initial execution header banner. |
| **`ProgressBarAtom`**| Renders atomic progress telemetry bar. | `[██████████░░░░] 75%` |

---

## 🟡 2. Molecules

Molecules are combinations of two or more atoms that form a simple functional unit.

| Molecule | Integrated Components | Purpose |
| :--- | :--- | :--- |
| **`FileMetadataMolecule`** | `BadgeAtom` + `DividerAtom` + Labels | Displays formatted archive metadata tables (sizes, compression %, applied engine). |
| **`CommandOptionsMolecule`** | `BadgeAtom` + Option Labels | Displays parsed command arguments passed by the user. |
| **`LanguageToggleMolecule`** | Language Buttons (`en-US` / `pt-BR`) | Enables dynamic runtime culture switching. |

---

## 🔴 3. Organisms

Organisms are higher-level components that orchestrate molecules and atoms to execute complete workflow tasks.

| Organism | Responsibility |
| :--- | :--- |
| **`ArchiveCompressOrganism`** | Verifies input paths, displays real-time progress telemetry, triggers compression engines, and outputs final summaries. |
| **`ArchiveExtractOrganism`** | Manages extraction output directories, password handling, and decompression validation. |
| **`ArchiveInspectorOrganism`** | Performs content listing (`list`), metadata inspection (`info`), and integrity verification (`verify`). |

---

## 🔵 4. Templates

Templates provide layout frameworks and application shells.

| Template | Purpose |
| :--- | :--- |
| **`GlassConsoleTemplate`** | Wraps CLI terminal output with the official header, ecosystem signature, and active culture footer. |
| **`MainWindow` (GUI)** | Desktop WPF glassmorphic application window with dark background, dual-pane panels, and telemetry status footer. |

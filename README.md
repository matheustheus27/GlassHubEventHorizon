<div align="center">

  <a href="https://glass-hub-engine.vercel.app/api/logo?project=eventhorizon&animated=true">
    <img src="https://glass-hub-engine.vercel.app/api/logo?project=eventhorizon&animated=true&width=180&height=180" alt="GlassHub Event Horizon Animated Logo" width="180" height="180" />
  </a>

  # GlassHub Event Horizon

  <p><strong>High-performance stream-based file compression, extraction, and telemetry tool built for the GlassHub Ecosystem.</strong></p>

  <p>
    <a href="https://glass-hub-engine.vercel.app/api/badge?label=GlassHub&value=Ecosystem&icon=glasshub&theme=glass-dark&glow=true">
      <img src="https://glass-hub-engine.vercel.app/api/badge?label=GlassHub&value=Ecosystem&icon=glasshub&theme=glass-dark&glow=true" alt="GlassHub Ecosystem" />
    </a>
    <a href="https://glass-hub-engine.vercel.app/api/badge?label=.NET&value=v8.0&icon=csharp&theme=glass-dark&glow=true">
      <img src="https://glass-hub-engine.vercel.app/api/badge?label=.NET&value=v8.0&icon=csharp&theme=glass-dark&glow=true" alt=".NET 8.0" />
    </a>
    <a href="https://glass-hub-engine.vercel.app/api/badge?label=CLI&value=evh&icon=vscode&theme=glass-dark&glow=true">
      <img src="https://glass-hub-engine.vercel.app/api/badge?label=CLI&value=evh&icon=vscode&theme=glass-dark&glow=true" alt="evh CLI" />
    </a>
    <a href="https://glass-hub-engine.vercel.app/api/badge?label=Engine&value=Dual--Fallback&icon=gear&theme=glass-dark&glow=true">
      <img src="https://glass-hub-engine.vercel.app/api/badge?label=Engine&value=Dual--Fallback&icon=gear&theme=glass-dark&glow=true" alt="Dual Fallback Engine" />
    </a>
  </p>

</div>

---

## 🌐 Project Overview

![Overview Card](https://glass-hub-engine.vercel.app/api/card?title=GlassHub+Event+Horizon&description=High-Performance+Stream-Based+Compression+%26+Telemetry+Engine+with+Clean+Architecture+and+Atomic+Design.&tag=Cosmic+Arch&icon=sparkles&theme=glass-dark)

**GlassHubEventHorizon** is a high-performance stream-based desktop application and CLI tool for specialized file compression, extraction, and telemetry inspection. Architected with **Clean Architecture**, **Atomic Component Design** (Atoms, Molecules, Organisms, Templates), a smart dual-engine fallback system (*Native .NET* + *7-Zip CLI*), and dynamic dual-language i18n (`pt-BR` / `en-US`).

---

## ⚡ Core Engineering Features

![Features Card](https://glass-hub-engine.vercel.app/api/card?title=Core+Engineering+Pillars&description=Dual-Engine+Fallback+%7C+Atomic+CLI+(evh)+%26+GUI+%7C+AES-256+Encryption+%7C+Dual-Language+i18n&tag=FEATURES&icon=gear&theme=glass-dark)

- 📦 **Smart Dual-Engine Fallback:** Operates 100% *out-of-the-box* via native `.NET` engine (`System.IO.Compression`), seamlessly leveraging the *7-Zip CLI* for advanced formats (`.7z`, `.rar`) or password protection.
- 🧩 **Atomic Component Architecture:** CLI and GUI interfaces organized strictly into *Atoms*, *Molecules*, *Organisms*, and *Templates*.
- 🌐 **i18n Dual-Language Support:** Runtime culture switching between English (`en-US`) and Portuguese (`pt-BR`).
- 📊 **Complete Telemetry Inspection:** Calculates compression ratio (%), entry counts, structural integrity verification, and applied engine metadata.

---

## 🚀 Command Line Interface (`evh` Quickstart)

The official command-line executable is short, memorable, and easy to run: **`evh`**.

```bash
# Compress files or directories (using positional or flag syntax)
evh compress -i data/ -o backup.zip
evh -c -i data/ -o backup.zip

# Extract archive contents
evh extract -f backup.zip -d output/
evh -x -f backup.zip -d output/

# Display telemetry & metadata inspection
evh info -f backup.zip
evh --info -f backup.zip

# List internal archive entries
evh list -f backup.zip
evh --list -f backup.zip

# Verify structural integrity
evh verify -f backup.zip
evh -v -f backup.zip

# View full help manual
evh help
evh --help
```

### Desktop GUI Application:
```bash
dotnet run --project src/GlassHub.EventHorizon.GUI
```

---

## 📚 Technical Documentation (`docs/`)

All technical documentation is written in clear, accessible, and didactic language for developers across all levels (from intern to staff engineer):

![Documentation Table](https://glass-hub-engine.vercel.app/api/table?title=Technical+Documentation&columns=Guide,Description,File&rows=Architecture,Clean+Architecture+%26+Dual+Engine,docs%2Farchitecture.md;Components,Atomic+Design+Architecture,docs%2Fcomponents.md;Execution,Build+Guide+%26+evh+CLI+Manual,docs%2Fexecution.md;i18n+%26+Design,Localization+%26+Glassmorphic+Theme,docs%2Fi18n-and-design.md&width=820&col_widths=22,50,28&theme=glass-dark)

- [📐 **Architecture Guide (`docs/architecture.md`)**](docs/architecture.md) — Clean Architecture, SOLID principles, and dual engine design.
- [🧩 **Atomic Components (`docs/components.md`)**](docs/components.md) — Atoms, Molecules, Organisms, and Templates breakdown.
- [🚀 **Execution & CLI Guide (`docs/execution.md`)**](docs/execution.md) — Build steps, local execution, and complete `evh` CLI manual.
- [🌐 **i18n & Design System (`docs/i18n-and-design.md`)**](docs/i18n-and-design.md) — Dual language localization and GlassHub visual identity.

---

## ⚖️ Software License

Published under the **Proprietary, Source-Available, Non-Commercial License**. Read the `LICENSE` file for complete terms.

---

<div align="center">
  <sub>Forged with ❤️ by <b>Matheus Ferreira</b> — GlassHub Ecosystem © 2026</sub>
</div>

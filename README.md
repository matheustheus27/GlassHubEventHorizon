<div align="center">

  <a href="https://github.com/matheustheus27/GlassHubQuasar">
    <img src="https://glasshub-quasar.vercel.app/api/logo?project=eventhorizon&animated=true&width=180&height=180" alt="GlassHub Event Horizon Animated Logo" width="180" height="180" />
  </a>

  # 🕳️ GlassHub Event Horizon

  <p><strong>High-Performance Stream-Based File Compression, Extraction, and Telemetry Desktop Application &amp; CLI for the GlassHub Ecosystem.</strong></p>

  <p>
    <a href="https://github.com/matheustheus27/GlassHubQuasar">
      <img src="https://glasshub-quasar.vercel.app/api/badge?label=GlassHub&value=Ecosystem&icon=glasshub&theme=glass-dark&glow=true" alt="GlassHub Ecosystem" />
    </a>
    <a href="https://dotnet.microsoft.com/">
      <img src="https://glasshub-quasar.vercel.app/api/badge?label=.NET&value=v8.0&icon=csharp&theme=glass-dark&glow=true" alt=".NET 8.0" />
    </a>
    <a href="https://github.com/matheustheus27/GlassHubEventHorizon">
      <img src="https://glasshub-quasar.vercel.app/api/badge?label=GUI&value=Windows11--WPF&icon=windows&theme=glass-dark&glow=true" alt="Windows 11 WPF" />
    </a>
    <a href="https://github.com/matheustheus27/GlassHubEventHorizon">
      <img src="https://glasshub-quasar.vercel.app/api/badge?label=CLI&value=evh&icon=vscode&theme=glass-dark&glow=true" alt="evh CLI" />
    </a>
    <a href="https://github.com/matheustheus27/GlassHubQuasar">
      <img src="https://glasshub-quasar.vercel.app/api/badge?label=Powered%20By&value=GlassHub%20Quasar&icon=glasshubquasar&theme=glass-dark&glow=true" alt="Powered By GlassHub Quasar" />
    </a>
  </p>

</div>

---

## 🌐 Project Overview

![Overview Card](https://glasshub-quasar.vercel.app/api/card?title=GlassHub+Event+Horizon&description=High-Performance+Stream-Based+Compression+%26+Telemetry+Engine+with+Cosmic+Glassmorphism.&tag=Cosmic+Arch&icon=sparkles&width=650&theme=glass-dark)

**GlassHubEventHorizon** is a high-performance stream-based desktop application and CLI tool for specialized file compression, extraction, and telemetry inspection. Built with **WPF & .NET 8**, it adopts a native **Windows 11 Cosmic Dark Glassmorphism** visual identity, decoupled **MVVM architecture**, dual-engine fallback (*Native .NET* + *7-Zip CLI*), and dynamic dual-language localization (`pt-BR` / `en-US`).

---

## ⚡ Core Engineering Features

![Features Card](https://glasshub-quasar.vercel.app/api/card?title=Core+Engineering+Pillars&description=Dual-Engine+Fallback+%7C+Windows+11+NavigationView+%7C+1-Click+Presets+%7C+AES-256+Encryption+%7C+Inno+Setup&tag=FEATURES&icon=gear&width=650&theme=glass-dark)

- 🌌 **Windows 11 Cosmic Dark (Glassmorphism):** Deep space backdrop (`#070B13`), glass cards (`#0E1626`), and neon cyan accents (`#00E5FF`) with customizable live themes (*Cosmic Dark*, *Fluent Light*, *WinRAR Cyber*, *Neon Cyberpunk*, *Glass Dark*).
- 🧭 **Modular NavigationView:** Clean lateral navigation split into **Compactar (Compress)**, **Extrair (Extract)**, **Inspetor (Inspect)**, and **Configurações (Settings)**.
- 🎯 **1-Click Quick Presets:** Instant selection pills (*Balanceado*, *Tamanho Mínimo (7z Ultra)*, *Ultra Rápido*, *Sem Perdas / Raw*).
- 🧩 **Progressive Disclosure:** Collapsible expanders for advanced settings: multi-format selection (`.zip`, `.7z`, `.tar`, `.gz`, `.zst`), AES-256 password encryption, volume splitting (CD, DVD, custom), and CPU core/thread allocation sliders.
- 📦 **Smart Dual-Engine Fallback:** Operates 100% *out-of-the-box* via native `.NET` engine (`System.IO.Compression`), seamlessly leveraging the *7-Zip CLI* for advanced formats (`.7z`, `.rar`, `.tar`, `.gz`, `.zst`) or encrypted archives.
- 🛡️ **Complete Telemetry & CRC Inspection:** Calculates compression ratios (%), original uncompressed size, entry count registry, and verifies archive structural integrity.
- 🌐 **Dynamic Dual-Language i18n:** Live switching between Portuguese (`pt-BR`) and English (`en-US`).
- 💿 **Installer & Windows Explorer Integration:** Complete Inno Setup script (`installer.iss`) and automated publishing pipeline (`publish.ps1`) adding context menu actions (*"Abrir / Compactar com GlassHub EventHorizon"*).

---

## 🚀 Quickstart & Commands

### Taskfile (`task`) & Makefile (`make`)

| Command | Equivalent Make | Description |
| :--- | :--- | :--- |
| `task build` | `make build` | Build Debug da solução completa |
| `task build:release` | `make build-release` | Build Release da solução completa |
| `task run:gui` | `make run-gui` | Executar aplicação Desktop WPF Windows 11 |
| `task run:cli ARG="--help"` | `make run-cli ARG="--help"` | Executar utilitário de linha de comando `evh` |
| `task publish` | `make publish` | Publicar binários self-contained single-file (`win-x64`) |
| `task installer` | `make installer` | Gerar instalador Windows oficial via Inno Setup |
| `task clean` | `make clean` | Limpar binários, cache e pastas de publicação |

---

## 💻 CLI Usage (`evh`)

The official command-line executable is short, memorable, and fast: **`evh`**.

```bash
# Compress files or directories into an archive
evh compress -i data/ -o backup.zip
evh -c -i data/ -o backup.zip

# Extract archive contents into a destination folder
evh extract -f backup.zip -d output/
evh -x -f backup.zip -d output/

# Display telemetry & metadata inspection
evh info -f backup.zip
evh --info -f backup.zip

# List internal archive entries
evh list -f backup.zip
evh --list -f backup.zip

# Verify structural integrity (CRC check)
evh verify -f backup.zip
evh -v -f backup.zip

# View full help manual
evh help
evh --help
```

---

## 📦 Automated Build & Installer Pipeline

### 1. Build Single-File Release
```powershell
pwsh -File publish.ps1
```
Generates single-file self-contained executables in `./publish/win-x64/`:
- `GlassHub.EventHorizon.GUI.exe`
- `evh.exe`
- `app.ico` & `logo.png`

### 2. Generate Setup Installer
```powershell
ISCC.exe installer.iss
```
Generates the official installer `setup_output/GlassHubEventHorizon_Setup_v1.0.0.exe`.

---

## 📚 Technical Documentation (`docs/`)

- [📐 **Architecture Guide (`docs/architecture.md`)**](docs/architecture.md) — Clean Architecture, MVVM, and dual engine design.
- [🧩 **Atomic Components (`docs/components.md`)**](docs/components.md) — Atoms, Molecules, Organisms, and Templates breakdown.
- [🚀 **Execution & CLI Guide (`docs/execution.md`)**](docs/execution.md) — Build steps, local execution, and complete `evh` CLI manual.
- [🌐 **i18n & Design System (`docs/i18n-and-design.md`)**](docs/i18n-and-design.md) — Dual language localization and GlassHub visual identity.

---

## 🌌 GlassHub Ecosystem & Visual Components

All dynamic visual badges, telemetry SVG cards, and animated branding in this repository are generated and powered by **[GlassHub Quasar](https://github.com/matheustheus27/GlassHubQuasar)** (formerly *GlassHubEngine*), the official Glassmorphic cosmic widget and SVG rendering engine of the GlassHub ecosystem.

- 🔮 **[GlassHub Quasar Repository](https://github.com/matheustheus27/GlassHubQuasar):** Dynamic Glassmorphic SVG Widgets, Cards & Badges for GitHub & Web.

---

## ⚖️ Software License

This project is licensed under a proprietary source-available non-commercial license. See the [LICENSE](LICENSE) file for complete legal terms.

Copyright (c) 2026 Matheus Ferreira. All rights reserved.


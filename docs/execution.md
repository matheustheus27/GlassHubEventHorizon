# 🚀 Compilation and Execution Guide - GlassHubEventHorizon

![Execution Card](https://glasshub-quasar.vercel.app/api/card?title=Execution+Guide+%26+CLI+Manual&description=Compilation+instructions+and+simplified+syntax+for+the+evh+CLI.&tag=EXECUTION&icon=sparkles&width=650&theme=glass-dark)

This guide provides step-by-step instructions to restore, build, test, and execute **GlassHubEventHorizon** via the Command Line Interface (`evh`) and Desktop GUI.

---

## 🛠️ System Prerequisites

- **.NET SDK:** Version `8.0` or higher (`8.0.x` / `9.0.x`).
- **Operating System:** Windows 10/11, Linux, or macOS.
- *(Optional)* **7-Zip:** Installed on system path for `.7z` format and AES encryption support. If absent, the native `.NET` engine transparently handles all `.zip` operations.

Verify your installed .NET SDK version:
```bash
dotnet --version
```

---

## 📦 Project Compilation

Clone the repository or navigate to the project root:

```bash
cd GlassHubEventHorizon
```

Restore dependencies and compile all solution projects:

```bash
dotnet build src/GlassHub.EventHorizon.CLI/GlassHub.EventHorizon.CLI.csproj
dotnet build src/GlassHub.EventHorizon.GUI/GlassHub.EventHorizon.GUI.csproj
```

---

## 💻 Command Line Interface (`evh` Manual)

The official CLI binary name is short and memorable: **`evh`**. You can execute commands using positional syntax or command flags interchangeably.

```bash
evh <command|flag> [options]
```

### Command Syntax & Flag Equivalents

| Action | Positional Command | Command Flag | Description |
| :--- | :--- | :--- | :--- |
| **Compress** | `evh compress` | `evh --compress` / `evh -c` | Compress files or directories into a target archive. |
| **Extract** | `evh extract` | `evh --extract` / `evh -x` | Extract an archive into a destination directory. |
| **List** | `evh list` | `evh --list` | List internal archive entries and file hierarchy. |
| **Info** | `evh info` | `evh --info` | Display technical telemetry, sizes, ratio, and applied engine. |
| **Verify** | `evh verify` | `evh --verify` / `evh -v` | Test structural integrity and CRC of an archive. |
| **Help** | `evh help` | `evh --help` / `evh -h` | Display the comprehensive CLI help manual. |

---

### 1. Compress Files or Folders (`compress` / `-c`)
Compress a folder or multiple files into a `.zip` or `.7z` archive:
```bash
evh compress -i data/ -o backup.zip
# or using flag syntax
evh -c -i data/ -o backup.zip
```

With password encryption (requires 7-Zip engine):
```bash
evh compress -i report.pdf -o secure.7z -p Secret123
```

### 2. Extract Archive (`extract` / `-x`)
Decompress an archive to a destination directory:
```bash
evh extract -f backup.zip -d output/
# or
evh -x -f backup.zip -d output/
```

### 3. List Archive Entries (`list` / `--list`)
List file entries contained inside an archive:
```bash
evh list -f backup.zip
evh --list -f backup.zip
```

### 4. Display Metadata & Telemetry (`info` / `--info`)
Display archive metadata (compressed size, original size, ratio %, and applied engine):
```bash
evh info -f backup.zip
evh --info -f backup.zip
```

### 5. Verify Integrity (`verify` / `-v`)
Test structural integrity and stream CRC:
```bash
evh verify -f backup.zip
evh -v -f backup.zip
```

### 6. Dynamic Language Switching (`--lang`)
Switch output culture at runtime between English (`en-US`) and Portuguese (`pt-BR`):
```bash
evh info -f backup.zip --lang en-US
evh info -f backup.zip --lang pt-BR
```

---

## 🖥️ Running Desktop Graphical User Interface (GUI)

To launch the desktop WPF application with cosmic glassmorphic aesthetics (Windows):

```bash
dotnet run --project src/GlassHub.EventHorizon.GUI
```

### GUI Features:
- **Dual Pane Layout:** Left side for compression, right side for extraction.
- **Language Selector:** Header buttons for instantaneous culture toggling (`🇺🇸 EN` / `🇧🇷 PT`).
- **Telemetry Footer:** Displays active compression engine status and operation telemetry.

# Makefile — GlassHub Event Horizon
# Equivalente ao Taskfile.yml, usando comandos dotnet CLI nativos.
# Compatível com GNU Make (mingw32-make no Windows ou WSL/Git Bash).

# ─── VARIÁVEIS ────────────────────────────────────────────────────────────────
SOLUTION    := GlassHubEventHorizon.slnx
GUI_PROJ    := src/GlassHub.EventHorizon.GUI/GlassHub.EventHorizon.GUI.csproj
CLI_PROJ    := src/GlassHub.EventHorizon.CLI/GlassHub.EventHorizon.CLI.csproj
CONFIG      := Release
DIST        := publish/win-x64
APP_NAME    := GlassHub.EventHorizon
APP_VERSION := 1.0.0
ISS_SCRIPT  := installer.iss

.PHONY: help restore build build-release test lint format clean \
        run-gui run-cli publish-gui publish-cli publish installer all

# ─── DEFAULT ──────────────────────────────────────────────────────────────────
help:
	@echo ""
	@echo "  GlassHub Event Horizon — Tarefas disponíveis"
	@echo "  ─────────────────────────────────────────────"
	@echo "  make restore        Restaurar pacotes NuGet da solução"
	@echo "  make build          Build Debug da solução"
	@echo "  make build-release  Build Release da solução"
	@echo "  make test           Executar todos os testes"
	@echo "  make lint           Verificar formatação (dotnet format --verify)"
	@echo "  make format         Aplicar formatação automática"
	@echo "  make clean          Limpar bin/ obj/ dist/ publish/ setup_output/"
	@echo "  make run-gui        Rodar a GUI (WPF Windows 11)"
	@echo "  make run-cli ARG=.. Rodar a CLI evh com argumentos opcionais"
	@echo "  make publish-gui    Publicar GUI self-contained (win-x64)"
	@echo "  make publish-cli    Publicar CLI self-contained (win-x64)"
	@echo "  make publish        Publicar GUI + CLI via publish.ps1"
	@echo "  make installer      Gerar instalador Windows (Inno Setup)"
	@echo "  make all            Restore → Build → Test → Publish → Installer"
	@echo ""

# ─── RESTORE ──────────────────────────────────────────────────────────────────
restore:
	@echo "[restore] Restaurando pacotes NuGet..."
	dotnet restore $(SOLUTION)

# ─── BUILD ────────────────────────────────────────────────────────────────────
build: restore
	@echo "[build] Build Debug..."
	dotnet build $(SOLUTION) --no-restore -c Debug

build-release: restore
	@echo "[build-release] Build Release..."
	dotnet build $(SOLUTION) --no-restore -c $(CONFIG)

# ─── TEST ─────────────────────────────────────────────────────────────────────
test: restore
	@echo "[test] Executando testes..."
	dotnet test $(SOLUTION) --no-restore -c $(CONFIG) --logger "console;verbosity=normal"

# ─── LINT / FORMAT ────────────────────────────────────────────────────────────
lint:
	@echo "[lint] Verificando formatação..."
	dotnet format $(SOLUTION) --verify-no-changes --severity warn

format:
	@echo "[format] Aplicando formatação automática..."
	dotnet format $(SOLUTION) --severity warn

# ─── CLEAN ────────────────────────────────────────────────────────────────────
clean:
	@echo "[clean] Limpando artefatos..."
	dotnet clean $(SOLUTION)
	@if exist publish rmdir /s /q publish 2>nul || rm -rf publish
	@if exist dist rmdir /s /q dist 2>nul || rm -rf dist
	@if exist setup_output rmdir /s /q setup_output 2>nul || rm -rf setup_output

# ─── RUN ──────────────────────────────────────────────────────────────────────
run-gui:
	@echo "[run-gui] Iniciando GUI..."
	dotnet run --project $(GUI_PROJ) -c Debug

run-cli:
	@echo "[run-cli] Iniciando CLI com ARG='$(ARG)'..."
	dotnet run --project $(CLI_PROJ) -c Debug -- $(ARG)

# ─── PUBLISH (self-contained, single file, win-x64) ───────────────────────────
publish-gui: build-release
	@echo "[publish-gui] Publicando GUI self-contained..."
	dotnet publish $(GUI_PROJ) \
		-c $(CONFIG) \
		-r win-x64 \
		--self-contained true \
		-p:PublishSingleFile=true \
		-p:IncludeNativeLibrariesForSelfExtract=true \
		-p:EnableCompressionInSingleFile=true \
		-o $(DIST)

publish-cli: build-release
	@echo "[publish-cli] Publicando CLI self-contained..."
	dotnet publish $(CLI_PROJ) \
		-c $(CONFIG) \
		-r win-x64 \
		--self-contained true \
		-p:PublishSingleFile=true \
		-p:IncludeNativeLibrariesForSelfExtract=true \
		-p:EnableCompressionInSingleFile=true \
		-o $(DIST)

publish:
	@echo "[publish] Executando publish.ps1..."
	pwsh -File publish.ps1 -NoInnoSetup

# ─── INSTALLER (Inno Setup) ───────────────────────────────────────────────────
installer:
	@echo "[installer] Gerando instalador Windows via publish.ps1 / Inno Setup..."
	pwsh -File publish.ps1

# ─── ALL ──────────────────────────────────────────────────────────────────────
all: restore build-release test publish
	@echo ""
	@echo "  ✓ Pipeline completo concluído com sucesso."
	@echo "  Binários gerados em: $(DIST)/"
	@echo ""

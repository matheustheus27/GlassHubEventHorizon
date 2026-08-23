# Makefile — GlassHub Event Horizon
# Equivalente ao Taskfile.yml, usando comandos dotnet CLI nativos.
# Compatível com GNU Make (mingw32-make no Windows ou WSL/Git Bash).

# ─── VARIÁVEIS ────────────────────────────────────────────────────────────────
SOLUTION    := GlassHubEventHorizon.slnx
GUI_PROJ    := src/GlassHub.EventHorizon.GUI/GlassHub.EventHorizon.GUI.csproj
CLI_PROJ    := src/GlassHub.EventHorizon.CLI/GlassHub.EventHorizon.CLI.csproj
CONFIG      := Release
DIST        := dist
APP_NAME    := GlassHub.EventHorizon
APP_VERSION := 1.0.0
ISS_SCRIPT  := installer/setup.iss

.PHONY: help restore build build-release test lint format clean \
        run-gui run-cli publish-gui publish-cli publish installer all

# ─── DEFAULT ──────────────────────────────────────────────────────────────────
help:
	@echo ""
	@echo "  GlassHub Event Horizon — Tarefas disponíveis"
	@echo "  ─────────────────────────────────────────────"
	@echo "  make restore        Restaurar pacotes NuGet"
	@echo "  make build          Build Debug da solução"
	@echo "  make build-release  Build Release da solução"
	@echo "  make test           Executar todos os testes"
	@echo "  make lint           Verificar formatação (dotnet format --verify)"
	@echo "  make format         Aplicar formatação automática"
	@echo "  make clean          Limpar bin/ obj/ dist/"
	@echo "  make run-gui        Rodar a GUI (Debug)"
	@echo "  make run-cli ARG=.. Rodar a CLI com argumentos opcionais"
	@echo "  make publish-gui    Publicar GUI self-contained (win-x64)"
	@echo "  make publish-cli    Publicar CLI self-contained (win-x64)"
	@echo "  make publish        Publicar GUI + CLI"
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
	@if exist $(DIST) rmdir /s /q $(DIST) 2>nul || rm -rf $(DIST)

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
		-p:PublishReadyToRun=true \
		-p:AssemblyName=$(APP_NAME).GUI \
		-o $(DIST)/gui

publish-cli: build-release
	@echo "[publish-cli] Publicando CLI self-contained..."
	dotnet publish $(CLI_PROJ) \
		-c $(CONFIG) \
		-r win-x64 \
		--self-contained true \
		-p:PublishSingleFile=true \
		-p:IncludeNativeLibrariesForSelfExtract=true \
		-p:AssemblyName=evh \
		-o $(DIST)/cli

publish: publish-gui publish-cli
	@echo "[publish] GUI + CLI publicados em $(DIST)/"

# ─── INSTALLER (Inno Setup) ───────────────────────────────────────────────────
installer: publish
	@echo "[installer] Gerando instalador Windows (Inno Setup)..."
	@if not exist $(DIST) mkdir $(DIST)
	ISCC /DAppVersion=$(APP_VERSION) /DDistDir=$(DIST) $(ISS_SCRIPT)

# ─── ALL ──────────────────────────────────────────────────────────────────────
all: restore build-release test publish installer
	@echo ""
	@echo "  ✓ Pipeline completo concluído."
	@echo "  Instalador gerado em: $(DIST)/"
	@echo ""

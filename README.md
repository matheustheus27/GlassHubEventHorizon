# 📦 NativeZipTools - Lightweight and High-Performance .NET Compression Tool

## 🌐 Project Overview / Visão Geral do Projeto

* **[EN]** NativeZipTools is a lightweight, high-performance .NET console application developed for specialized file compression operations.
* **[PT]** O NativeZipTools é uma aplicação de console em .NET leve e de alta performance, desenvolvida para operações especializadas de compressão de arquivos.

---

## 🇺🇸 English Version

### About the Project

NativeZipTools is architected with a decoupled, domain-driven approach.

* **Version 1 (Current):** Uses an open-source abstraction layer wrapped around the United SevenZip engine.
* **Version 2 (Future):** Will introduce a proprietary, closed-source, high-efficiency compression algorithm. The core engine will be secured via private compiled assemblies (`.dll`), while the CLI wrapper and interface remain visible to the community for study.

### Software License and Terms

This project is published under a **Proprietary, Source-Available, Non-Commercial License**.

* **Viewing, studying, and modifying code for educational or personal use** is fully permitted.
* **Commercial exploitation, selling, renting, or sublicensing** this software or its derivative works is strictly prohibited.
* **Reverse engineering** of any future compiled closed-source components is strictly forbidden under international copyright frameworks.
* For full details, read the `LICENSE` file.

### How to Contribute

* **Pull Requests are disabled.** Direct code modifications to this repository are rejected.
* The community is encouraged to contribute through **GitHub Issues** and **GitHub Discussions** by reporting bugs, suggesting architecture improvements, or referencing base projects.

---

## 🇧🇷 Versão em Português

### Sobre o Projeto

O NativeZipTools é arquitetado com uma abordagem desacoplada e orientada ao domínio.

* **Versão 1 (Atual):** Utiliza uma camada de abstração aberta integrada ao motor CLI do SevenZip.
* **Versão 2 (Futura):** Introduzirá um algoritmo de compressão proprietário, secreto e de alta eficiência. O núcleo do motor será protegido por assemblies compilados privados (`.dll`), enquanto a interface CLI permanecerá visível para estudo da comunidade.

### Licença de Software e Termos

Este projeto é publicado sob uma **Licença Proprietária, de Código Visível (Source-Available) e Não Comercial**.

* **Visualizar, estudar e modificar o código para uso pessoal ou educacional** é totalmente permitido.
* **Exploração comercial, venda, aluguel ou sublicenciamento** deste software ou de seus trabalhos derivados é estritamente proibida.
* **Engenharia reversa** de quaisquer componentes futuros de código fechado compilados é estritamente proibida sob leis de propriedade intelectual.
* Para detalhes completos, consulte o arquivo `LICENSE`.

### Como Contribuir

* **Pull Requests estão desativados.** Modificações diretas de código neste repositório são rejeitadas.
* A comunidade é convidada a contribuir através do **GitHub Issues** e **GitHub Discussions**, relatando falhas técnicas, sugerindo melhorias de arquitetura ou trazendo referências de outros projetos.

---

## 🛠️ Architecture and Solution Layout

```text
NativeZipTools/
├── src/
│   ├── NativeZipTools.Core/              # Domain interfaces & Localization
│   ├── NativeZipTools.Engine.SevenZip/   # V1 Engine wrapper implementation
│   └── NativeZipTools.CLI/               # Main Console Application
└── LICENSE                               # Legal terms

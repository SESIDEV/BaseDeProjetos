# Tutorial: Como Subir o Sistema no Windows 11

Este tutorial vai guiá-lo passo a passo na instalação e execução do **Base de Projetos do SGI** em um ambiente Windows 11.

---

## 📋 Índice

1. [Pré-requisitos](#1-pré-requisitos)
2. [Instalação do .NET Core 3.1 SDK](#2-instalação-do-net-core-31-sdk)
3. [Instalação do MySQL Server](#3-instalação-do-mysql-server)
4. [Instalação do Git](#4-instalação-do-git)
5. [Instalação da IDE (Visual Studio ou VS Code)](#5-instalação-da-ide)
6. [Clone do Projeto](#6-clone-do-projeto)
7. [Configuração do Banco de Dados](#7-configuração-do-banco-de-dados)
8. [Configuração da Aplicação](#8-configuração-da-aplicação)
9. [Executando o Projeto](#9-executando-o-projeto)
10. [Verificação e Testes](#10-verificação-e-testes)
11. [Solução de Problemas Comuns](#11-solução-de-problemas-comuns)

---

## 1. Pré-requisitos

Antes de começar, você precisará instalar os seguintes componentes no seu Windows 11:

- **.NET Core 3.1 SDK** - Framework de desenvolvimento
- **MySQL Server 5.7.9+** - Banco de dados
- **Git** - Controle de versão
- **Visual Studio 2019+** ou **VS Code** - IDE para desenvolvimento

---

## 2. Instalação do .NET Core 3.1 SDK

### Passo 2.1: Download

1. Acesse o site oficial da Microsoft: https://dotnet.microsoft.com/download/dotnet/3.1
2. Na seção **.NET Core 3.1 SDK**, clique em **Download .NET Core 3.1 SDK** para Windows
3. Escolha a versão **x64** (mais comum) ou **x86** dependendo do seu sistema

### Passo 2.2: Instalação

1. Execute o arquivo baixado (exemplo: `dotnet-sdk-3.1.xxx-win-x64.exe`)
2. Siga o assistente de instalação (Next → Next → Install)
3. Aguarde a conclusão da instalação

### Passo 2.3: Verificação

1. Abra o **Prompt de Comando** (cmd) ou **PowerShell**
2. Digite o comando:
   ```bash
   dotnet --version
   ```
3. Você deverá ver algo como: `3.1.xxx`

> ✅ **Sucesso!** O .NET Core 3.1 SDK está instalado.

---

## 3. Instalação do MySQL Server

### Passo 3.1: Download

1. Acesse: https://dev.mysql.com/downloads/installer/
2. Baixe o **MySQL Installer for Windows** (mysql-installer-community)
3. Escolha a versão **Full** para ter todas as ferramentas

### Passo 3.2: Instalação

1. Execute o instalador baixado (mysql-installer-community-xxx.msi)
2. Escolha o tipo de instalação: **Developer Default** (recomendado)
3. Clique em **Next** → **Execute** (para baixar os componentes)

### Passo 3.3: Configuração do MySQL Server

Durante a instalação, você chegará na configuração do servidor:

1. **Type and Networking**:
   - Config Type: **Development Computer**
   - Port: **3306** (não altere!)
   - Marque **Open Windows Firewall ports for network access**
   - Clique em **Next**

2. **Authentication Method**:
   - Escolha: **Use Strong Password Encryption** (recomendado)
   - Clique em **Next**

3. **Accounts and Roles**:
   - **Root Password**: Digite `admin` (ou outra senha de sua preferência)
   - **Confirme a senha**: `admin`
   - ⚠️ **IMPORTANTE**: Se usar senha diferente de `admin`, você precisará alterar a configuração do projeto depois
   - Clique em **Next**

4. **Windows Service**:
   - Marque **Configure MySQL Server as a Windows Service**
   - Service Name: **MySQL80** (ou padrão)
   - Marque **Start the MySQL Server at System Startup**
   - Clique em **Next**

5. **Apply Configuration**:
   - Clique em **Execute** para aplicar as configurações
   - Aguarde a conclusão
   - Clique em **Finish**

### Passo 3.4: Instalação do MySQL Workbench (opcional mas recomendado)

O instalador também irá instalar o **MySQL Workbench**, uma interface gráfica para gerenciar o banco de dados.

1. Continue o assistente para instalar o Workbench
2. Clique em **Next** → **Execute** → **Finish**

### Passo 3.5: Verificação

1. Abra o **Prompt de Comando**
2. Digite:
   ```bash
   mysql --version
   ```
3. Ou teste a conexão:
   ```bash
   mysql -u root -p
   ```
4. Digite a senha (`admin`) e pressione Enter
5. Se conectar com sucesso, digite `exit;` para sair

> ✅ **Sucesso!** O MySQL Server está instalado e rodando.

---

## 4. Instalação do Git

### Passo 4.1: Download

1. Acesse: https://git-scm.com/download/win
2. O download do instalador deve iniciar automaticamente

### Passo 4.2: Instalação

1. Execute o instalador (Git-x.xx.x-64-bit.exe)
2. Aceite a licença
3. Configurações recomendadas:
   - **Select Components**: deixe as opções padrão
   - **Default editor**: escolha seu editor preferido (Vim, Notepad++, VS Code, etc.)
   - **Adjusting your PATH**: escolha **Git from the command line and also from 3rd-party software**
   - **Line ending conversions**: escolha **Checkout Windows-style, commit Unix-style line endings**
   - Demais opções: deixe o padrão
4. Clique em **Install** e aguarde

### Passo 4.3: Verificação

1. Abra um **novo** Prompt de Comando ou PowerShell
2. Digite:
   ```bash
   git --version
   ```
3. Você deverá ver: `git version x.xx.x`

> ✅ **Sucesso!** O Git está instalado.

---

## 5. Instalação da IDE

Você pode escolher entre **Visual Studio** (mais completo) ou **VS Code** (mais leve).

### Opção A: Visual Studio 2019/2022 (Recomendado)

#### Passo 5A.1: Download

1. Acesse: https://visualstudio.microsoft.com/pt-br/downloads/
2. Baixe o **Visual Studio Community** (gratuito)

#### Passo 5A.2: Instalação

1. Execute o instalador (VisualStudioSetup.exe)
2. Na tela de workloads, selecione:
   - ✅ **ASP.NET and web development**
   - ✅ **.NET Core cross-platform development**
3. Clique em **Install**
4. Aguarde a instalação (pode demorar bastante)

### Opção B: Visual Studio Code (Alternativa leve)

#### Passo 5B.1: Download

1. Acesse: https://code.visualstudio.com/
2. Clique em **Download for Windows**

#### Passo 5B.2: Instalação

1. Execute o instalador (VSCodeSetup-xxx.exe)
2. Aceite a licença
3. **Importante**: Marque as opções:
   - ✅ Add "Open with Code" action to Windows Explorer file context menu
   - ✅ Add "Open with Code" action to Windows Explorer directory context menu
   - ✅ Add to PATH
4. Clique em **Install**

#### Passo 5B.3: Extensões Necessárias

1. Abra o VS Code
2. Clique no ícone de **Extensions** (Ctrl+Shift+X)
3. Instale as seguintes extensões:
   - **C# for Visual Studio Code** (ms-dotnettools.csharp)
   - **C# Dev Kit** (ms-dotnettools.csdevkit)
   - **NuGet Package Manager** (jmrog.vscode-nuget-package-manager)

> ✅ **Sucesso!** Sua IDE está pronta.

---

## 6. Clone do Projeto

### Passo 6.1: Escolher o Diretório

1. Crie uma pasta para seus projetos, por exemplo:
   ```
   C:\Projetos
   ```
2. Abra o **Prompt de Comando** ou **PowerShell**
3. Navegue até a pasta:
   ```bash
   cd C:\Projetos
   ```

### Passo 6.2: Clonar o Repositório

1. Execute o comando:
   ```bash
   git clone https://github.com/SESIDEV/BaseDeProjetos.git
   ```
2. Aguarde o download do projeto
3. Entre na pasta do projeto:
   ```bash
   cd BaseDeProjetos
   ```

> ✅ **Sucesso!** O projeto foi clonado para sua máquina.

---

## 7. Configuração do Banco de Dados

### Passo 7.1: Criar o Banco de Dados

Você tem duas opções:

#### Opção A: Usando MySQL Workbench (Interface Gráfica)

1. Abra o **MySQL Workbench**
2. Clique na conexão **Local instance MySQL80** (ou similar)
3. Digite a senha do root (`admin`)
4. Na aba **Query**, digite:
   ```sql
   CREATE DATABASE basedb CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
   ```
5. Clique no ícone de **raio** (Execute) ou pressione **Ctrl+Enter**
6. Você verá a mensagem de sucesso

#### Opção B: Usando Linha de Comando

1. Abra o Prompt de Comando
2. Conecte ao MySQL:
   ```bash
   mysql -u root -p
   ```
3. Digite a senha (`admin`)
4. Execute o comando SQL:
   ```sql
   CREATE DATABASE basedb CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
   ```
5. Verifique a criação:
   ```sql
   SHOW DATABASES;
   ```
6. Você deverá ver `basedb` na lista
7. Saia do MySQL:
   ```sql
   exit;
   ```

> ✅ **Sucesso!** O banco de dados `basedb` foi criado.

### Passo 7.2: Verificar Configuração de Conexão

⚠️ **Se você usou uma senha diferente de `admin` no MySQL**, precisa alterar o arquivo de configuração:

1. Abra o arquivo `BaseDeProjetos/appsettings.json`
2. Localize a seção `ConnectionStrings`:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "server=localhost;userid=root;password=admin;database=basedb;port=3306"
   }
   ```
3. Altere o `password=admin` para sua senha
4. Salve o arquivo

---

## 8. Configuração da Aplicação

### Passo 8.1: Restaurar Pacotes NuGet

1. Abra o **Prompt de Comando** ou **PowerShell**
2. Navegue até a pasta do projeto:
   ```bash
   cd C:\Projetos\BaseDeProjetos
   ```
3. Execute o comando:
   ```bash
   dotnet restore
   ```
4. Aguarde o download de todos os pacotes NuGet

### Passo 8.2: Build do Projeto

1. Execute o comando:
   ```bash
   dotnet build
   ```
2. Aguarde a compilação
3. Você deverá ver: `Build succeeded.`

> ⚠️ Se houver erros, veja a seção [Solução de Problemas](#11-solução-de-problemas-comuns)

---

## 9. Executando o Projeto

Agora vamos rodar a aplicação! As **migrations** do Entity Framework serão executadas automaticamente na primeira execução, criando todas as tabelas necessárias.

### Opção A: Executar via Linha de Comando

1. No Prompt de Comando, dentro da pasta do projeto:
   ```bash
   cd BaseDeProjetos
   ```
2. Execute:
   ```bash
   dotnet run
   ```
3. Aguarde até ver as mensagens:
   ```
   Now listening on: https://localhost:5001
   Now listening on: http://localhost:5000
   Application started. Press Ctrl+C to shut down.
   ```
4. Abra seu navegador e acesse:
   - **https://localhost:5001** (HTTPS - recomendado)
   - ou **http://localhost:5000** (HTTP)

### Opção B: Executar via Visual Studio

1. Abra o arquivo `BaseDeProjetos.sln` no Visual Studio
2. Aguarde o projeto carregar
3. Pressione **F5** ou clique em **IIS Express** (botão verde)
4. O navegador abrirá automaticamente

### Opção C: Executar via VS Code

1. Abra a pasta do projeto no VS Code:
   ```bash
   code .
   ```
2. Pressione **F5** para iniciar o debug
3. Escolha **.NET Core** quando solicitado
4. O projeto será executado

> ✅ **Sucesso!** O sistema está rodando!

### Passo 9.1: Primeira Execução - Migrations

Na primeira vez que você executar o projeto, você verá no console mensagens sobre migrations sendo aplicadas:

```
info: Microsoft.EntityFrameworkCore.Migrations[20402]
      Applying migration '20200101000000_InitialCreate'.
info: Microsoft.EntityFrameworkCore.Migrations[20402]
      Applying migration '20200102000000_AddProspeccao'.
...
```

Isso é normal! O Entity Framework está criando todas as tabelas no banco de dados automaticamente.

---

## 10. Verificação e Testes

### Passo 10.1: Acessar a Aplicação

1. No navegador, acesse: **https://localhost:5001**
2. Você deverá ver a página inicial do sistema
3. Se aparecer um aviso de certificado SSL não confiável:
   - Clique em **Avançado** → **Continuar mesmo assim** (desenvolvimento local é seguro)

### Passo 10.2: Verificar o Banco de Dados

1. Abra o **MySQL Workbench**
2. Conecte ao servidor local
3. Expanda o banco `basedb`
4. Você deverá ver várias tabelas criadas:
   - `Projeto`
   - `Prospeccao`
   - `Empresa`
   - `Pessoa`
   - `AspNetUsers`
   - E muitas outras...

### Passo 10.3: Executar Testes (Opcional)

Para verificar que tudo está funcionando corretamente:

1. Abra o Prompt de Comando na pasta raiz do projeto
2. Execute:
   ```bash
   dotnet test
   ```
3. Aguarde a execução dos testes
4. Você verá um relatório com os resultados

---

## 11. Solução de Problemas Comuns

### Problema 1: "The certificate chain was issued by an authority that is not trusted"

**Solução:**
1. Abra o PowerShell como **Administrador**
2. Execute:
   ```powershell
   dotnet dev-certs https --trust
   ```
3. Confirme quando solicitado
4. Reinicie a aplicação

---

### Problema 2: "Unable to connect to any of the specified MySQL hosts"

**Possíveis causas e soluções:**

1. **MySQL não está rodando:**
   - Abra **Serviços** do Windows (Win+R → `services.msc`)
   - Procure por **MySQL80** (ou similar)
   - Se estiver parado, clique com botão direito → **Iniciar**

2. **Porta incorreta:**
   - Verifique no MySQL Workbench em qual porta o servidor está (padrão é 3306)
   - Ajuste o `appsettings.json` se necessário

3. **Senha incorreta:**
   - Verifique se a senha no `appsettings.json` está correta

---

### Problema 3: "dotnet: command not found" ou não reconhecido

**Solução:**
1. Feche e abra novamente o Prompt de Comando
2. Se o problema persistir, adicione o .NET ao PATH manualmente:
   - Win+R → `sysdm.cpl` → **Variáveis de Ambiente**
   - Em **Path**, adicione: `C:\Program Files\dotnet\`
   - Clique em **OK** e reinicie o terminal

---

### Problema 4: Build falha com erro de pacotes NuGet

**Solução:**
1. Limpe o cache do NuGet:
   ```bash
   dotnet nuget locals all --clear
   ```
2. Restaure novamente:
   ```bash
   dotnet restore --force
   ```
3. Tente fazer o build novamente:
   ```bash
   dotnet build
   ```

---

### Problema 5: Página não carrega, erro 500

**Solução:**
1. Verifique os logs no console onde o `dotnet run` está executando
2. Verifique se o banco `basedb` foi criado corretamente
3. Tente executar as migrations manualmente:
   ```bash
   dotnet ef database update --project BaseDeProjetos/BaseDeProjetos.csproj
   ```

---

### Problema 6: "The SDK 'Microsoft.NET.Sdk.Web' specified could not be found"

**Solução:**
1. Verifique se o .NET Core 3.1 SDK está instalado:
   ```bash
   dotnet --list-sdks
   ```
2. Você deve ver `3.1.xxx` na lista
3. Se não aparecer, reinstale o .NET Core 3.1 SDK

---

## 📚 Próximos Passos

Agora que o sistema está rodando, você pode:

1. **Criar seu primeiro usuário**: Acesse a área de registro
2. **Explorar os módulos**: Projetos, Empresas, Pessoas, Funil de Vendas, etc.
3. **Importar dados**: Se você tiver dados existentes
4. **Configurar email**: Configure a API do SendGrid para envio de emails

---

## 🔧 Configurações Adicionais (Opcional)

### Configurar SendGrid para Emails

1. Crie uma conta no [SendGrid](https://sendgrid.com/)
2. Obtenha sua API Key
3. Configure o User Secret:
   ```bash
   cd BaseDeProjetos
   dotnet user-secrets set "SendGridKey" "SUA_API_KEY_AQUI"
   ```

### Hot Reload (Recarga Automática de Views)

O projeto já está configurado para hot reload! Qualquer alteração nas Views Razor será refletida automaticamente sem precisar reiniciar a aplicação.

---

## 📞 Suporte

Se você encontrar problemas não listados aqui:

1. Verifique o arquivo `Docs/Problemas e Soluções.pdf` no projeto
2. Consulte a documentação oficial: https://docs.microsoft.com/aspnet/core
3. Entre em contato com a equipe de desenvolvimento

---

## ✅ Checklist Final

Antes de começar a desenvolver, verifique se:

- [ ] .NET Core 3.1 SDK instalado (`dotnet --version`)
- [ ] MySQL Server rodando (Serviços do Windows)
- [ ] Git instalado (`git --version`)
- [ ] Projeto clonado
- [ ] Banco `basedb` criado
- [ ] Pacotes NuGet restaurados (`dotnet restore`)
- [ ] Projeto compila (`dotnet build`)
- [ ] Aplicação executa (`dotnet run`)
- [ ] Página inicial carrega no navegador (https://localhost:5001)
- [ ] Tabelas criadas no banco de dados

---

**Parabéns!** 🎉 Você configurou com sucesso o sistema **Base de Projetos do SGI** no Windows 11!

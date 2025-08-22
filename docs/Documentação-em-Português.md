# Documentação em Português - REDZAuth API

## 📋 Visão Geral

REDZAuth API é um sistema abrangente de autenticação de chaves de licença construído com ASP.NET Core 8.0 e MongoDB. Esta API fornece autenticação segura de usuários, gerenciamento de chaves de licença, vinculação HWID e funcionalidade de lista negra com integração de webhook do Discord para monitoramento.

## 🎯 Propósito

Esta API foi criada para uso pessoal e estudo.

Foi projetada para servir como autenticação/logs/registros para meus projetos C#. Em testes reais, não foi muito utilizada porque eu não tinha muita experiência ou tempo para dedicar a ela. No entanto, durante testes de interação usando POSTMAN, ela funcionou muito bem e foi funcional. Eu só precisava implementá-la em minhas produções (que eu não fiz, aliás, rs). Mas é uma boa API, bem estruturada e projetada para substituir o KeyAuth.cs e oferecer um nível mais alto de segurança, AES 256. Bem, agora é código aberto, aproveitem.

## 🚀 Recursos

### Autenticação Principal
- **Registro e Login de Usuários** com validação de chave de licença
- **Vinculação HWID (Hardware ID)** para controle de acesso específico do dispositivo
- **Rastreamento de Endereço IP** para monitoramento de segurança
- **Hash de Senha** usando SHA256 para armazenamento seguro

### Gerenciamento de Licenças
- **Geração Automática de Chaves de Licença** com formatos personalizáveis
- **Criação de Chaves de Licença Personalizadas** para casos de uso específicos
- **Validação de Licença** e rastreamento de uso
- **Múltiplos Tipos de Plano**: Mensal, Trimestral, Anual e Vitalício
- **Gerenciamento de Expiração de Licença**

### Segurança e Monitoramento
- **Sistema de Lista Negra** para banir usuários, IPs e HWIDs
- **Integração de Webhook do Discord** para monitoramento em tempo real
- **Detecção de Incompatibilidade HWID** para prevenir acesso não autorizado
- **Logs Abrangentes** de todos os eventos de autenticação

### Funções Administrativas
- **Reset HWID** para mudanças legítimas de dispositivo
- **Gerenciamento de Chaves de Licença** (gerar, listar, filtrar)
- **Operações de Ban/Unban** de usuários
- **Página de Status** para monitoramento de saúde da API

## 🛠️ Stack Tecnológica

- **.NET 8.0** - Versão LTS mais recente
- **ASP.NET Core Web API** - Framework web moderno
- **MongoDB** - Banco de dados NoSQL para armazenamento flexível
- **MongoDB.Driver** - Driver oficial do MongoDB para .NET
- **Swagger/OpenAPI** - Documentação e teste da API
- **Injeção de Dependência** - Container DI integrado do .NET

## 🔧 Frameworks e Bibliotecas Utilizadas

- **Microsoft.AspNetCore.Authentication** (v2.3.0) - Framework de autenticação
- **Microsoft.Extensions.Configuration.UserSecrets** (v8.0.0) - Gerenciamento de configuração
- **MongoDB.Driver** (v2.24.0) - Driver MongoDB para .NET
- **Swashbuckle.AspNetCore** (v6.5.0) - Documentação da API

## ⚠️ Aviso de Segurança

**IMPORTANTE**: Este código foi preparado para lançamento de código aberto, mas esteja ciente das seguintes considerações de segurança:

- **Sem Autenticação/Autorização**: Os endpoints administrativos atualmente não têm autenticação. Em produção, implemente autenticação adequada (JWT, chaves de API, etc.).
- **Sem Limitação de Taxa**: Nenhuma limitação de taxa está implementada. Considere adicionar limitação de taxa para uso em produção.
- **Sem CORS**: Políticas CORS não estão configuradas. Configure políticas CORS apropriadas para seu domínio.
- **HTTPS**: Certifique-se de que HTTPS está configurado adequadamente em produção.
- **Validação de Entrada**: Embora exista validação básica, considere implementar validação de entrada mais robusta.
- **Logging**: O logging atual usa Console.WriteLine. Implemente logging estruturado para produção.

**Sempre revise e teste as medidas de segurança antes de implantar em produção!**

## 📋 Pré-requisitos

Antes de executar esta aplicação, certifique-se de ter:

- **.NET 8.0 SDK** instalado em sua máquina
- **MongoDB** em execução (local ou na nuvem)
- **Visual Studio 2022** ou **VS Code** (recomendado)
- **Git** para controle de versão

## 🔧 Instalação e Configuração

### Opção 1: Visual Studio 2022

#### 1. Clonar o Repositório
```bash
git clone https://github.com/onlyredz/REDZAuthAPI-v1.0.git
cd REDZAuthAPI-v1.0
```

#### 2. Abrir no Visual Studio
- Abra o Visual Studio 2022
- Vá em `Arquivo` → `Abrir` → `Projeto/Solução`
- Navegue até a pasta clonada e selecione `REDZAuthApi.sln`
- Clique em `Abrir`

#### 3. Configurar Conexão do Banco de Dados
- No Gerenciador de Soluções, clique com o botão direito em `appsettings.json`
- Selecione `Abrir` ou clique duas vezes para abrir
- Atualize a string de conexão do MongoDB:

```json
{
  "ConnectionStrings": {
    "MongoDB": "mongodb://localhost:27017"
  }
}
```

Para MongoDB Atlas (nuvem), use:
```json
{
  "ConnectionStrings": {
    "MongoDB": "mongodb+srv://usuario:senha@cluster.mongodb.net/?retryWrites=true&w=majority"
  }
}
```

#### 4. Configurar Configurações de Segurança
No mesmo arquivo `appsettings.json`, atualize as configurações de segurança:

```json
{
  "SecuritySettings": {
    "AesEncryptionKey": "SUA_CHAVE_DE_32_CARACTERES_AQUI"
  }
}
```

**Importante**: Substitua `SUA_CHAVE_DE_32_CARACTERES_AQUI` por uma chave de criptografia segura de 32 caracteres.

#### 5. Configurar Webhook do Discord (Opcional)
Para habilitar notificações do Discord, adicione sua URL de webhook:

```json
{
  "WebhookSettings": {
    "DiscordWebhookUrl": "https://discord.com/api/webhooks/SUA_URL_DE_WEBHOOK"
  }
}
```

#### 6. Compilar e Executar
- Pressione `Ctrl + Shift + B` para compilar a solução
- Pressione `F5` para executar a aplicação em modo de depuração
- Ou pressione `Ctrl + F5` para executar sem depuração

A API estará disponível em `https://localhost:7001` (ou a porta mostrada no console).

### Opção 2: Visual Studio Code

#### 1. Clonar o Repositório
```bash
git clone https://github.com/onlyredz/REDZAuthAPI-v1.0.git
cd REDZAuthAPI-v1.0
```

#### 2. Abrir no VS Code
```bash
code .
```

#### 3. Instalar Extensões Necessárias
Instale as seguintes extensões do VS Code:
- **C#** (da Microsoft)
- **C# Dev Kit** (da Microsoft)
- **REST Client** (opcional, para testar endpoints da API)

#### 4. Configurar Conexão do Banco de Dados
- Abra `appsettings.json` no VS Code
- Atualize a string de conexão do MongoDB conforme mostrado na seção do Visual Studio acima

#### 5. Configurar Configurações de Segurança
Atualize as configurações de segurança em `appsettings.json` conforme mostrado na seção do Visual Studio acima.

#### 6. Configurar Webhook do Discord (Opcional)
Adicione sua URL de webhook do Discord conforme mostrado na seção do Visual Studio acima.

#### 7. Compilar e Executar
Abra o terminal integrado no VS Code (`Ctrl + `` `) e execute:

```bash
dotnet restore
dotnet build
dotnet run
```

A API estará disponível em `https://localhost:7001` (ou a porta mostrada no console).

### Opção 3: Linha de Comando

#### 1. Clonar e Configurar
```bash
git clone https://github.com/onlyredz/REDZAuthAPI-v1.0.git
cd REDZAuthAPI-v1.0
```

#### 2. Configurar Configurações
- Copie `appsettings.Example.json` para `appsettings.json`
- Edite `appsettings.json` com sua configuração conforme mostrado acima

#### 3. Executar a Aplicação
```bash
dotnet restore
dotnet build
dotnet run
```

## 📚 Documentação da API

Uma vez que a aplicação esteja em execução, você pode acessar a documentação interativa da API em:
`https://localhost:7001/swagger`

## 🔌 Endpoints da API

### Endpoints de Autenticação

#### Registrar Usuário
```http
POST /api/auth/register
Content-Type: application/json

{
  "username": "usuario_teste",
  "password": "senha123",
  "key": "MENSAL-ABCD-1234",
  "hwid": "hwid-do-dispositivo",
  "ip": "192.168.1.1"
}
```

#### Login do Usuário
```http
POST /api/auth/login
Content-Type: application/json

{
  "username": "usuario_teste",
  "password": "senha123",
  "hwid": "hwid-do-dispositivo",
  "ip": "192.168.1.1"
}
```

### Endpoints de Gerenciamento de Licenças

#### Gerar Chave de Licença
```http
POST /api/license/generate?plan=mensal
```

#### Criar Chave de Licença Personalizada
```http
POST /api/license/custom?customKey=CHAVE-PERSONALIZADA-123&plan=mensal
```

#### Gerar Chave de Licença (Alternativo)
```http
POST /api/key/generate?plan=mensal
```

#### Criar Chave de Licença Personalizada (Alternativo)
```http
POST /api/key/custom?plan=mensal&customKey=CHAVE-PERSONALIZADA-123
```

#### Listar Chaves de Licença
```http
GET /api/key/list?plan=mensal&used=false&fromDate=2024-01-01&toDate=2024-12-31
```

### Endpoints Administrativos

#### Reset HWID
```http
POST /api/admin/reset-hwid?username=usuario_teste
```

#### Reset HWID (Alternativo)
```http
POST /api/hwid/reset
Content-Type: application/json

{
  "username": "usuario_teste"
}
```

### Gerenciamento de Lista Negra

#### Banir Usuário/IP/HWID
```http
POST /api/ban
Content-Type: application/json

{
  "username": "usuario_teste",
  "ip": "192.168.1.1",
  "hwid": "hwid-do-dispositivo",
  "banUser": true,
  "banIP": false,
  "banHWID": false,
  "reason": "Violação dos termos de serviço"
}
```

#### Desbanir Usuário/IP/HWID
```http
POST /api/unban
Content-Type: application/json

{
  "username": "usuario_teste",
  "ip": "192.168.1.1",
  "hwid": "hwid-do-dispositivo",
  "unbanUser": true,
  "unbanIP": false,
  "unbanHWID": false,
  "reason": "Recurso aprovado"
}
```

### Página de Status
```http
GET /apistatus
```

## 📊 Coleções do Banco de Dados

A aplicação usa as seguintes coleções do MongoDB:

- **Users** - Contas de usuário e dados de autenticação
- **Licenses** - Chaves de licença e seu status de uso
- **Blacklist** - Usuários, IPs e HWIDs banidos

## 🔒 Recursos de Segurança

### Segurança de Senha
- Senhas são hasheadas usando SHA256 antes do armazenamento
- Nenhuma senha em texto simples é armazenada no banco de dados

### Vinculação HWID
- IDs de hardware são vinculados a contas de usuário no primeiro login
- Logins subsequentes devem usar o mesmo HWID
- Admin pode resetar HWID para mudanças legítimas de dispositivo

### Sistema de Lista Negra
- Usuários, endereços IP e HWIDs podem ser banidos
- Rastreamento abrangente de motivos de ban
- Funcionalidade flexível de desban

### Monitoramento de Webhook
- Notificações do Discord em tempo real para eventos de segurança
- URLs de webhook configuráveis
- Tratamento gracioso de erros para falhas de webhook

## 🚨 Tratamento de Erros

A API fornece tratamento abrangente de erros:

- **400 Bad Request** - Dados de entrada inválidos
- **401 Unauthorized** - Falhas de autenticação
- **500 Internal Server Error** - Erros do lado do servidor

Todos os erros incluem mensagens descritivas para ajudar na depuração.

## 🔧 Opções de Configuração

### Planos Disponíveis
- `mensal` - 30 dias
- `trimestral` - 91 dias
- `anual` - 366 dias
- `vitalicio` - 9999 dias (efetivamente ilimitado)

### Formato da Chave de Licença
Chaves geradas automaticamente seguem o padrão: `{PLANO}-{ALEATORIO}-{ALEATORIO}`

Exemplos:
- `MENSAL-ABCD-1234`
- `TRIMES-EFGH-5678`
- `ANUAL-IJKL-9012`
- `VITAL-MNOP-3456`

## 🐛 Solução de Problemas

### Problemas Comuns

1. **Falha na Conexão MongoDB**
   - Verifique sua string de conexão em `appsettings.json`
   - Certifique-se de que o MongoDB está em execução e acessível
   - Verifique conectividade de rede para instâncias na nuvem

2. **Erro de Chave de Criptografia**
   - Certifique-se de que sua chave AES tem exatamente 32 caracteres
   - Atualize a chave em `appsettings.json`

3. **Webhook Não Funcionando**
   - Verifique se sua URL de webhook do Discord está correta
   - Verifique permissões do servidor Discord
   - Revise logs do console para erros de webhook

4. **Incompatibilidade HWID**
   - Este é comportamento esperado para segurança
   - Use endpoint administrativo para resetar HWID se legítimo

5. **Erros de Compilação**
   - Certifique-se de que o .NET 8.0 SDK está instalado
   - Execute `dotnet restore` para restaurar pacotes
   - Verifique dependências ausentes

### Problemas Específicos do Visual Studio

1. **Solução Não Carrega**
   - Certifique-se de que o Visual Studio 2022 está atualizado
   - Instale a carga de trabalho .NET 8.0 se solicitado
   - Tente limpar e recompilar a solução

2. **Problemas de Depuração**
   - Verifique se o projeto de inicialização está definido corretamente
   - Verifique configurações de inicialização em `Properties/launchSettings.json`

### Problemas Específicos do VS Code

1. **IntelliSense Não Funciona**
   - Instale a extensão C#
   - Recarregue o VS Code após a instalação
   - Execute `dotnet restore` no terminal

2. **Depuração Não Funciona**
   - Instale a extensão C# Dev Kit
   - Crie uma configuração de inicialização se necessário

## 🤝 Contribuindo

1. Faça um fork do repositório
2. Crie um branch de feature (`git checkout -b feature/feature-incrivel`)
3. Commit suas mudanças (`git commit -m 'Adicionar feature incrível'`)
4. Push para o branch (`git push origin feature/feature-incrivel`)
5. Abra um Pull Request

## 📄 Licença

Este projeto está licenciado sob a Licença MIT - veja o arquivo [LICENSE](LICENSE) para detalhes.

## 👨‍💻 Créditos

**Desenvolvedor Original**: [onlyredz](https://github.com/onlyredz)

**Auditoria de Segurança e Preparação para Código Aberto**: Este código foi preparado para lançamento de código aberto com melhorias de segurança e documentação abrangente.

**Por favor, mantenha os créditos originais ao usar ou modificar este código.**

## ⚠️ Aviso de Segurança

- **Nunca comite informações sensíveis** como strings de conexão ou chaves de criptografia
- **Use variáveis de ambiente** para implantações em produção
- **Atualize dependências regularmente** para corrigir vulnerabilidades de segurança
- **Monitore logs** para atividade suspeita
- **Implemente limitação de taxa** em ambientes de produção
- **Adicione autenticação adequada** para endpoints administrativos
- **Configure políticas CORS** apropriadamente
- **Use HTTPS** em produção

## 📞 Suporte

Para suporte e perguntas:
- Crie uma issue no repositório GitHub
- Verifique a documentação da API em `/swagger`
- Revise os logs do console para informações detalhadas de erro

## 🔄 Histórico de Versões

- **v1.0.0** - Lançamento inicial com autenticação principal e gerenciamento de licenças
- **v1.1.0** - Adicionado sistema de lista negra e webhooks do Discord
- **v1.2.0** - Melhorado tratamento de erros e gerenciamento de configuração
- **v1.3.0** - Adicionada documentação Swagger e melhorias na injeção de dependência
- **v1.4.0** - Auditoria de segurança e preparação para código aberto

---

**Nota**: Esta API é projetada para uso educacional e de negócios legítimos. Certifique-se de cumprir as leis e regulamentos aplicáveis em sua jurisdição. Sempre revise e teste as medidas de segurança antes de implantar em produção.

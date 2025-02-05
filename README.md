# processtecTest

## Descrição

Este projeto é uma aplicação .NET que se conecta a um banco de dados PostgreSQL para verificar e exibir informações sobre as tabelas existentes. Ele utiliza as bibliotecas [DotNetEnv](https://www.nuget.org/packages/DotNetEnv), [Npgsql](https://www.nuget.org/packages/Npgsql), [Serilog](https://www.nuget.org/packages/Serilog) e [Supabase](https://www.nuget.org/packages/supabase).

## Pré-requisitos

- [.NET SDK 9.0](https://dotnet.microsoft.com/en-us/download/dotnet/9.0)
- Um banco de dados PostgreSQL configurado e acessível.

## Configuração

1.  Clone o repositório:

    ```sh
    git clone <https://github.com/victorbrandaao/processtecTest.git>
    cd processtecTest
    ```

2.  Crie um arquivo [.env](http://_vscodecontentref_/0) na raiz do projeto e configure a string de conexão com o banco de dados:

    ```
    DATABASE_URL="Host=<seu_host>;Database=<seu_banco>;Username=<seu_usuario>;Password=<sua_senha>"
    ```

    Substitua `<seu_host>`, `<seu_banco>`, `<seu_usuario>` e `<sua_senha>` pelas informações do seu banco de dados.

## Como executar

1.  Restaure as dependências do projeto:

    ```sh
    dotnet restore
    ```

2.  Execute o projeto:

    ```sh
    dotnet run
    ```

## Dependências

*   [DotNetEnv](https://www.nuget.org/packages/DotNetEnv) - Para carregar variáveis de ambiente do arquivo [.env](http://_vscodecontentref_/1).
*   [Npgsql](https://www.nuget.org/packages/Npgsql) - Provider .NET para PostgreSQL.
*   [Serilog](https://www.nuget.org/packages/Serilog) - Para logging.
*   [Supabase](https://www.nuget.org/packages/supabase) - Client para Supabase.

## Estrutura do projeto

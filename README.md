# Apresentação 
Produtos API - API desenvolvida para processo seletivo na [AutoGlass](https://www.autoglass.com.br)

# Configuração do ambiente

## Requisitos
1. [.NET 6](https://dotnet.microsoft.com/download)
2. [Docker](https://www.docker.com)

## Preparando o ambiente
1.  Para preparar o ambiente é necessário inciar o docker, e subir a docker compose (é necessário estar na pasta root do projeto)
  ``` base
  docker-compose up -d
  ```
 - A docker compose irá subir o SQLServer.

## Comandos Migration
1.  Adicionar uma Migration
    ``` bash
    Add-Migration -Name <NomeDaMigration> -StartupProject Produtos.WebApi -Project Produtos.Infra.SqlServer -Context ApplicationDbContext
    ```
2.  Remover uma Migration (A ultima)
    ``` bash
    Remove-Migration -StartupProject Produtos.WebApi -Project Produtos.Infra.SqlServer -Context ApplicationDbContext
    ```
3.  Atualizar a base
    ``` bash
    Update-Database -StartupProject Produtos.WebApi -Project Produtos.Infra.SqlServer -Context ApplicationDbContext
    ```

## Executando a aplicação
- Para executar a aplicação é necessário atualizar a base rodando o comando de migration e depois usar o IIS Express ou o docker para inicar
- A aplicação ira abrir em uma porta, para ir até a documentação da API basta digitar `/swagger` depois da rota, por exemplo: `localhost:XXXXX/swagger`

## Documentação Postman
[Documentação e collection do postman](https://documenter.getpostman.com/view/7360830/2s8Z6x3ZBF)

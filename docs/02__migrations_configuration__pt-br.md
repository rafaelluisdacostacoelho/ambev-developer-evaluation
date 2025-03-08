## 1. Crie a migração inicial

Caso esteja rodando o projeto pela primeira vez e não tenha a migração inicial criada, rode o comando a seguir:

```sh
dotnet ef migrations add InitialMigration --project src/Ambev.DeveloperEvaluation.ORM --startup-project src/Ambev.DeveloperEvaluation.WebApi --context StoreDbContext --verbose
```

## 2. Crie os volumes manualmente:

```bash
docker volume create ambev_pg_data
docker volume create ambev_mongo_data
```

## 3. Confirme que os volumes foram criados:

```bash
docker volume ls
```
Você deve ver algo assim na saída:

```bash
DRIVER    VOLUME NAME
local     ambev_pg_data
local     ambev_mongo_data
```

## 4. Verifique a string de conexão do EF Core

Antes de rodar o comando é preciso ajustar o acesso ao container de fora dele.

No caso mude para localhost e para a porta externa do pg que está no docker, assim:

```json
"ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=54322;Database=developer_evaluation;Username=developer;Password=ev@luAt10n"
},
```

## 5. E finalmente atualize o DB

E para aplicar as migrações use o comando a seguir:

```sh
dotnet ef database update --project src/Ambev.DeveloperEvaluation.ORM --startup-project src/Ambev.DeveloperEvaluation.WebApi  --context StoreDbContext --verbose
```

>[!IMPORTANT]
>
> Lembre-se que estes comandos só funcionarão se o shell estiver apontando para a pasta raiz deste projeto (raiz do repositório no caso).

>[!IMPORTANT]
>
> Os comandos devem ser executados com o projeto parado senão causarão falha.





--------

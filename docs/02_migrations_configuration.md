Caso esteja rodando o projeto pela primeira vez e não tenha a migração inicial criada, rode o comando a seguir:

```sh
dotnet ef migrations add InitialMigration --project src/Ambev.DeveloperEvaluation.ORM --startup-project src/Ambev.DeveloperEvaluation.WebApi --context StoreDbContext --verbose
```

E para aplicar a migração inicial use o comando a seguir:

```sh
dotnet ef database update --project src/Ambev.DeveloperEvaluation.ORM --startup-project src/Ambev.DeveloperEvaluation.WebApi  --context StoreDbContext --verbose
```

>[!NOTE]
>
> Lembre-se que estes comandos só funcionarão se o shell estiver apontando para a pasta raiz deste projeto.

>[!NOTE]
>
> Os comandos devem ser executados com o projeto parado senão causarão falha.

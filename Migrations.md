
dotnet ef migrations add InitialMigration --no-build --project src/Ambev.DeveloperEvaluation.ORM --startup-project src/Ambev.DeveloperEvaluation.WebApi --context StoreDbContext --verbose

dotnet ef database update --no-build --project src/Ambev.DeveloperEvaluation.ORM --startup-project src/Ambev.DeveloperEvaluation.WebApi

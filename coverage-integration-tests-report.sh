# Bash script for Unix-based systems
#!/bin/bash

# Install tools if not present
dotnet tool install --global coverlet.console
dotnet tool install --global dotnet-reportgenerator-globaltool

# Clean and build solution
dotnet restore Ambev.DeveloperEvaluation.Integration.sln
dotnet build Ambev.DeveloperEvaluation.Integration.sln --configuration Release --no-restore

# Run integration tests with coverage
dotnet test Ambev.DeveloperEvaluation.IntegrationTests --no-restore --verbosity normal \
/p:CollectCoverage=true \
/p:CoverletOutputFormat=cobertura \
/p:CoverletOutput=./TestResults/IntegrationTests/coverage.cobertura.xml \
/p:Exclude="[*]*.Program%2c[*]*.Startup%2c[*]*.Migrations.*"

# Generate coverage report
reportgenerator \
-reports:"./TestResults/IntegrationTests/coverage.cobertura.xml" \
-targetdir:"./TestResults/IntegrationTests/CoverageReport" \
-reporttypes:Html

# Removing temporary files
rm -rf bin obj

echo "Coverage report generated at TestResults/IntegrationTests/CoverageReport/index.html"

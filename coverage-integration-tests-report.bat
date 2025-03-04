@ECHO OFF

REM Install tools if not present
dotnet tool install --global coverlet.console
dotnet tool install --global dotnet-reportgenerator-globaltool

REM Clean and build solution
REM Use Debug configuration to ensure compatibility with coverage

dotnet restore Ambev.DeveloperEvaluation.sln

dotnet build Ambev.DeveloperEvaluation.sln --configuration Debug --no-restore

REM Ensure output directory exists
mkdir "./TestResults/IntegrationTests"

REM Run integration tests with coverage

dotnet test ./tests/Ambev.DeveloperEvaluation.Integration/Ambev.DeveloperEvaluation.Integration.csproj --no-restore --verbosity detailed ^
--collect:"XPlat Code Coverage" ^
/p:CoverletOutputFormat=cobertura ^
/p:CoverletOutput=./TestResults/IntegrationTests/coverage.cobertura.xml ^
/p:CoverletDebugOutput=true ^
/p:Configuration=Debug ^
/p:Exclude="[*]*.Program%2c[*]*.Startup%2c[*]*.Migrations.*"

REM Search for the generated coverage file
FOR /R "tests\Ambev.DeveloperEvaluation.Integration\TestResults" %%F IN (coverage.cobertura.xml) DO (
    echo "Found coverage file: %%F"
    COPY "%%F" "./TestResults/IntegrationTests/coverage.cobertura.xml"
)

REM Check if the coverage file was generated
IF EXIST "./TestResults/IntegrationTests/coverage.cobertura.xml" (
    echo "Coverage file generated successfully."
) ELSE (
    echo "ERROR: Coverage file not generated. Check test execution."
    dir /s .\TestResults
    pause
    EXIT /B 1
)

REM Generate coverage report
reportgenerator ^
-reports:"./TestResults/IntegrationTests/coverage.cobertura.xml" ^
-targetdir:"./TestResults/IntegrationTests/CoverageReport" ^
-reporttypes:Html

REM Removing temporary files
rmdir /s /q bin 2>nul
rmdir /s /q obj 2>nul

echo.
echo Coverage report generated at TestResults/IntegrationTests/CoverageReport/index.html
pause
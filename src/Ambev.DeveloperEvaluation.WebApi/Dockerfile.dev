# Define a imagem base para execução
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Define a imagem de build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copia os arquivos de projeto e restaura as dependências
COPY ["src/Ambev.DeveloperEvaluation.WebApi/Ambev.DeveloperEvaluation.WebApi.csproj", "src/Ambev.DeveloperEvaluation.WebApi/"]
COPY ["src/Ambev.DeveloperEvaluation.IoC/Ambev.DeveloperEvaluation.IoC.csproj", "src/Ambev.DeveloperEvaluation.IoC/"]
COPY ["src/Ambev.DeveloperEvaluation.Domain/Ambev.DeveloperEvaluation.Domain.csproj", "src/Ambev.DeveloperEvaluation.Domain/"]
COPY ["src/Ambev.DeveloperEvaluation.Common/Ambev.DeveloperEvaluation.Common.csproj", "src/Ambev.DeveloperEvaluation.Common/"]
COPY ["src/Ambev.DeveloperEvaluation.Application/Ambev.DeveloperEvaluation.Application.csproj", "src/Ambev.DeveloperEvaluation.Application/"]
COPY ["src/Ambev.DeveloperEvaluation.ORM/Ambev.DeveloperEvaluation.ORM.csproj", "src/Ambev.DeveloperEvaluation.ORM/"]

RUN dotnet restore "src/Ambev.DeveloperEvaluation.WebApi/Ambev.DeveloperEvaluation.WebApi.csproj"

# Copia todo o código fonte
COPY . .

# Configura o locale para UTF-8
RUN apt-get update \
    && apt-get install -y locales \
    && locale-gen en_US.UTF-8

ENV LANG en_US.UTF-8
ENV LANGUAGE en_US:en
ENV LC_ALL en_US.UTF-8

WORKDIR "/src/src/Ambev.DeveloperEvaluation.WebApi"

# Constrói o projeto
RUN dotnet build "Ambev.DeveloperEvaluation.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publica o projeto
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Ambev.DeveloperEvaluation.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Configura o container final
FROM base AS final
WORKDIR /app

# Copia os arquivos publicados para o container
COPY --from=publish /app/publish .

# Define o comando de entrada com suporte ao Hot Reload
ENTRYPOINT ["dotnet", "watch", "run", "--urls", "http://0.0.0.0:8080"]

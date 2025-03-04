﻿services:
  ambev.developerevaluation.webapi:
    container_name: ambev_developer_evaluation_webapi
    image: ${DOCKER_REGISTRY-}ambevdeveloperevaluationwebapi
    build:
      context: .
      dockerfile: src/Ambev.DeveloperEvaluation.WebApi/Dockerfile
    environment:
      - ASPNETCORE_ENVIRONMENT=Development
      - ASPNETCORE_HTTP_PORTS=8080
      - ASPNETCORE_HTTPS_PORTS=8081
    ports:
      - "61094:8080"
      - "61095:8081"
    volumes:
      - ${APPDATA}/Microsoft/UserSecrets:/home/app/.microsoft/usersecrets:ro
      - ${APPDATA}/ASP.NET/Https:/home/app/.aspnet/https:ro

  ambev.developerevaluation.database:
    container_name: ambev_developer_evaluation_database
    image: postgis/postgis:13-3.3
    environment:
      POSTGRES_DB: developer_evaluation
      POSTGRES_USER: developer
      POSTGRES_PASSWORD: ev@luAt10n
    ports:
      - "54322:5432"
    restart: unless-stopped
    volumes:
      - ambev_pg_data:/var/lib/postgresql/data

  ambev.developerevaluation.nosql:
    container_name: ambev_developer_evaluation_nosql 
    image: mongo:8.0
    environment:
        MONGO_INITDB_ROOT_USERNAME: developer
        MONGO_INITDB_ROOT_PASSWORD: ev@luAt10n
    ports:
      - "27017:27017"
    volumes:
      - ambev_mongo_data:/data/db

  ambev.developerevaluation.cache:
    container_name: ambev_developer_evaluation_cache 
    image: redis:7.4.1-alpine
    command: redis-server --requirepass ev@luAt10n
    ports:
       - "6379:6379"

volumes:
  ambev_pg_data:
    external: true
  ambev_mongo_data:
    external: true

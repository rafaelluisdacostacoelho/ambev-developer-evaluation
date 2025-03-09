# Docker Compose - Ambev Developer Evaluation

## Visão Geral
Este documento fornece uma visão detalhada do arquivo `docker-compose.yml` usado no projeto Ambev Developer Evaluation. Ele descreve cada serviço configurado, suas portas, variáveis de ambiente, volumes e dependências.

---

## Serviços Configurados

### 1. **ambev.developerevaluation.webapi**
- **Nome do Container:** `ambev_developer_evaluation_webapi`
- **Imagem:** `${DOCKER_REGISTRY-}ambevdeveloperevaluationwebapi`
- **Build:**
  - Contexto: Diretório raiz (`.`)
  - Dockerfile: `src/Ambev.DeveloperEvaluation.WebApi/Dockerfile.dev`
- **Variáveis de Ambiente:**
  - `ASPNETCORE_ENVIRONMENT=Development`
  - `ASPNETCORE_HTTP_PORTS=8080`
  - `ASPNETCORE_HTTPS_PORTS=8081`
  - `DOTNET_USE_POLLING_FILE_WATCHER=true`
  - `DOTNET_CLI_UI_LANGUAGE=en`
  - `LC_ALL=en_US.UTF-8`
  - `LANG=en_US.UTF-8`
  - `ELASTICSEARCH_URL=http://elasticsearch:9200`
- **Portas Expostas:**
  - HTTP: `61094:8080`
  - HTTPS: `61095:8081`
- **Volumes:**
  - Secrets: `${APPDATA}/Microsoft/UserSecrets:/home/app/.microsoft/usersecrets:ro`
  - Certificados HTTPS: `${APPDATA}/ASP.NET/Https:/home/app/.aspnet/https:ro`
  - Código Fonte: `.:/app`
- **Dependências:**
  - `elasticsearch`
  - `logstash`
  - `kibana`

### 2. **ambev.developerevaluation.database**
- **Nome do Container:** `ambev_developer_evaluation_database`
- **Imagem:** `postgis/postgis:13-3.3`
- **Variáveis de Ambiente:**
  - `POSTGRES_DB=developer_evaluation`
  - `POSTGRES_USER=developer`
  - `POSTGRES_PASSWORD=ev@luAt10n`
- **Portas:**
  - `54322:5432`
- **Volumes:**
  - `ambev_pg_data:/var/lib/postgresql/data`
- **Política de Reinício:** `unless-stopped`

### 3. **ambev.developerevaluation.nosql**
- **Nome do Container:** `ambev_developer_evaluation_nosql`
- **Imagem:** `mongo:8.0`
- **Variáveis de Ambiente:**
  - `MONGO_INITDB_ROOT_USERNAME=developer`
  - `MONGO_INITDB_ROOT_PASSWORD=ev@luAt10n`
- **Portas:**
  - `27017:27017`
- **Volumes:**
  - `ambev_mongo_data:/data/db`

### 4. **ambev.developerevaluation.cache**
- **Nome do Container:** `ambev_developer_evaluation_cache`
- **Imagem:** `redis:7.4.1-alpine`
- **Comando:** `redis-server --requirepass ev@luAt10n`
- **Portas:**
  - `6379:6379`

### 5. **elasticsearch**
- **Imagem:** `docker.elastic.co/elasticsearch/elasticsearch:8.10.1`
- **Nome do Container:** `elasticsearch`
- **Configuração:**
  - `discovery.type=single-node`
  - `ES_JAVA_OPTS=-Xms512m -Xmx512m`
  - `xpack.security.enabled=false`
- **Portas:**
  - `9200:9200`
- **Volumes:**
  - `es_data:/usr/share/elasticsearch/data`

### 6. **logstash**
- **Imagem:** `docker.elastic.co/logstash/logstash:8.10.1`
- **Nome do Container:** `logstash`
- **Portas:**
  - `5044:5044`
  - `9600:9600`
- **Volumes:**
  - `./logstash:/usr/share/logstash/pipeline`
- **Dependências:**
  - `elasticsearch`
- **Healthcheck:**
  - Teste: `curl -f http://elasticsearch:9200`
  - Intervalo: `30s`
  - Timeout: `10s`
  - Retries: `5`

### 7. **kibana**
- **Imagem:** `docker.elastic.co/kibana/kibana:8.10.1`
- **Nome do Container:** `kibana`
- **Variáveis de Ambiente:**
  - `ELASTICSEARCH_HOSTS=http://elasticsearch:9200`
- **Portas:**
  - `5601:5601`
- **Dependências:**
  - `elasticsearch`
- **Healthcheck:**
  - Teste: `curl -f http://elasticsearch:9200`
  - Intervalo: `30s`
  - Timeout: `10s`
  - Retries: `5`

---

## Volumes
- `ambev_pg_data`: volume externo para dados do PostgreSQL.
- `ambev_mongo_data`: volume externo para dados do MongoDB.
- `es_data`: volume interno para dados do Elasticsearch.

## Comandos Úteis
- **Subir os serviços:**
```bash
docker-compose up -d
```

- **Derrubar os serviços:**
```bash
docker-compose down
```

- **Verificar os logs:**
```bash
docker-compose logs -f
```

- **Acessar o container do banco de dados:**
```bash
docker exec -it ambev_developer_evaluation_database psql -U developer -d developer_evaluation
```

---

## Notas
- Certifique-se de que os volumes externos estejam previamente criados com `docker volume create [nome_do_volume]`.
- O serviço WebAPI depende do Elasticsearch, Logstash e Kibana para iniciar corretamente.


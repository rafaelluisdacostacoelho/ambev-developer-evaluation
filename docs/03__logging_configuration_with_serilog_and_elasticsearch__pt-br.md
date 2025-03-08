# Configuração de Logging com Serilog e Elasticsearch

## Visão Geral

Esta documentação descreve a configuração de logging utilizando o Serilog integrado ao Elasticsearch em uma aplicação .NET 8. Inclui também as configurações de contêineres Docker para uma stack completa de observabilidade com Logstash e Kibana.

## 📂 Estrutura dos Arquivos

Program.cs: Configuração do Serilog no código da aplicação.

docker-compose.yml: Configuração dos serviços Docker, incluindo Elasticsearch, Logstash e Kibana.

appsettings.json: Configuração detalhada do Serilog, incluindo o envio de logs para o Elasticsearch.

## 🔧 Configuração do Serilog

No arquivo Program.cs, o Serilog é configurado desde o início da aplicação, garantindo que todos os logs, mesmo durante a inicialização, sejam capturados:

```cpp
builder.Host.UseSerilog((context, services, configuration) =>
{
    configuration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext()
        .Enrich.WithMachineName()
        .WriteTo.Console();
});
```

Essa configuração lê as informações do appsettings.json, adiciona informações de contexto (como o nome da máquina) e exibe os logs no console.

## 📑 Configuração do appsettings.json

A configuração do Serilog no appsettings.json permite o envio dos logs para o Elasticsearch:

```json
"Serilog": {
  "Using": [ "Serilog.Sinks.Console", "Serilog.Sinks.Elasticsearch" ],
  "MinimumLevel": {
    "Default": "Information",
    "Override": {
      "Microsoft": "Warning",
      "System": "Warning"
    }
  },
  "WriteTo": [
    {
      "Name": "Console",
      "Args": {
        "outputTemplate": "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}"
      }
    },
    {
      "Name": "Elasticsearch",
      "Args": {
        "nodeUris": "http://elasticsearch:9200",
        "indexFormat": "ambev-logs-{0:yyyy.MM.dd}",
        "autoRegisterTemplate": true,
        "autoRegisterTemplateVersion": "ESv8",
        "failureCallback": "Console.WriteLine",
        "emitEventFailure": "WriteToSelfLog",
        "numberOfShards": 1,
        "numberOfReplicas": 0
      }
    }
  ],
  "Enrich": [ "FromLogContext", "WithMachineName", "WithThreadId" ],
  "Properties": {
    "Application": "AmbevDeveloperEvaluation"
  }
}
```

## 💡 Dicas

Para personalizar o índice do Elasticsearch, ajuste o parâmetro indexFormat.

O failureCallback ajuda a identificar problemas ao enviar logs para o Elasticsearch.

## 🚀 Configuração do Docker

Abaixo está o trecho do docker-compose.yml relevante para o Elasticsearch, Logstash e Kibana:

```yml
services:
  elasticsearch:
    image: docker.elastic.co/elasticsearch/elasticsearch:8.10.1
    container_name: elasticsearch
    environment:
      - discovery.type=single-node
      - "ES_JAVA_OPTS=-Xms512m -Xmx512m"
      - xpack.security.enabled=false
    ports:
      - "9200:9200"

  logstash:
    image: docker.elastic.co/logstash/logstash:8.10.1
    container_name: logstash
    ports:
      - "5044:5044"
      - "9600:9600"
    volumes:
      - ./logstash:/usr/share/logstash/pipeline

  kibana:
    image: docker.elastic.co/kibana/kibana:8.10.1
    container_name: kibana
    environment:
      - ELASTICSEARCH_HOSTS=http://elasticsearch:9200
    ports:
      - "5601:5601"
```

## 🔍 Acessando os Logs no Kibana

Abra o navegador e vá para: http://localhost:5601

No Kibana, crie um padrão de índice ambev-logs-* para visualizar os logs.

Utilize a ferramenta Discover para explorar os eventos em tempo real.

🛠️ Testando os Logs

Você pode gerar logs diretamente na aplicação usando o Serilog:

```cpp
Log.Information("Aplicação iniciada com sucesso!");
Log.Error("Erro simulado para teste!");
```

Os logs devem aparecer no console e também no Elasticsearch, acessível via Kibana.

## ✅ Conclusão

Esta configuração fornece uma solução robusta para monitoramento de logs em aplicações .NET 8. Com o Elasticsearch e o Kibana, você terá uma visão detalhada dos eventos da aplicação, facilitando a depuração e o monitoramento em produção.

## 🆘 Suporte

Em caso de dúvidas ou problemas, consulte a documentação oficial do Serilog, Elasticsearch, Logstash e Kibana ou entre em contato com o suporte técnico.


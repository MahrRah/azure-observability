# K8s Observability Sample

## Set Up

1. Build images

```sh
docker build --pull --rm -f "Poc.Generator/Dockerfile" -t optlsample.azurecr.io/poc/poc-generator:1.0 .
docker build --pull --rm -f "Poc.Processor/Dockerfile" -t optlsample.azurecr.io/poc/poc-processor:1.0 .
```

2. Push Image to registry

```sh
az acr login -n optlsample.azurecr.io
docker push optlsample.azurecr.io/poc/poc-processor:1.0
docker push optlsample.azurecr.io/poc/poc-generator:1.0
```

## Locally

1. Start collector
   create configuration file as following

```yaml
# otel-local-config.yaml
receivers:
  otlp:
    protocols:
      grpc:
      http:
exporters:
  azuremonitor:
    instrumentation_key: 58b88ff2-bf29-4d30-9113-f49a8f90ef65
  logging:
    loglevel: debug

processors:
  batch:
service:
  pipelines:
    traces:
      receivers: [otlp]
      processors: [batch]
      exporters: [logging, azuremonitor]
    logs:
      receivers: [otlp]
      processors: [batch]
      exporters: [logging, azuremonitor]
    metrics:
      receivers: [otlp]
      processors: [batch]
      exporters: [logging]
```

Run Collector locally.

```sh
cd otel-config/

docker run \
-v "${PWD}/otel-local-config.yaml":/otel-local-config.yaml \
-p 4318:4318 -p 4317:4317 \
otel/opentelemetry-collector-contrib-dev:latest \
--config otel-local-config.yaml;
```

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

3. Add secretes
4. Apply deployments

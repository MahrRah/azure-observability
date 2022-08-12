# Cluster

## Deployment
1.Connect ACR to AKS
```sh
az aks update -n observabilitySpikes -g k8sobs --attach-acr optlsample
```

To dry run you deployment use:

```sh
kubectl kustomize <kustomization_directory>
```

Deply using kustomization :

```sh
kubectl apply -k <kustomization_directory>
```

Add secretes

```sh
kubectl create secret generic poc-creds --from-literal event-connection-string='<connection-string>'
````
# Poc.Generator module

Running locally, ensure Mosquitto is running as documented in the root readme file.

- Create a folder /configuration
- Create a file `appconfig.yaml`

```yaml
# file appconfig.yaml
frequencySeconds: 5
medium: "liquid"
```

- Create a file `infraconfig.yaml`

```yaml
# file infraconfig.yaml
mqttUrl: "localhost"
topic: "output"
```

From within current folder:

```sh
dotnet run
```

## Build docker on dev machine

This commend needs to be executed at the level above this current folder (so move to `/src`). You will use the -f parameter to pass the child dockerfile.

```sh
cd src
ACR=<your_acr>
VERSION=<version>
docker build --pull --rm -f "Poc.Generator/Dockerfile" -t $ACR/poc/poc-generator:$VERSION .
```

Test the docker locally

Docker to docker container is not able to use `localhost` URI for Mosquitto. You can use the IP address of your host instead (`ifconfig` or `ipconfig` to get it)

```yaml
mqttUrl: "x.x.x.x"
topic: "output"
```

Run the container, map the configuration folder into the container's /app/configuration folder:

```sh
docker run -it -v /home/[yourpathtoconfigfolder]/Poc.Generator/configuration:/app/configuration $ACR/poc/poc-generator:$VERSION
```

Push image to ACR

```sh
docker push $ACR/poc/poc-generator:$VERSION
```

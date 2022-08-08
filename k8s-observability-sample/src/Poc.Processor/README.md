# `PoC.Processor` Solution

The `PoC.Processor` module is designed to subscript to an MQTT topic, process messages and send the results to the cloud, and save the messages into storage local storage.

## Setup

### Prerequisite

- Azure Event Hub provisioned (see [docs](https://docs.microsoft.com/en-us/azure/event-hubs/event-hubs-create))
- Azure Storage acount with container provisioned (see [docs](https://docs.microsoft.com/en-us/azure/storage/common/storage-account-create?tabs=azure-portal))
- MQTT Broker running

### Run project locally

1. Environment variables

   Create `.env` file in the root of the project with the connection string end Eventhub name

   ```sh
   #.env-template
   EVENTHUB_CONNECTION_STRING=<namespace-connection-string>
   EVENTHUB_NAME=<eventhub-name>
   MQTT_TOPIC=output
   MQTT_URL=localhost
   CONTAINER_NAME=<container-name>
   AZURE_STORAGE_CONNECTION_STRING=<storage-connection-string>
   ```

   Load key-value pairs into environment variables

   ```sh
   export $(grep -v '^#' .env | xargs)
   ```

   > Note:
   >
   > - MQTT_URL only works set to `localhost` if you are not running the project in a container.
   > - StorageAccount connection string should not have leading and trailing quotations when defined in `.env`.

2. Build

   To build the `PoC.Processor` Solution go to the `src/PoC.Processor` directory and run the following commands:

   ```sh
   dotnet build
   ```

3. Run

   To run the solution you can use the next command for the `src/PoC.Processor` directory

   ```sh
   dotnet run
   ```

### Run project in a container

1. Build container image

   To build the container image for `Poc.Processor` run the following command from within the `src/` folder.

   ```sh
   cd src
   ACR=<your_acr>
   VERSION=<version>
   docker build --pull --rm -f "Poc.Processor/Dockerfile" -t $ACR/poc/poc-processor:$VERSION  .
   ```

2. Run container

   After the image is built, you can run it locally.
   For that copy your`.env` file to `.env-docker` change your MQTT_URL to your local IP and run

   ```sh
   docker run  $ACR/poc/poc-processor:$VERSION  --env-file Poc.Processor/.env-docker
   ```

3. Push

   To be able to run the application in the k8s cluster, the images need to be also pushed to a repository.

   ```sh
   docker push   $ACR/poc/poc-processor:$VERSION
   ```

## Notes

You can check your local IP using

- MacOS: `ifconfig | grep inet`
- Windows: `??`

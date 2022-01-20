# IoT Edge
## Metrics
*  [Monitoring module](https://docs.microsoft.com/en-us/azure/iot-edge/how-to-collect-and-transport-metrics?view=iotedge-2020-11)
 Uses Prometheus formate.
    * List of metrics https://github.com/Azure/iotedge/blob/master/doc/BuiltInMetrics.md
    * Visualization [Workbook](https://docs.microsoft.com/en-us/azure/iot-edge/how-to-explore-curated-visualizations?view=iotedge-2020-11&tabs=devices%2Cmessaging)
    * Use Prometheus client to create custom metrics
    * Configured using deployment manifest 
      ```json
      "monitorModule": {
              "type": "docker",
              "status": "running",
              "restartPolicy": "always",
              "env": {
                "UploadTarget": {
                  "value": "AzureMonitor"
                },
                "LogAnalyticsWorkspaceId": {
                  "value": "${LOG_ANALYTICS_WORKSPACE_ID}"
                },
                "LogAnalyticsSharedKey": {
                  "value": "${LOG_ANALYTICS_WORKSPACE_KEY}"
                },
                "hubResourceID": {
                  "value": "${IOT_HUB_RESOURCE_ID}"
                },
                "MetricsEndpointsCSV": {
                  "value": "${METRICS_ENDPOINT}"
                }
              },
               "settings": {
                    "image": "mcr.microsoft.com/azuremonitor/containerinsights/ciprod:iot-0.1.3.3",
                    "createOptions": {}
                  }
          }
      ```
    * Scraped metrics are accessible in the `IoT Resource > Logs` in the `InsightsMetrics` table.

## Logs
* [ELMS](https://docs.microsoft.com/en-us/azure/iot-edge/how-to-retrieve-iot-edge-logs?view=iotedge-2020-11) sample to use direct method for log retival (pull method).
    * Makes use fo direct method `GetModuleLogs` and time-triggered function to periodically get logs. 
        * **Careful** This might cause duplication of lines of logs.
    * **Down side:** HTTP request, henc **needs** Internet connectivity 
- [Logging Drivers](https://github.com/suneetnangia/iot-edge-logging-fluentd) (pull method)

## Tracing
* Potentially the [In-Build Tracing (in Preview)](https://docs.microsoft.com/en-us/azure/iot-hub/iot-hub-distributed-tracing) can be used for this. 



# IoT Hub
## Metrics
* [In-Build Metrics](https://docs.microsoft.com/en-us/azure/iot-hub/monitor-iot-hub) are automatically collected
    * Data reference to metrics [here](https://docs.microsoft.com/en-us/azure/iot-hub/monitor-iot-hub-reference)

## Logs
- [In-Build Logs](https://docs.microsoft.com/en-us/azure/iot-hub/monitor-iot-hub#collection-and-routing)

## Tracing
* [In-Build Tracing (in Preview)](https://docs.microsoft.com/en-us/azure/iot-hub/iot-hub-distributed-tracing)
    * Used [tracing context](https://github.com/w3c/trace-context)
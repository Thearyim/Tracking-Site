## Eventsource Monitoring System Demo

### Preliminaries
* Open Visual Studio and Build Solution 
* Open command prompt to output path (Ex: C:\Source\Tracking-Site.bin\Debug)
* Start MAMP to ensure MySQL is running.
* Open MyPHPAdmin site and have database ready to show.

### Demo: Explain What a Synthetic Transaction is  
A synthetic transaction is an automated test that is executed against a live system in order to determine if the system is working as expected.
The Eventcore monitoring system that will be demoed executes calls against live websites and writes the status (e.g. Online/Offline) to a backing data store.

### Demo: Explain Major Components

**MySQL Database**  
A SQL database that stores information about telemetry events. Each telemetry event describes the outcome of a synthetic transaction.  

**Synthetic Transaction Service**  
A simple service that automates the execution of synthetic transactions repeatedly over a specified interval of time. 

**Telemetry API**  
A REST service that enables other system component to read/write telemetry event data from/to the backing SQL server. 

**Configuration API**  
A REST service that enables other system component to read/write configuration settings required for opertation. 
This service is hosted in the same application as the Telemetry API.

**Monitoring Status Website**  
A React/Redux-based website that displays the status of websites that are monitored by the Synthetic Transaction Service. 

### Demo: Configuring the System
* **Open command prompt and start the API using the 'Start-Api.ps1' PowerShell commandlet.**

```
cd C:\Source\Tracking-Site.bin\Debug
powershell .\Start-Api.ps1
```

* **Open Chrome, then YARC and PUT a new configuration**  
This configuration provides the Synthetic Transaction Service with the list of websites that it needs to monitor for
availability.

```
PUT http://localhost:5000/api/configuration

{
  "Id": "test",
  "SyntheticTransactions": {
    "WebsiteAvailability": {
      "RunInterval": "00:00:15",
      "Sites": [
        {
          "name": "Eventcore",
          "url": "https://www.eventcore.com"
        },
        {
          "name": "Google Search",
          "url": "https://www.google.com"
        },
        {
          "name": "ICHS",
          "url": "https://www.ichs.com"
        },
        {
          "name": "Microsoft",
          "url": "https://www.microsoft.com"
        },
        {
          "name": "Non-Existent Site",
          "url": "https://www.this.site.does.not.exist.com"
        }
      ]
    }
  }
}
```

### Demo: Writing Telemetry Event Data to System 

* **Post the Following Telemetry Event using YARC and Show the Database**  
This shows how the Synthetic Transaction Service can use the Telemetry API to write telemetry events to the system
and how the monitoring website can use the Telemetry API to read telemetry events from the system.

Note:  
The 'correlation ID' is an identifier used to group a set of individual calls to websites together.

```json
POST http://localhost:5000/api/telemetry

{
   'eventName': 'WebsiteAvailabilityCheck',
   'correlationId': 'e915e981-92ae-4968-84c1-52437f3c265a',
   'timestamp' : '2019-07-08T00:00:00.0000000Z',
   'context': {
      'name' : 'Eventcore',
      'url' : 'https://www.eventcore.com/',
      'status' : 'OK',
      'statusCode': 200
    }
}

```

* **Then get the latest telemetry to show this event**  

``` json
GET http://localhost:5000/api/telemetry?eventName=WebsiteAvailabilityCheck&latest=true

```

### Demo: Show the Synthetic Transaction Service in Action
* **Open command prompt and start the API using the 'Start-Hosts.ps1' PowerShell commandlet.**   
This will start the Synthetic Transaction Service and the web server.

```
cd C:\Source\Tracking-Site.bin\Debug
powershell .\Start-Hosts.ps1
```

### Demo: Show the Synthetic Transaction Service and Website Side-by-Side
* **Open a Chrome tab and browse to the website**  

```
http://localhost:8080
```

* **Split-screen the STS and the Website**  
This enables users to see the website refresh as the STS is running and writing new telemetry.
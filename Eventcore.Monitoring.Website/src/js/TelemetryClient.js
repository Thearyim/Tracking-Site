/*
 * Provides functions for reading data from the Telemetry API.
 * 
 * References:
 * - Fetch Basics
 *   https://javascript.info/fetch-basics
 */
export default class TelemetryClient {
    telemetryApiUri;

    // param: apiUri
    // The absolute URI to the Telemetry API (ex: https://www.eventcore.com:5000/api/telemetry)
    constructor(apiUri) {
        if (apiUri == null) {
            throw "The API URI parameter is required: apiUri";
        }

        this.telemetryApiUri = apiUri;
    }

    async getEvents(eventName = null, latest = null) {
        let targetUri = `${this.telemetryApiUri}/api/telemetry`;
        let result = {
            data: null,
            error: null
        };

        // Format:
        // http://www.eventcore.com:5000/api/telemetry
        // http://www.eventcore.com:5000/api/telemetry?eventName=WebsiteAvailabilityCheck
        // http://www.eventcore.com:5000/api/telemetry?eventName=WebsiteAvailabilityCheck&latest=true
        //
        if (eventName != null && latest != null) {
            let queryParameters = [];
            if (eventName != null) {
                queryParameters.push(`eventName=${eventName}`);
            }

            if (latest != null) {
                queryParameters.push(`latest=${latest}`);
            }

            targetUri += `?${queryParameters.join('&')}`;
        }

        console.log(`Get latest telemetry events: ${targetUri}`);

        try {
            var response = await fetch(targetUri, {
                // mode: 'no-cors',
                method: 'GET',
                headers: {
                    'Accept': 'application/json'
                }
            });

            console.log(`Response: Status ${response.status}-${response.statusText}`);

            if (response.ok) {
                result.data = await response.json().then(json => result.data = json);
            }
            else {
                result.error = `Telemetry API request failed. Status = ${response.status}-${response.statusText}`;
            }
        }
        catch (err) {
            console.log(err);
            result.error = err;
        }

        // console.log(`Result: ${JSON.stringify(result)}`);
        return result;
    }
}

export default class TelemetryClient {
    telemetryApiUri: string;
    constructor(apiUri: string);
    getLatestEvents(eventName: string): any[];
}

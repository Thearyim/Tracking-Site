import React, { useState } from 'react';
import TelemetryClient from 'TelemetryApiClient';

var telemetryClient = new TelemetryClient('http://127.0.0.1:5000');

class HomeContainer extends Component {

    constructor(props) {
        super(props);
    }

    // var telemetryClient = new TelemetryClient('http://127.0.0.1:5000');

    //const [latestEvents, setLatestEvents] = useState(async () => {
    //    var initialEvents = await getTelemetryEvents();
    //    console.log(`Initial Events: ${initialEvents.data.length}`)
    //    return initialEvents;
    //});

    greenTile = {
        color: 'white',
        border: '1px solid black',
        borderRadius: '4px',
        backgroundColor: 'green',
        width: '700px',
        margin: '20px',
        float: 'left'
    };

    redTile = {
        color: 'white',
        border: '1px solid black',
        borderRadius: '4px',
        backgroundColor: 'red',
        width: '700px',
        margin: '20px',
        float: 'left'
    };

    keyCell = {
        borderRight: '1px solid white',
        padding: '5px'
    };

    valueCell = {
        padding: '5px'
    };

    componentWillMount() {
        console.log("Here");
    }

    async getTelemetryEvents() {
        var events = await telemetryClient.getLatestEvents('WebsiteAvailabilityCheck');
        // console.log(`Data: ${JSON.stringify(events.data)}`);

        //for (var i = 0; i < events.data.length; i++) {
        //    console.log(events.data[i]);
        //}

        return events;
    }


    render() {
        if (latestEvents != null) {
        return (
            <div>
                {
                    /*
                       Example JSON Structure
                       {
                          "eventName": "WebsiteAvailabilityCheck",
                          "correlationId": "e6898fce-6151-4bb0-8000-4debb772d548",
                          "timestamp": "2019-06-28T21:54:52",
                          "context": {
                              "url": "https://www.eventcore.com",
                              "status": "OK",
                              "statusCode": 200
                          }
                       }
                     */
                    // latestEvents.data.map((item, key) => {
                    // let siteStatus = parseInt(item.context.statusCode);
                    // let tileStyle = (siteStatus == 200) ? greenTile : redTile;
                    <div style={greenTile}>
                        <table>
                            <tbody>
                                <tr>
                                    <td style={keyCell}>Uri</td>
                                    <td style={valueCell}>{latestEvents.data[0].context.url}</td>
                                </tr>
                                <tr>
                                    <td style={keyCell}>Status</td>
                                    <td style={valueCell}>{latestEvents.data[0].context.statusCode}-{latestEvents.data[0].context.status}</td>
                                </tr>
                                <tr>
                                    <td style={keyCell}>Last Checked</td>
                                    <td style={valueCell}>{latestEvents.data[0].timestamp}</td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                    // })
                }
            </div>);
        }
        else {
            return (
                <div>No telemetry events found...</div>
            );
        }
    }
}

export default HomeContainer;
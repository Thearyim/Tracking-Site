import React from 'react';
import './EventTable.css';

import offlineIcon from 'SiteImages/icon_offline.png';
import onlineIcon from 'SiteImages/icon_online.png';

function EventTable({ events, colorCoded }) {

    let eventCount = 0;
    let isColorCoded = colorCoded === true || colorCoded.toLowerCase() === "true";

    console.log(isColorCoded);

    return (
        <div className="eventTableContainer">
            <table className="eventTable">
                <thead>
                    <tr>
                        <th>Site</th>
                        <th>Timestamp</th>                  
                        <th>Status</th>
                        <th>Context</th>
                    </tr>
                </thead>
                <tbody>
                    {
                        events.map(event => {
                            if (isColorCoded == true) {
                                if (parseInt(event.context.statusCode) >= 300) {
                                    return (
                                        <tr key={eventCount++}>
                                            <td>
                                                <div className="eventStatus">
                                                    <div>
                                                        <img src={offlineIcon} alt="Site Offline" />
                                                    </div>
                                                    <div>
                                                        <b>{event.context.name}</b><br />
                                                        {event.context.url}
                                                    </div>
                                                </div>
                                            </td>
                                            <td>{new Date(Date.parse(event.timestamp)).toLocaleString()}</td>
                                            <td>{event.context.statusCode}-{event.context.status}</td>
                                            <td>{JSON.stringify(event.context)}</td>
                                        </tr>
                                    )
                                }
                                else {
                                    return (
                                        <tr key={eventCount++}>
                                            <td>
                                                <div className="eventStatus">
                                                    <div>
                                                        <img src={onlineIcon} alt="Site Online" />
                                                    </div>
                                                    <div>
                                                        <b>{event.context.name}</b><br/>
                                                        {event.context.url}
                                                    </div>
                                                </div>
                                            </td>
                                            <td>{new Date(Date.parse(event.timestamp)).toLocaleString()}</td>
                                            <td>{event.context.statusCode}-{event.context.status}</td>
                                            <td>{JSON.stringify(event.context)}</td>
                                        </tr>
                                    )
                                }
                            }
                            else
                            {
                                return (
                                    <tr key={eventCount++}>
                                        <td>
                                            <div>
                                                <b>{event.context.name}</b><br />
                                                {event.context.url}
                                            </div>
                                        </td>
                                        <td>{new Date(Date.parse(event.timestamp)).toLocaleString()}</td>
                                        <td>{event.context.statusCode}-{event.context.status}</td>
                                        <td>{JSON.stringify(event.context)}</td>
                                    </tr>
                                )
                            }
                        })
                    }
                </tbody>
            </table>    
        </div>
    );
}

export default EventTable;
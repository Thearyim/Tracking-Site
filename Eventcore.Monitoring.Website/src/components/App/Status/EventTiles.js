import React from 'react';

function EventTiles({ events }) {

    let greenTile = {
        color: 'white',
        border: '1px solid black',
        borderRadius: '4px',
        backgroundColor: 'green',
        width: '400px',
        margin: '20px',
        float: 'left'
    };

    let keyCell = {
        borderRight: '1px solid white',
        padding: '5px'
    };

    let valueCell = {
        padding: '5px'
    };

    let eventCount = 0;

    return (
        <div>            
            {
                events.map(event => {
                    let siteStatus = parseInt(event.context.statusCode);
                    let tileStyle = (siteStatus == 200) ? greenTile : redTile;
                    
                    // console.log(`Event: ${JSON.stringify(event)}`);

                    return (
                        <div key={eventCount++} style={tileStyle}>
                            <table>
                                <tbody>
                                    <tr>
                                        <td style={keyCell}>Uri</td>
                                        <td style={valueCell}>{event.context.url}</td>
                                    </tr>
                                    <tr>
                                        <td style={keyCell}>Status</td>
                                        <td style={valueCell}>{event.context.statusCode}-{event.context.status}</td>
                                    </tr>
                                    <tr>
                                        <td style={keyCell}>Last Checked</td>
                                        <td style={valueCell}>{event.timestamp}</td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    )
                })
            }
        </div>
    );
}

export default EventTiles;
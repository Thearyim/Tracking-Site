import React from 'react';

function SiteState({ event }) {

    let greenTile = {
        color: 'white',
        border: '1px solid black',
        borderRadius: '4px',
        backgroundColor: 'green',
        width: '700px',
        margin: '20px',
        float: 'left'
    };

    let redTile = {
        color: 'white',
        border: '1px solid black',
        borderRadius: '4px',
        backgroundColor: 'red',
        width: '700px',
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

    let siteStatus = parseInt(event.context.statusCode);
    let tileStyle = (siteStatus == 200) ? greenTile : redTile;

    return (
        <div style={tileStyle}>
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
    );
}

export default SiteState;
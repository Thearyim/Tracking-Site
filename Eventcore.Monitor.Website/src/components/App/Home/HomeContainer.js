import React from 'react';
import firebase from '../../../../.firebase/firebase.js';

const HomeContainer = () => {

    var database = firebase.database();

    //function writeUserData(id, name, status, lastPing) {
    //    firebase.database().ref('users/' + userId).set({
    //        id: 1,
    //        name: 'Site 1',
    //        status: 'Status: Online',
    //        lastPing: 'Last Ping: 2019-06-11T00:00:00.0000000Z'
    //    });
    //}

    var greenTile = {
        color: 'white',
        border: '1px solid black',
        borderRadius: '4px',
        backgroundColor: 'green',
        width: '400px',
        margin: '20px',
        float: 'left'
    };

    var redTile = {
        color: 'white',
        border: '1px solid black',
        borderRadius: '4px',
        backgroundColor: 'red',
        width: '400px',
        margin: '20px',
        float: 'left'
    };

    var keyCell = {
        borderRight: '1px solid white',
        padding: '5px'
    };

    var valueCell = {
        padding: '5px'
    };

    function getSiteState() {
        var sites = [];
        var siteRef = database.ref('/site/');
        
        siteRef.once('value', function(snap) {
            snap.forEach(function(item) {
                var itemVal = item.val();
                console.log(itemVal);
                sites.push(itemVal);
            });
        });

        return sites;
    }

    return (
        <div>
            {
                getSiteState().map((item, key) => {
                    let siteStatus = parseInt(item.status);
                    let tileStyle = (siteStatus == 200) ? greenTile : redTile;
                    alert("refresh");
                    
                    return ([
                        <div key={item.id} style={tileStyle}>
                            <table>
                                <tbody>
                                    <tr>
                                        <td style={keyCell}>Uri</td>
                                        <td style={valueCell}>{item.uri}</td>
                                    </tr>
                                    <tr>
                                        <td style={keyCell}>Status</td>
                                        <td style={valueCell}>{item.status}</td>
                                    </tr>
                                    <tr>
                                        <td style={keyCell}>Last Checked</td>
                                        <td style={valueCell}>{item.lastChecked}</td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    ])
                }
            )}
        </div>
    );
}

export default HomeContainer;
import React from 'react';
import { bindActionCreators } from 'redux';
import { connect } from 'react-redux';
import SiteState from './SiteState.js';
import * as Actions from 'SiteStateActions';
import TelemetryClient from 'TelemetryApiClient';

var refreshTimer;

function StatusContainer({ state, actions, apiUri }) {

    var telemetryClient = new TelemetryClient(apiUri);
    var refreshInProgress = false;

    function setEventRefresh() {
        console.log(`Refreshing event status display: ${new Date(Date.now()).toLocaleString()}`);

        if (refreshTimer == null) {
            console.log("Create refresh interval.");

            refreshTimer = setInterval(async () => {
                if (!refreshInProgress) {
                    try {
                        refreshInProgress = true;
                        let events = await telemetryClient.getLatestEvents('WebsiteAvailabilityCheck');
                        let eventData = [];

                        for (let i = 0; i < events.data.length; i++) {
                            eventData.push(events.data[i]);
                        }

                        actions.setTelemetryEvents(eventData);
                    }
                    finally {
                        refreshInProgress = false;
                    }
                }
            },
            10000);
        }
    }

    setEventRefresh();

    var eventCount = 0;
    var containerStyle = {
        display: 'flex',
        flexWrap: 'wrap'
    };

    if (state.events != null && state.events.data != null && state.events.data.length > 0) {
        return (
            <div style={containerStyle}>
                {
                    state.events.data.map((item) => <SiteState key={eventCount++} event={item} />)
                }
            </div>
        );
    }
    else {
        return (
            <div>...Awaiting event capture</div>
        );
    }
}

const mapStateToProps = (state) => ({
    state: state
});

const mapDispatchToProps = (dispatch) => ({
    actions: bindActionCreators(Actions, dispatch)
});

export default connect(
    mapStateToProps,
    mapDispatchToProps
)(StatusContainer);
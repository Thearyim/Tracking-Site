import React from 'react';
import { bindActionCreators } from 'redux';
import { connect } from 'react-redux';
import * as Actions from 'SiteStateActions';
import TelemetryClient from 'TelemetryApiClient';
import EventTable from './EventTable.js';

import './SiteStatusContainer.css';

// Timer is used to define intervals at which the data will be
// refreshed
var refreshTimer;
var refreshInProgress = false;

function SiteStatusContainer({ state, actions, apiUri }) {

    var telemetryClient = new TelemetryClient(apiUri);

    /*
        Function calls the Telemetry API to refresh the list of events.
    */
    async function refreshEvents() {
        if (!refreshInProgress) {
            try {
                refreshInProgress = true;
                let events = await telemetryClient.getEvents('WebsiteAvailabilityCheck', true);
                let eventData = [];

                for (let i = 0; i < events.data.length; i++) {
                    eventData.push(events.data[i]);
                }
                
                actions.setSiteStatus(eventData);
            }
            finally {
                refreshInProgress = false;
            }
        }
    }

    /*
        Function sets the event refresh timer and gets the latest
        telemetry events from the Telemetry API at each interval.
     */ 
    function setRefresh() {
        console.log(`Refreshing event status display: ${new Date(Date.now()).toLocaleString()}`);

        if (refreshTimer == null) {
            console.log("Create refresh interval.");
            refreshTimer = setInterval(async () => await refreshEvents(), 10000);
        }
    }

    // Begin telemetry event refresh
    refreshEvents();
    setRefresh();

    let currentState = state.siteStatus;
    console.log(`Current State: ${JSON.stringify(currentState)}`);

    if (currentState != null && currentState.error != null) {
        return (
            <div className="status-error">
                <div>
                    ...An error occurred while attempting to get telemetry event data: {currentState.error.message}
                </div>
                <div>
                    ...The web application will continue to poll for telemetry event data.
                </div>
            </div>
        )
    }
    else if (currentState != null && currentState.data != null && currentState.data.length > 0) {
        return (
            <div>
                <EventTable events={currentState.data} colorCoded='true' />
            </div>
        );
    }
    else {
        return (
            <div className="status-waiting">...Awaiting event capture.</div>
        );
    }
}

/*
    React-Redux Integration
    Function connects the state of this component with application redux state
    management. This causes the state specified to be passed to this component
    in standard React 'props'.
 */ 
const mapStateToProps = (state) => ({
    state: state
});

/*
    React-Redux Integration
    Function connects the actions required by this component with application redux state
    management. This causes the actions specified to be passed to this component
    in standard React 'props'.
 */
const mapDispatchToProps = (dispatch) => ({
    actions: bindActionCreators(Actions, dispatch)
});

/*
    Function connects this component to Redux state management using the
    state and action specifiers provided.
 */ 
export default connect(
    mapStateToProps,
    mapDispatchToProps
)(SiteStatusContainer);
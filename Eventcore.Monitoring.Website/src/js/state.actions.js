/*
    Constants, Enumerations
*/
export const ACTION_TYPES = {
    REFRESH_EVENTS: "REFRESH_EVENTS",
    REFRESH_EVENT_STATUS: "REFRESH_EVENT_STATUS"
};

/*
    Action Creators
*/
export const setSiteEvents = (events) => ({
    type: ACTION_TYPES.REFRESH_EVENTS,
    events: events
});

/*
    Action Creators
*/
export const setSiteStatus = (events) => ({
    type: ACTION_TYPES.REFRESH_EVENT_STATUS,
    events: events
});

/*
    Action Reducers
*/
export const telemetryEventReducer = (state = [], action) => {
    let newState = undefined;

    // State Object Format:
    // siteEvents: {
    //    id: 'events',
    //    data: []
    // },
    // siteStatus: {
    //    id: 'eventStatus',
    //    data: []
    // }

    switch (action.type) {
        case ACTION_TYPES.REFRESH_EVENTS:

            // Refresh event status used to display current availability
            // of the sites.
            newState = {
                siteEvents: {
                    id: state.siteEvents.id,
                    data: action.events // update events
                },
                siteStatus: {
                    id: state.siteStatus.id,
                    data: state.siteStatus.data
                }
            };

            break;

        case ACTION_TYPES.REFRESH_EVENT_STATUS:

            // Refresh the list of all events showing the outcome of current
            // as well as historical site validation checks.
            newState = {
                siteEvents: {
                    id: state.siteEvents.id,
                    data: state.siteEvents.data
                },
                siteStatus: {
                    id: state.siteStatus.id,
                    data: action.events // update events
                }
            };

            break;

        default:
            console.log(`Unchanged state`);
            newState = state;
            break;
    }

    // console.log(`New State: ${JSON.stringify(newState)}`);

    return newState;
};
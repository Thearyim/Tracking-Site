/*
    Constants, Enumerations
*/
export const ACTION_TYPES = {
    REFRESH_EVENTS: "REFRESH_EVENTS"
};

/*
    Action Creators
*/
export const setTelemetryEvents = (events) => ({
    type: ACTION_TYPES.REFRESH_EVENTS,
    events: events
});

/*
    Action Reducers
*/
export const telemetryEventReducer = (state = [], action) => {
    let newState = undefined;

    switch (action.type) {
        case ACTION_TYPES.REFRESH_EVENTS:
            newState = {
                events: {
                    id: 'events',
                    data: action.events
                }
            };

            break;

        default:
            console.log(`Unchanged state`);
            newState = state;
            break;
    }

    console.log(`New State: ${JSON.stringify(newState)}`);

    return newState;
};
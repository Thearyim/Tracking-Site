import React from 'react';
import { Switch, Route } from 'react-router-dom';
import { createStore } from 'redux';
import { Provider } from 'react-redux';

import * as siteState from 'SiteState';
import { telemetryEventReducer } from 'SiteStateActions';

import HeaderContainer from './Header/HeaderContainer.js';
//import HomeContainer from './Home/HomeContainer.js';
import StatusContainer from './Status/StatusContainer.js';
import AdminLogIn from './Admin/AdminLogIn.js';
import AdminFormPage from './Admin/AdminFormPage.js';

function App() {

    const initialState = siteState.initialState;
    const stateStore = createStore(telemetryEventReducer, initialState);

    return (
        <div>
            <HeaderContainer />
            <Provider store={stateStore}>
                <Switch>
                    <Route
                        exact path="/"
                        render={(props) => (
                            <StatusContainer apiUri="http://127.0.0.1:5000" state={initialState.events} />
                        )}
                    />
                    <Route path="/AdminLogIn" render={(props) => (<AdminLogIn />)} />
                    <Route path="/AdminFormPage" render={(props) => (<AdminFormPage />)} />
                </Switch>
            </Provider>
        </div>
    );
}

export default App;
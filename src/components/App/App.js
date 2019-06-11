import React from 'react';
import { Switch, Route } from 'react-router-dom';
import HeaderContainer from './Header/HeaderContainer.js';
import HomeContainer from './Home/HomeContainer.js';
import RealTimeContainer from './RealTime/RealTimeContainer.js';
import AdminLogIn from './Admin/AdminLogIn.js';
import AdminFormPage from './Admin/AdminFormPage.js';

function App(){
  return (
    <div>
      <style jsx>{`

      `}</style>

          <div>
              <HeaderContainer />
              <HomeContainer />
              <Switch>
                  <Route
                      path="/realtime"
                      render={(props) => (
                          <RealTimeContainer />
                      )}
                  />
                  <Route path="/AdminLogIn" render={(props)=>(<AdminLogIn/>)}/>
                  <Route path="/AdminFormPage" render={(props)=>(<AdminFormPage/>)}/>
              </Switch>
      </div>
    </div>
  );
}

export default App;
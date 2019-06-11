import React from 'react';
import { Switch, Route } from 'react-router-dom';
import HeaderContainer from './Header/HeaderContainer.js';
import HomeContainer from './Home/HomeContainer.js';
import RealTimeContainer from './RealTime/RealTimeContainer.js';

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
              </Switch>
      </div>
    </div>
  );
}

export default App;
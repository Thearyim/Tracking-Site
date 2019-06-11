import React from 'react';
import { Switch, Route } from 'react-router-dom';
import HeaderContainer from './Header/HeaderContainer.js';
import HomeContainer from './Home/HomeContainer.js';

function App(){
  return (
    <div>
      <style jsx>{`

      `}</style>

          <div>
              <HeaderContainer />
              <Switch>
                  <Route
                      path="/"
                      render={(props) => (
                          <HomeContainer />
                      )}
                  />
              </Switch>
      </div>
    </div>
  );
}

export default App;
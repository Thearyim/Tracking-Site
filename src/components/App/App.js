import React from 'react';
import { Switch, Route } from 'react-router-dom';
import HeaderContainer from './Header/HeaderContainer.js';
import HomeContainer from './Home/HomeContainer.js';
import AdminLogIn from './Admin/AdminLogIn.js';
import AdminFormPage from './Admin/AdminFormPage.js';

function App(){
  return (
          <div>
              <HeaderContainer />
              <Switch>
                  <Route
                      exact path="/"
                      render={(props) => (
                          <HomeContainer />
                      )}
                  />
                  <Route path="/AdminLogIn" render={(props)=>(<AdminLogIn/>)}/>
                  <Route path="/AdminFormPage" render={(props)=>(<AdminFormPage/>)}/>
              </Switch>
      </div>
  );
}

export default App;
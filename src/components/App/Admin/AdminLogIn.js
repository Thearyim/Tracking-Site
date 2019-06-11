import React from 'react';
import { Link } from 'react-router-dom';

function AdminLogIn(){
  return (
    <div>
      <style jsx>{`
      .body{
        text-align:center;
      }
      h2{
        padding-bottom:50px;
      }
      `}</style>
<div>
<form className="body">
      <h2>Only Admins can Sign In!</h2>
  <h3>User Name</h3>
          <input
            type='text'
            id='Name'
            placeholder='Url'/>
          <br />
          <h3>Password</h3>
          <textarea
            type='text'
            id='password'
            placeholder='Password'/>
          <br/>
          <Link to="/AdminFormPage"><button type='submit'>Sign-In</button></Link>
        </form>
</div>
</div>
  );
}

export default AdminLogIn;
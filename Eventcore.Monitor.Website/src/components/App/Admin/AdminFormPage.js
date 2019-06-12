import React from 'react';

function AdminFormPage(){
  return (
    <div>
      <style jsx>{`
      .body{
        text-align:center;
      }
      `}</style>
<div>
<form className="body">
  <h3></h3>
          <input
            type='text'
            id='url'
            placeholder='Url'/>
          <br />
          <h3>Email</h3>
          <textarea
            type='text'
            id='email'
            placeholder='Email' required/>
          <br/>
          <button type='submit'>Add</button>
        </form>
</div>
</div>
  );
}

export default AdminFormPage;

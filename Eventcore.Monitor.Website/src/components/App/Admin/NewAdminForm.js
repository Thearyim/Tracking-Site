
import React from 'react';
import PropTypes from 'prop-types';
import { v4 } from 'uuid';




function NewAdminForm(props) {
  let url = null;
  let email = null;

  function handleNewFormSubmission(event) {
    event.preventDefault();
    props.onAddForm({ url: url.value, email: email.value, id: v4() });
    url.value = '';
    email.value = '';
  }


  return (
    <div>
      <style jsx>{`
      `}</style>
      <div>
        <form onSubmit={handleNewFormSubmission}>
          <input
            type='text'
            id='url'
            placeholder='Url'
            ref={(input) => { url = input; }} required />
          <br />
          <textarea
            type='text'
            id='email'
            placeholder='Email'
            ref={(input) => { email = input; }} required/>
          <br />
          <button type='submit'>New Form!</button>
        </form>
      </div>
    </div>
  );
}
NewAdminForm.propTypes = {
  onAddForm: PropTypes.func
};

export default NewAdminForm;
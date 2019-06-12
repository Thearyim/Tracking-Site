import React from 'react';
import PropTypes from 'prop-types';

function Form(props){
  return (
    <div>
      <h3>{props.url} - {props.email}</h3>
      <hr/>
    </div>
  );
}

Form.propTypes = {
  url: PropTypes.string.isRequired,
  email: PropTypes.string.isRequired,
};

export default Form;
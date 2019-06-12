import React from 'react';
import Form from './Form';
import PropTypes from 'prop-types';


function AdminList(props){
  return (
    <div>
      <hr/>
      {props.adminList.map((form) =>
        <Form url={form.url}
          email={form.email}
          key={form.id}/>
      )}
    </div>
  );
}
AdminList.propTypes = {
  adminList: PropTypes.array
};


export default AdminList;
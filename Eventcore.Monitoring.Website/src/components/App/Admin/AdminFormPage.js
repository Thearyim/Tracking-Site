import React from 'react';
import NewAdminForm from './NewAdminForm';
import AdminList from './AdminList';


class AdminFormPage extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      websites: []
    };
  }



  handleAddingFormToArray(newForm) {
    const newFormArray = this.state.websites;
    newFormArray.push(newForm);
    this.setState({websites: newFormArray});
    console.log(this.state);
  }

  render() {
    return (
      <div>
        <NewAdminForm
          onAddForm={e => this.handleAddingFormToArray(e)}
        />
        <AdminList
          adminList={this.state.websites}
        />
      </div>
    );
  }
}

export default AdminFormPage;

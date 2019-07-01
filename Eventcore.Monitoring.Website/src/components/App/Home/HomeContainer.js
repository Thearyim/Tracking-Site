import React from 'react';

function HomeContainer({ children }) {

    var containerStyle = {
        marginLeft: "30px",
        marginRight: "30px"
    };

    return (
        <div style={containerStyle}>
            { children }
        </div>
    )
}

export default HomeContainer;
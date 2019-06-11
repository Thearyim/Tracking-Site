import React from 'react';
import MockData from '_MockDataJs';

const RealTimeContainer = () => {

    var mockData = new MockData();

    return (
        <div>
            {
                mockData.getStatus().map((item, key) =>
                <div>{item.id}</div>)
            }
        </div>
    );
}

export default RealTimeContainer;

import React from 'react';
import MockData from '_MockDataJs';

const HomeContainer = () => {

    var mockData = new MockData();

    function getStatusDateString(statusDates) {
        var dates = mockData.getDates(statusDates, '')
        var options = {
            year: 'numeric',
            month: 'long',
            day: 'numeric'
        };

        var statusDate = new Date(date.begin);
        return statusDate.toLocaleDateString("en-US", options);
    }

    return (
        <div>
            {
                mockData.getStatus().map((item, key) =>
                    <div>{item.id} {item.name} {item.status} {item.lastPing}</div>)
            }
        </div>
    );
}

export default HomeContainer;
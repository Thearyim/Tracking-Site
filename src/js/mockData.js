
class MockData {

    getDates(dateObjects, type) {
        var dates = [];
        dateObjects.forEach((date) => {
            if (date.description == type) {
                dates.push(date);
            }
        });

        // console.log(dates);
        return dates;
    }

    getJobs(site) {
        return [
            {
                id: 1,
                name: 'Site 1',
                url: 'ul: Event 1'
            },
            {
                id: 2,
                name: 'Site 2',
                url: 'url: Event 2'
            },
            {
                id: 3,
                name: 'Site 3',
                url: 'url: Event 3'
            }
        ]
    }

    getStatus() {
        return [
            {
                id: 1,
                name: 'Site 1',
                status: 'Status: Online',
                lastPing: 'Last Ping: 2019-06-11T00:00:00.0000000Z'
            },
            {
                id: 2,
                name: 'Site 2',
                status: 'Status: Offline',
                lastPing: 'Last Ping: 2019-06-11T00:00:00.0000000Z'
            },
            {
                id: 3,
                name: 'Site 3',
                status: 'Status: Online',
                lastPing: 'Last Ping: 2019-06-11T00:00:00.0000000Z'
            }
        ]
    }
}

export default MockData;


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
            }
        ]
    }

    getStatus() {
        return [
            {
                id: 1,
                name: 'Site 1',
                status: 'Description: Event 1',
                lastPing: 'Description: Event 1'
            },
            {
                id: 2,
                name: 'Site 2',
                status: 'Description: Event 2',
                lastPing: 'Description: Event 1'
            }
        ]
    }
}

export default MockData;

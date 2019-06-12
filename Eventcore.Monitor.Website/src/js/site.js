class SiteUtils {

    getFormattedDate(dateBegin, dateEnd) {
        // Example Format:
        // Monday July 5th, 2019 8:00am - 5:00pm
        var options = {
            weekday: 'long',
            year: 'numeric',
            month: 'long',
            day: 'numeric',
            hour: 'numeric',
            minute: '2-digit'
        };

        var timezoneOptions = {
            timeZoneName: 'short'
        }

        return `${dateBegin.toLocaleDateString("en-US", options)} - ${dateEnd.toLocaleTimeString("en-US", timezoneOptions)}`;
    }
}

export default SiteUtils;
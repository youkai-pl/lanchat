//COMMON

module.exports = {

    //get time
    time: function () {
        var date = new Date();
        var time = (("0" + date.getHours()).slice(-2) + ":" +
            ("0" + date.getMinutes()).slice(-2) + ":" +
            ("0" + date.getSeconds()).slice(-2));
        return time;
    }
}


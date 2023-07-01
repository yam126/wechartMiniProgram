var com = new function () {
    this.data = {
        user: null,
        token:''
    }
    //通用-警示框
    this.showAlertMessage = function (message) {
        com.consoleInfo(message)
        $('.topmsg').show();
        $('.topmsg .message').html(message);
        setTimeout(com.hidMessage, 2000);
    }
    this.hidMessage = function () {
        $('.topmsg').hide();
    }
    this.consoleInfo = function (message) {
        console.info(message);
    }
    this.initPage = function () {
        com.checkLogind();
        //com.consoleInfo($('body').css('font-size').replace('px', ''))
        //$('body').css('font-size', parseInt($('body').css('font-size').replace('px', '')) / parseInt($('html').data('dpr')));
    }
    this.checkLogind = function () {

        var user = com.getUser();
        if (user != null) {
            com.user = user;
            com.token = user.token;
        } else {
            com.user = null;
            com.token = '';
        }
    }
    this.setUser = function (userObj) {
        com.data.user = userObj;
        com.data.token = userObj.token;
        var userStr = JSON.stringify(userObj);
        $.cookie("user", userStr, { expires: 1 });
    }
    this.getUser = function () {
        var userStr = $.cookie("user");
        if (userStr == null) return null;
        var userObj = JSON.parse(userStr);
        
        if (userObj != null)
        {
            com.data.token = userObj.token;
            com.data.user = userObj;
        }
           
        return userObj;
    }
}

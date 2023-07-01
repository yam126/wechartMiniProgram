var app = getApp()
var webapi = {
    getAllGuide(callback) {
        console.log('getAllGuide')
        console.log(app.com.api)
        wx.request({
            url: app.com.api + 'GetAllGuideList',
            data: {},
            async: false,
            method: 'POST',
            success: function (res) {
                console.log(res)
                callback(res)
            },
            fail: function (res) {
                console.log('is fail')
                console.log(res)
            }
        })
    },
    GuidQuickRegist(guideObj, callback) {
        var vurl = app.com.api + 'GuidQuickRegist'
        console.log(vurl)
        wx.request({
            url: vurl,
            data: JSON.stringify(guideObj),
            async: false,
            method: 'POST',
            success: function (res) {
                console.log(res)
                callback(res)
            },
            fail: function (res) {
                console.log('is fail')
                console.log(res)
            }
        })
    },
    GuideIsRegist(phoneNumber, callback) {
        var vurl = app.com.api + 'GuideIsRegist?PhoneNumber=' + phoneNumber
        console.log(vurl)
        wx.request({
            url: vurl,
            data: {},
            async: false,
            method: 'POST',
            success: function (res) {
                console.log(res)
                callback(res)
            },
            fail: function (res) {
                console.log('is fail')
                console.log(res)
            }
        })
    },
    getGuideInfo(guideid, callback) {
        wx.request({
            url: app.com.api + 'GetGuideInfo?guideid=' + guideid,
            data: {},
            async: false,
            method: 'POST',
            success: function (res) {
                console.log(res)
                callback(res)
            },
            fail: function (res) {
                console.log('is fail')
                console.log(res)
            }
        })
    },
    GetGuideOrders(guideid, callback) {
        wx.request({
            url: app.com.api + 'GetGuideOrders?vguideId=' + guideid,
            data: {},
            async: false,
            method: 'POST',
            success: function (res) {
                console.log(res)
                callback(res)
            },
            fail: function (res) {
                console.log('is fail')
                console.log(res)
            }
        })
    },
    SaveGuide(guideInfo, callback) {
        var vurl = app.com.api + 'SaveGuide'
        console.log(vurl)
        wx.request({
            url: vurl,
            data: JSON.stringify(guideInfo),
            async: false,
            method: 'POST',
            success: function (res) {
                console.log(res)
                callback(res)
            },
            fail: function (res) {
                console.log('is fail')
                console.log(res)
            }
        })
    },
    ChangeGuideBusyState(PhoneNumber, BusyState, callback) {
        wx.request({
            url: app.com.api + 'ChangeGuidBusyState?PhoneNumber=' + PhoneNumber + '&BusyState=' + BusyState,
            data: {},
            async: false,
            method: 'POST',
            success: function (res) {
                console.log(res)
                callback(res)
            },
            fail: function (res) {
                console.log('is fail')
                console.log(res)
            }
        })
    },
    GetGuideInfoByPhoneNumber(phoneNumber, callback) {
        wx.request({
            url: app.com.api + 'GetGuideInfoByPhoneNumber?PhoneNumber=' + phoneNumber,
            data: {},
            async: false,
            method: 'POST',
            success: function (res) {
                console.log(res)
                callback(res)
            },
            fail: function (res) {
                console.log('is fail')
                console.log(res)
            }
        })
    }
}
module.exports = webapi
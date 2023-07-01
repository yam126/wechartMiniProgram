// pages/regist/regist.js
const app = getApp()
const webapi = require('../../utils/webapi.js')
Page({

    /**
     * 页面的初始数据
     */
    data: {
        PhoneNumber: '',
        canIUseOpenData: wx.canIUse('open-data.type.userAvatarUrl')
    },

    /**
     * 生命周期函数--监听页面加载
     */
    onLoad(options) {
        var that=this
        console.log(options)
        if(options!=null&&typeof(options)!='undefined')
        {
           that.setData({
               PhoneNumber:options.PhoneNumber
           })
        }
    },

    /**
     * 生命周期函数--监听页面初次渲染完成
     */
    onReady() {

    },

    /**
     * 生命周期函数--监听页面显示
     */
    onShow() {

    },

    /**
     * 生命周期函数--监听页面隐藏
     */
    onHide() {

    },

    /**
     * 生命周期函数--监听页面卸载
     */
    onUnload() {

    },

    /**
     * 页面相关事件处理函数--监听用户下拉动作
     */
    onPullDownRefresh() {

    },

    /**
     * 页面上拉触底事件的处理函数
     */
    onReachBottom() {

    },

    /**
     * 用户点击右上角分享
     */
    onShareAppMessage() {

    },
    onRegistClick() {
        var that = this
        console.log(this)
        console.log('that.data.PhoneNumber='+that.data.PhoneNumber)
        if (that.data.PhoneNumber == ''||typeof(that.data.PhoneNumber)=='undefined') {
            wx.showToast({
                icon: 'error',
                title: '手机号为空不能注册'
            })
            return false
        }
        wx.getUserInfo({
            success: function (res) {
                console.log(res)
                webapi.GuidQuickRegist(
                    {
                        NickName:res.userInfo.nickName,
                        UserFaceUrl: res.userInfo.avatarUrl,
                        PhoneNumber:that.data.PhoneNumber
                    },
                    function (res) {
                        console.log(res)
                        if(res.data.result=='ok'){
                            app.com.guideInfo=res.data.obj
                            console.log(app.com.guideInfo)
                            wx.switchTab({
                              url: '/pages/index/index'
                            })
                        }
                    }
                )
            }
        })

    },
    receiveInputPhoneNumber(res) {
        this.setData({
            PhoneNumber: res.detail.value
        })
    }
})
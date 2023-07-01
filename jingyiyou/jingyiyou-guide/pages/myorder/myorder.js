// pages/myorder/myorder.js
const app = getApp();
const webapi = require('../../utils/webapi.js')
const utils = require('../../utils/util.js')
Page({

    /**
     * 页面的初始数据
     */
    data: {
        guideOrders:[]//当前页面的所有订单
    },

    /**
     * 生命周期函数--监听页面加载
     */
    onLoad(options) {

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
        this.readGuideOrders()
    },
    //读取当前导游的所有订单
    readGuideOrders(){
        var that=this
        var guideInfo=app.com.guideInfo
        if(guideInfo!=null&&typeof(guideInfo)!='undefined'){
            var guideId=guideInfo.GuideID
            webapi.GetGuideOrders(
                guideId,
                function(res){
                    if (res.data.result != 'ok') {
                        wx.showToast({
                          icon: 'error',
                          title: '读取订单出错,原因[' + res.data.message + ']'
                        })
                        return false
                    }
                    var guideOrders = res.data.obj
                    if (guideOrders != null && typeof (guideOrders) != 'undefined' && guideOrders.length > 0) {
                      //#region 转换时间格式  
                      for (var i = 0; i < guideOrders.length; i++) {
                        if (guideOrders[i].AddDate != null && typeof (guideOrders[i].AddDate) != 'undefined') {
                          var tempDateTime = utils.convertJsonDateTime(guideOrders[i].AddDate)
                          var dateTimeStr=utils.formatDateTime(tempDateTime,'yyyy-MM-dd hh:mm:ss')
                          guideOrders[i].AddDate=dateTimeStr
                        }
                      }
                      //#endregion
                      that.setData({
                        guideOrders: res.data.obj
                      })
                    }                    
                }
            )
        }
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

    }
})
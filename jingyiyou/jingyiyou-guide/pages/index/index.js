// index.js
// 获取应用实例
const app = getApp()
const webapi=require('../../utils/webapi.js')
Page({
    data: {
        selected: 0,
        popup: null,
        orderNumber:0,//接单数
        toDayOrderNum:0,//今日接单
        totalRevenue:0,//收入总额
        withDrawable:0,//可提现
        phoneNumberDialog:null,
        PhoneNumber:'',
        userInfo: {},
        nowStatus:2,//状态默认休息
        hasUserInfo: false,
        guideInfo:null,
        canIUse: wx.canIUse('button.open-type.getUserInfo'),
        canIUseGetUserProfile: false,
        canIUseOpenData: wx.canIUse('open-data.type.userAvatarUrl') && wx.canIUse('open-data.type.userNickName') // 如需尝试获取用户信息可改为false
    },
    pageLifetimes: {
        show() {
            if (typeof this.getTabBar === 'function' &&
                this.getTabBar()) {
                this.getTabBar().setData({
                    selected: 0
                })
            }
        }
    },
    // 事件处理函数
    bindViewTap() {
        wx.navigateTo({
            url: '../logs/logs'
        })
    },
    onLoad(options) {
        this.canIUse=wx.canIUse('button.open-type.getUserInfo')
        console.log("this.canIUse")
        console.log(this.canIUse)
        //获得popup组件
        this.popup = this.selectComponent("#popup");
        this.showInputPhoneNumber();
        if (wx.getUserProfile) {
            this.setData({
                canIUseGetUserProfile: true,              
            })
        }
        if(options!=null&&options.PhoneNumber!=''&&typeof(options.PhoneNumber)!='undefined'){
            this.GetGuideInfoByPhoneNumber(options.PhoneNumber)
        }
    },
    GetGuideInfoByPhoneNumber(PhoneNumber){
        var that=this
        webapi.GetGuideInfoByPhoneNumber(
            PhoneNumber,
            function(res){
                console.log('GetGuideInfoByPhoneNumber')
                console.log(res)
                if(res.data.result=='ok'){
                    that.setData({
                        PhoneNumber:PhoneNumber,
                        guideInfo:res.data.obj.guidInfo,
                        nowStatus:res.data.obj.guidInfo.BusyStatus,
                        orderNumber:res.data.obj.orderNumber,
                        toDayOrderNum:res.data.obj.toDayOrderNum,
                        withDrawable:res.data.obj.withDrawable,
                        totalRevenue:res.data.obj.totalRevenue                     
                    })
                    app.com.guideInfo=res.data.obj.guidInfo
                }
            }
        )
    },
    gotoMyOrder(){
        wx.navigateTo({
          url: '/pages/myorder/myorder'
        })
    },
    gotoMyEdit(){
        wx.navigateTo({
          url: '/pages/myedit/myedit'
        })
    },
    showTabBar() {
        if (typeof this.getTabBar === 'function' &&
            this.getTabBar()) {
            this.getTabBar().setData({
                selected: 0
            })
        }
    },
    onShow() {
        this.showTabBar()
        console.log("onShow")
        var guideInfo=app.com.guideInfo
        console.log(guideInfo)
        if(guideInfo!=null&&typeof(guideInfo)!="undefined"){
            this.GetGuideInfoByPhoneNumber(guideInfo.PhoneNumber)
        }
    },
    _successPhoneNumber(res){
        console.log('_successPhoneNumber')
        console.log(res)
        console.log('res.detail.PhoneNumber='+res.detail.PhoneNumber)
        var vPhoneNumber=res.detail.PhoneNumber
        var that=this
        webapi.GuideIsRegist(
            res.detail.PhoneNumber,
            function(res){
                console.log('api response')
                console.log(res)
                if(res.data.result=='ok'){
                    if(!res.data.obj){
                        wx.showModal({
                            title: '提示',
                            content: '还没有注册是否确定注册？',
                            success: function (res) {
                                if (res.confirm) {
                                    //这里是点击了确定以后
                                    console.log('用户点击确定')
                                    wx.navigateTo({
                                      url: '/pages/regist/regist?PhoneNumber='+vPhoneNumber
                                    })
                                } 
                                else 
                                {
                                    //这里是点击了取消以后
                                    console.log('用户点击取消')
                                }
                            }
                        })
                    }else{
                        that.GetGuideInfoByPhoneNumber(vPhoneNumber)
                    }
                }else{
                    wx.showToast({
                        title: '调用接口检查导游是否注册失败,原因['+res.data.message+']',
                        icon:'error',
                        duration: 1000
                      }
                    )
                }
            }
        )

    },
    showInputPhoneNumber(){
        this.phoneNumberDialog=this.selectComponent("#inputPhoneNum")
        this.phoneNumberDialog.showGetPhoneNumberDialog()
    },
    showPopup() {
        //获得popup组件
        console.log(this.popup)
        this.popup = this.selectComponent("#popup")
        console.log(this.popup)
        this.popup.showPopup(this.data.PhoneNumber,this.data.nowStatus)
    },
    //取消事件
    _error() {
        console.log('你点击了取消')
        this.popup.hidePopup()
    },
    //确认事件
    _success(res) {
        console.log('你点击了确定')
        console.log(res)
        this.popup.hidePopup()
        console.log('res.detail.BusyStatus='+res.detail.BusyStatus)
        this.setData({
            nowStatus:res.detail.BusyStatus
        })
    },
    getUserProfile(e) {
        // 推荐使用wx.getUserProfile获取用户信息，开发者每次通过该接口获取用户个人信息均需用户确认，开发者妥善保管用户快速填写的头像昵称，避免重复弹窗
        wx.getUserProfile({
            desc: '展示用户信息', // 声明获取用户个人信息后的用途，后续会展示在弹窗中，请谨慎填写
            success: (res) => {
                console.log(res)
                this.setData({
                    userInfo: res.userInfo,
                    hasUserInfo: true
                })
            }
        })
    },
    getUserInfo(e) {
        // 不推荐使用getUserInfo获取用户信息，预计自2021年4月13日起，getUserInfo将不再弹出弹窗，并直接返回匿名的用户个人信息
        console.log(e)
        this.setData({
            userInfo: e.detail.userInfo,
            hasUserInfo: true
        })
    }
})

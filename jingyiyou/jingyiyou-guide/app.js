//app.js
App({
    onLaunch: function () {
      var that = this;
      console.log('App onLaunch');
  
    
        that.showallmenu();
      
      // 获取系统信息 
      wx.getSystemInfo({
        success: function (res) {
          that.com.scrollHeight = res.windowHeight
          that.com.winWidth = res.windowWidth
          that.com.winHeight = res.windowHeight
  
        }
      });
      console.log('begin Login');
      // 登录
      wx.login({
        success: res => {
         // console.log(res);
      
          if (res.code) {
            //发起网络请求
            var code = res.code
            that.com.code = code
            console.log('begin getUserInfo');
            wx.getUserInfo({//getUserInfo流程
              success: function (res2) {//获取userinfo成功
                console.log(res2);
                var encryptedData = res2.encryptedData;//一定要把加密串转成URI编码
                var iv = res2.iv;
                //请求自己的服务器
                console.log('Login');
                wx.request({
                  url: that.com.api + '/SetLoginInfo',
                  data: {
                    code: code,
                    //signature: signature,
                    encryptedData: encryptedData,
                    iv: iv,
                    ver: that.com.version
                  },
                  success: function (res3) {
                    if (res3.data.result == 'ok') {
                      console.log(res3.data.obj.token);
                      that.com.token = res3.data.obj.token
                      that.com.showhide = res3.data.obj.showhide
                      that.com.isrenzheng = res3.data.obj.isrenzheng
                      that.com.pushurl = res3.data.obj.pushurl
                      that.com.openid=res3.data.obj.openid
                      
                     
                        that.showallmenu();
                        if (that.com.indexthat != null) {
                          that.com.indexthat.setData({
                            menulist: that.allmenulist
                          })
                        }
                      
  
                      wx.setStorageSync('showhide', res3.data.obj.showhide)
                      wx.setStorageSync('token', res3.data.obj.token)
                      
                      if(that.com.funcloginok!=null){
                        that.com.funcloginok(res3.data.obj.openid)
                      }
                      //wx.setStorageSync('isrenzheng', res3.data.obj.isrenzheng)
                      //console.log('Login success');
                    }
                  }
                })
                //console.log('Login end');
              },
              fail:function(res3){
                console.info(res3)
              }
            })
  
          } else {
            console.log('获取用户登录态失败！' + res.errMsg)
          }        
        }
      })
      console.info("getSetting")
      // 获取用户信息
      wx.getSetting({
        success: res => {
          console.info(res)
          if (res.authSetting['scope.userInfo']) {
            // 已经授权，可以直接调用 getUserInfo 获取头像昵称，不会弹框
            wx.getUserInfo({
              success: res => {
                //console.info(res)
                // 可以将 res 发送给后台解码出 unionId
                this.globalData.userInfo = res.userInfo
                this.com.user=res.userInfo
               // console.info(this.globalData.userInfo)
                // 由于 getUserInfo 是网络请求，可能会在 Page.onLoad 之后才返回
                // 所以此处加入 callback 以防止这种情况
                if (this.userInfoReadyCallback) {
                  this.userInfoReadyCallback(res)
                }
              }
            })
          }
        }
      })
      const systemInfo = wx.getSystemInfoSync(); //获取系统信息
      const menuInfo = wx.getMenuButtonBoundingClientRect(); // 获取胶囊按钮的信息
      this.globalData.menuHeight = menuInfo.height; // 获取胶囊按钮的高
      this.globalData.statusBarHeight = systemInfo.statusBarHeight; // 获取状态栏的高
      this.globalData.menuRight = menuInfo.right; // 获取胶囊按钮的距离屏幕最右边的距离（此处用于设置导航栏左侧距离屏幕的距离）
      this.globalData.navBarHeight = (menuInfo.top - systemInfo.statusBarHeight) * 2 + menuInfo.height; // 计算出导航栏的高度
    },
    globalData: {
      userInfo: null,
      navBarHeight:0,// 导航栏高度
      menuHeight:0,//胶囊按钮 高度
      statusBarHeight:0,//状态栏高度
      menuRight:0//胶囊按钮 距离屏幕右边的距离  
    },
    com: {
      user:null,
      host:'http://localhost:1285/',
      api:'http://localhost:1285/apis/',
      //api: 'https://m.ar.jyweip.com/apis/',
      //api: 'https://m.ncc2019.com/apis/',
      uploadFileApi:'http://localhost:1285/apis/',
      token: 'none',
      guideInfo:null,
      reflushlist: false,
      showchat: false,
      scrollHeight: 0,
      winWidth: 0,
      winHeight: 0,
      showhide: false,
      menulist:[],
      version: '1.1.7',
      xiaoquid: 1,
      isrenzheng: 0,
      indexthat: null,
      code: '',
      pushurl:'',
      openid:null,
      funcloginok:null
    },
    tojson: function (str) {
      return JSON.parse(str)
    },
    tostring: function (obj) {
      return JSON.stringify(obj)
    },
    allmenulist: [    
      { id: 'shouye', imgurl: '../../images/zixun.png', imgurl1: '../../images/zixun1.png', tap: 'menutap', name: '记录' },
      { id: 'order', imgurl: '../../images/video.png', imgurl1: '../../images/video1.png', tap: 'menutap', name: '操作间' },
      { id: 'my', imgurl: '../../images/my.png', imgurl1: '../../images/my1.png', tap: 'menutap', name: '我的' },
    ],
    menutap: function (e) {
      if (e.currentTarget.id == 'shouye') {
        wx.redirectTo({
          url: '../goodlist/index',
        })
      } else if (e.currentTarget.id == 'order') {      
          wx.redirectTo({
            url: '../orderlist/index',
          })     
      } else if (e.currentTarget.id == 'my') {
        wx.redirectTo({
          url: '../my/index',
        })
      } 
  
    },
    showallmenu: function () {
      this.com.menulist = this.allmenulist
    }
  })
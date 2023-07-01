// components/popup/popup.js
const app = getApp()
const webapi=require('../../utils/webapi.js')
Component({
    /**
     * Component properties
     */
    properties: {
      title: {            // 属性名
        type: String,     // 类型（必填），目前接受的类型包括：String, Number, Boolean, Object, Array, null（表示任意类型）
        value: ''     // 属性初始值（可选），如果未指定则会根据类型选择一个
      },
      // 弹窗图片
      image: {
        type: String,
        value: ''
      },
      // 弹窗内容
      content: {
        type: String,
        value: ''
      },
      // 弹窗取消按钮文字
      btn_no: {
        type: String,
        value: '取消'
      },
      // 弹窗确认按钮文字
      btn_ok: {
        type: String,
        value: '确定'
      }
    },
   
    /**
     * Component initial data
     */
    data: {
      flag: true,
      BusyStatus:-1,
      PhoneNumber:''
    },
   
    /**
     * Component methods
     */
    methods: {
      //隐藏弹框
      hidePopup: function () {
        this.setData({
          flag: !this.data.flag
        })
      },
      //展示弹框
      showPopup:function (PhoneNumber,BusyStatus) {
        console.log('showPopup')
        console.log(PhoneNumber)
        console.log(BusyStatus)
        this.setData({
          flag: !this.data.flag,
          BusyStatus:BusyStatus,
          PhoneNumber:PhoneNumber
        })
      },
      changeBusyState:function(e){
        var that=this
        console.log('changeBusyState')
        console.log(e)
        console.log(e.currentTarget.dataset)
        console.log('that.data.PhoneNumber')
        const BusyStatus=e.currentTarget.dataset.busystate       
        webapi.ChangeGuideBusyState(
            that.data.PhoneNumber,
            BusyStatus,
            function(res){
                if(res.data.result=="ok"){
                    that.triggerEvent("success",{
                        BusyStatus:BusyStatus
                    })
                }
            }
        )
      },
      /*
      * 内部私有方法建议以下划线开头
      * triggerEvent 用于触发事件
      */
      _error () {
        //触发取消回调
        this.triggerEvent("error")
      },
      _success () {
        //触发成功回调
        this.triggerEvent("success");
      }
    }
  })

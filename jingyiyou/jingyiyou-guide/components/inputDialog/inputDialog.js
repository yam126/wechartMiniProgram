Component({

    properties: {
        title: {            // 属性名
            type: String,     // 类型（必填），目前接受的类型包括：String, Number, Boolean, Object, Array, null（表示任意类型）
            value: ''     // 属性初始值（可选），如果未指定则会根据类型选择一个
        },
        Placeholder:{
            type:String,
            value:''
        }
    },
    /**
     * 页面的初始数据
     */
    data: {
        completed: false,  // 弹窗控制
        deposit: 0,  // 存储用户输入的内容
    },
    methods: {
        // 获取用户输入的内容
        getUserInput(event) {
            const money = event.detail.value || event.detail.text
            this.setData({ deposit: money })
        },
        showGetPhoneNumberDialog(){
            this.setData({
                completed: true
            })
        },
        cancelSelected() {
            this.setData({
                completed: false
            })
        },

        // 确定按钮触发事件
        successSelected(event) {
            var that=this;
            //if (/^1[3456789]\d{9}$/.test(this.data.deposit)) {
                that.triggerEvent("successPhoneNumber",{
                    PhoneNumber:that.data.deposit
                })
                console.log('successSelected')
                that.setData({ completed: false })
            /*} else {
                wx.showToast({
                    title: '输入的不是手机号',
                    icon:'error',
                    duration: 1000
                  })
            }*/
        }
    }

})
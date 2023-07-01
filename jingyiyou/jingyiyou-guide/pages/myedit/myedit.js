// pages/myedit/myedit.js
const app = getApp()
const webapi=require('../../utils/webapi.js')
Page({

    /**
     * 页面的初始数据
     */
    data: {
        userNickName:'',
        userPhoneNumber:'',
        userFaceUrl: '../../images/noImage.png',
        userFaceFile: null,
        Intorduction:'',
        userVideoUrl: '../../images/novideo.png',
        userVideoFile: null,
        IDCard:'',
        Skills:[],
        SkillPoints:[],
        inputDialog:null,
        inputDialogType:''
    },
    showChoseUserFaceImage() {
        var that = this;
        wx.chooseMedia({
            count: 1,
            mediaType: ['image'],
            sourceType: ['album', 'camera'],
            maxDuration: 30,
            success(res) {
                console.log(res)
                if (res.tempFiles != null && res.tempFiles.length > 0) {
                    console.log("res.tempFiles[0].tempFilePath")
                    console.log(res.tempFiles[0].tempFilePath)
                    console.log("res.tempFiles[0].size")
                    console.log(res.tempFiles[0].size)
                    var tempUserFaceUrl=''
                    var tempFiles=res.tempFiles
                    that.uploadFile(
                        res.tempFiles[0].tempFilePath,
                        function(res){
                            tempUserFaceUrl=res
                            that.setData({
                                userFaceUrl: tempUserFaceUrl,
                                userFaceFile: tempFiles[0]
                            })                            
                        }
                    )
                }
            }
        })
    },
    uploadFile(filePath,callback){
        wx.uploadFile({
          filePath: filePath,
          name: 'file',
          url: app.com.uploadFileApi+'UploadFile',
          success:function(res){
              console.log(res)
              res.data=JSON.parse(res.data)
              console.log(res)
              if(res.data.result=='ok'){
                 var resultFilePath=app.com.host+res.data.obj
                 callback(resultFilePath)
              }
              else{
                  wx.showToast({
                      icon:'error',
                      title: '上传出错,原因['+res.data.message+']',
                  })
              }
          },
          fail:function(res){
            wx.showToast({
                icon:'error',
                title: '上传出错,原因['+res+']',
            })
          }
        })
    },
    showChoseVideo() {
        var that = this;
        if (that.userVideoFile != null) {
            var videoContext = wx.createVideoContext('myVideo')  //获取video的dom
            videoContext.requestFullScreen({ direction: 90 })
            videoContext.play() //视频播放
        }
        else {
            wx.chooseMedia({
                count: 1,
                mediaType: ['video'],
                sourceType: ['album', 'camera'],
                maxDuration: 30,               
                success(res) {
                    console.log(res)
                    if (res.tempFiles != null && res.tempFiles.length > 0) {
                        console.log("res.tempFiles[0].tempFilePath")
                        console.log(res.tempFiles[0].tempFilePath)
                        console.log("res.tempFiles[0].size")
                        console.log(res.tempFiles[0].size)
                        var tempUrl=''
                        var tempFiles=res.tempFiles
                        that.uploadFile(
                            res.tempFiles[0].tempFilePath,
                            function(res){
                                tempUrl=res
                                that.setData({
                                    userVideoUrl: tempUrl,
                                    userVideoFile: tempFiles[0]
                                })                            
                            }
                        )
                    }
                }
            })
        }
    },
    saveData(e){
        console.log('saveData')
        console.log(e)
        var that=this
        var guideId=app.com.guideInfo.GuideID
        var submitData=e.detail.value
        var checkEmpty=''
        var SkillPoints=this.data.SkillPoints
        var Skills=this.data.Skills
        var SkillPointsStr=this.arrayToString(SkillPoints)
        var SkillsStr=this.arrayToString(Skills)
        console.log('guideId='+guideId)
        if(submitData.nickName=='')
           checkEmpty+='昵称、'
        if(submitData.PhoneNumber=='')
           checkEmpty+='手机号、'
        if(submitData.IDCard=='')
           checkEmpty+='身份证号、'
        if(SkillPoints==null||SkillPoints.length<=0)
           checkEmpty+='可讲解景点、'
        if(checkEmpty!=''){
            checkEmpty=checkEmpty.substring(0,checkEmpty.length-1)
            wx.showToast({
                icon:'error',
                title: '['+checkEmpty+']不能为空'
            })
            return false
        }
        webapi.SaveGuide({
            GuidId:guideId,
            userFaceUrl:that.data.userFaceUrl,
            userVideoUrl:that.data.userVideoUrl,
            nickName:submitData.nickName,
            PhoneNumber:submitData.PhoneNumber,
            IDCard:submitData.IDCard,
            Intorduction:submitData.Intorduction,
            Skills:SkillsStr,
            SkillPointsStr:SkillPointsStr
        },
        function(res){
          if(res.data.result!='ok'){
            wx.showToast({
              icon:'error',
              title: '保存失败,原因['+res.data.message+']'
            })
          }
          else{
            wx.showToast({
              icon:'success',
              title: '保存成功'
            })
            app.com.guideInfo=res.data.obj
            wx.switchTab({
              url: '/pages/index/index',
            })           
          }
        })
    },
    arrayToString(arrayParm){
      var result=''
      if(arrayParm!=null&&typeof(arrayParm)!='undefined'&&arrayParm.length>0){
        for(var i=0;i<arrayParm.length;i++)
            result+=arrayParm[i].text+','
      }
      if(result!='')
         result=result.substring(0,result.length-1)
      return result
    },
    /**
     * 生命周期函数--监听页面加载
     */
    onLoad(options) {
        console.log('app.com.guideInfo')
        console.log(app.com.guideInfo)
        var that=this
        var guideInfo=app.com.guideInfo
        var HeadUrl=guideInfo.HeadUrl
        var SkillsStr=guideInfo.Skills
        var SkillPointsStr=guideInfo.SkillPoints
        var Skills=[]
        var SkillPoints=[]
        if(guideInfo.VideoShowUrl!=''&&guideInfo.VideoShowUrl!=null&&typeof(guideInfo.VideoShowUrl)!='undefined'){
            that.setData({
                userVideoUrl:guideInfo.VideoShowUrl
            })
        }
        if(SkillsStr!=''&&SkillsStr!=null&&typeof(SkillsStr)!='undefined'){
            var tempAry=SkillsStr.split(',')
            for(var i=0;i<tempAry.length;i++){
                Skills.push({
                    index:i,
                    text:tempAry[i]
                })
            }  
        }
        if(SkillPointsStr!=''&&SkillPointsStr!=null&&typeof(SkillPointsStr)!='undefined'){
            var tempAry=SkillPointsStr.split(',')
            for(var i=0;i<tempAry.length;i++){
                SkillPoints.push({
                    index:i,
                    text:tempAry[i]
                })
            }              
        }
        this.setData({
            userFaceUrl:HeadUrl,
            userNickName:guideInfo.Name,
            userPhoneNumber:guideInfo.PhoneNumber,
            IDCard:guideInfo.IDCard,
            Intorduction:guideInfo.Intorduction,
            Skills:Skills,
            SkillPoints:SkillPoints
        })
    },
    removeSkills(e){
        var Skills=this.data.Skills
        var index=e.currentTarget.dataset.index
        console.log('removeSkills')
        console.log('index='+index)
        Skills.splice(index,1)
        this.setData({
            Skills:Skills
        })
    },
    removeSkillPoints(e){
        var SkillPoints=this.data.SkillPoints
        var index=e.currentTarget.dataset.index
        console.log('removeSkillPoints')
        console.log('index='+index)
        SkillPoints.splice(index,1)
        this.setData({
            SkillPoints:SkillPoints
        })
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
    inputSkills(){
        this.showInputDialog('Skills')
    },
    inputSkillPoints(){
        this.showInputDialog('SkillPoints')
    },
    showInputDialog(inputDialogType){
        this.inputDialog=this.selectComponent("#inputPhoneNum")
        this.inputDialogType=inputDialogType
        this.inputDialog.showGetPhoneNumberDialog()
    },
    _successPhoneNumber(res){
        var that=this
        var inputValue=res.detail.PhoneNumber
        var Skills=that.data.Skills
        var SkillPoints=that.data.SkillPoints
        var SkillsIndex=(Skills==null||Skills.length<=0)?0:Skills.length+1
        var SkillPointsIndex=(SkillPointsIndex==null||SkillPointsIndex.length<=0)?0:SkillPointsIndex.length+1
        console.log('SkillsIndex='+SkillsIndex)
        console.log('SkillPointsIndex='+SkillPointsIndex)
        switch(that.inputDialogType){
            case 'Skills':
                Skills.push({
                    index:SkillsIndex,
                    text:inputValue
                })
                that.setData({
                    Skills:Skills
                })               
                break
            case 'SkillPoints':
                SkillPoints.push({
                    index:SkillPointsIndex,
                    text:inputValue
                })
                that.setData({
                    SkillPoints:SkillPoints
                })                
                break
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
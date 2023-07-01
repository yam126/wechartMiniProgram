const formatTime = date => {
    const year = date.getFullYear()
    const month = date.getMonth() + 1
    const day = date.getDate()
    const hour = date.getHours()
    const minute = date.getMinutes()
    const second = date.getSeconds()
  
    return [year, month, day].map(formatNumber).join('/') + ' ' + [hour, minute, second].map(formatNumber).join(':')
  }
  
  const formatNumber = n => {
    n = n.toString()
    return n[1] ? n : '0' + n
  }
  
  const convertJsonDateTime = function (jsonDateStr) {
    var date = new Date(parseInt(jsonDateStr.replace("/Date(", "").replace(")/", ""), 10))
    return date
  }
  
  /**
   * 格式化输出日期和时间
   * @param {日期时间格式} fmt
   * @param {要格式化的原始Date的object}  dateTimeObj
   */
  const formatDateTime = function (dateTimeObj,fmt) {
  
    var o = {
      "M+": dateTimeObj.getMonth() + 1, //月份
      "d+": dateTimeObj.getDate(), //日
      "h+": dateTimeObj.getHours(), //小时
      "m+": dateTimeObj.getMinutes(), //分
      "s+": dateTimeObj.getSeconds(), //秒
      "q+": Math.floor((dateTimeObj.getMonth() + 3) / 3), //季度
      "S": dateTimeObj.getMilliseconds() //毫秒
    };
  
    if (/(y+)/.test(fmt)) {
      fmt = fmt.replace(RegExp.$1, (dateTimeObj.getFullYear() + "").substr(4 - RegExp.$1.length));
    }
  
    for (var k in o) {
      if (new RegExp("(" + k + ")").test(fmt)) {
        fmt = fmt.replace(
          RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
      }
    }
  
    return fmt;
  
  
  }
  
  module.exports = {
    formatTime: formatTime,
    formatDateTime:formatDateTime,
    convertJsonDateTime: convertJsonDateTime
  }
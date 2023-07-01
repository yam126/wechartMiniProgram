Component({
    data: {
      selected: 0,
      color: "#7A7E83",
      selectedColor: "#FF0000",
      list: [{
        pagePath: "/pages/waitingorder/waitingorder",
        iconPath: "/images/waitingorder.jpg",
        selectedIconPath: "/images/waitingorder.jpg",
        text: "接单"
      }, {
        pagePath: "/pages/index/index",
        iconPath: "/images/my.jpg",
        selectedIconPath: "/images/my.jpg",
        text: "我的"
      }]
    },
    attached() {
    },
    methods: {
      switchTab(e) {
        const data = e.currentTarget.dataset
        const url = data.path
        wx.switchTab({url})
        this.setData({
          selected: data.index
        })
      }
    }
  })
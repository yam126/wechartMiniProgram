/* $Id : common.js 4865 2007-01-31 14:04:10Z paulgao $ */
var loadscroll = 0;
var binded = false;
function get_asynclist(url, src) {
    $('#J_ItemList').reset();
    $('#J_ItemList').more({ 'address': url, 'spinner_code': '<div style="text-align:center; margin:10px;"><img src="' + src + '" /></div>' });


    $(window).scroll(function () {
        if ($(window).scrollTop() > ($(document).height() - $(window).height() - 200)
            && $(window).scrollTop() <= ($(document).height() - $(window).height())) {

            if ($(window).scrollTop() - loadscroll > 200) {
                loadscroll = $(window).scrollTop();
                //alert('loading');
                $('.get_more').click();

            }

        }
    });



}

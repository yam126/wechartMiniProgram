
(function ($) {
    var target = null;
    var template_more = null;
    var lock = false;
    var variables = {
        'last': 0
    }
    var settings = {
        'amount': '10',
        'address': '../../search/AGoods',
        'format': 'json',
        'template_more': '.single_item',
        'trigger': '.get_more',
        'scroll': 'false',
        'offset': '100',
        'spinner_code': ''
    }
    $.fn.reset = function () {
        variables.last = 0;
    }
    var methods = {
        init: function (options) {
            return this.each(function () {

                if (options) {
                    $.extend(settings, options);
                }
                //debugger;
                //template_more = $(this).children(settings.template_more).wrap('<div/>').parent();
                //template_more.css('display', 'none')
                if ($(this).children(".more_loader_spinner").length == 0)
                    $(this).append('<div class="more_loader_spinner">' + settings.spinner_code + '</div>');
                $(this).children(settings.template_more).remove();
                target = $(this);
                if (settings.scroll == 'false') {
                    $(this).find(settings.trigger).unbind('click.more');
                    $(this).find(settings.trigger).bind('click.more', methods.get_data);
                    $(this).more('get_data');
                }
                else {
                    if ($(this).height() <= $(this).attr('scrollHeight')) {
                        target.more('get_data', settings.amount * 2);
                    }
                    $(this).bind('scroll.more', methods.check_scroll);
                }
            })
        },
        check_scroll: function () {
            if ((target.scrollTop() + target.height() + parseInt(settings.offset)) >= target.attr('scrollHeight') && lock == false) {
                target.more('get_data');
            }
        },
        debug: function () {
            var debug_string = '';
            $.each(variables, function (k, v) {
                debug_string += k + ' : ' + v + '\n';
            })
            alert(debug_string);
        },
        remove: function () {

            target.children(settings.trigger).unbind('.more');
            target.unbind('.more')
            target.children(settings.trigger).remove();
        },
        add_elements: function (data) {
            //alert('adding elements')

            var root = target
            //   alert(root.attr('id'))
            var counter = 0;
            if (data) {
                // debugger;
                $(data).each(function () {
                    counter++

                    var t = template_more
                    //$.each(this, function(GoodID, value){                          
                    //    if(t.find('.'+key)) t.find('.'+key).html(value);
                    //})
                    var html = template('temp', this);                    
                    //if (t.find('#single_' + this.GoodID).length>0) { t.find('#single_' + this.GoodID).html(html); }
                    //else { t.append(html); };
                    //$(html).insertBefore(".get_more")

                    //$("#tbox").html(html);
                    //t.attr('id', 'more_element_'+ (variables.last++))
                    if (settings.scroll == 'true') {
                        //    root.append(t.html())                        
                        root.children('.more_loader_spinner').before(html)
                    } else {
                        //    alert('...')
                        //debugger;
                        //root.children(settings.trigger).before(html)
                        root.children('.more_loader_spinner').before(html)
                    }
                    //variables.last++;
                    //debugger;
                    root.children(settings.template_more + ':last').attr('id', 'more_element_' + ((variables.last++) + 1))

                })


            }
            else methods.remove()
            target.children('.more_loader_spinner').css('display', 'none');
            if (counter < settings.amount) methods.remove()



        },
        get_data: function () {
            // alert('getting data')
            var ile;
            lock = true;
            target.children(".more_loader_spinner").css('display', 'block');
            $(settings.trigger).css('display', 'none');
            if (typeof (arguments[0]) == 'number') ile = arguments[0];
            else {
                ile = settings.amount;
            }
            var mlast = variables.last;
            
            $.post(settings.address, {
                last: mlast,
                amount: ile
            }, function (data) {
                $(settings.trigger).css('display', 'block')
                methods.add_elements(data)
                lock = false;

                $('.list li img').each(function () {
                    $(this).height($(this).width());
                });
                //alert($(document).scrollTop());
                
            }, settings.format)

        }
    };
    $.fn.more = function (method) {
        if (methods[method])
            return methods[method].apply(this, Array.prototype.slice.call(arguments, 1));
        else if (typeof method == 'object' || !method)
            return methods.init.apply(this, arguments);
        else $.error('Method ' + method + ' does not exist!');

    }
    $.fn.save_data = function () {
        var t = $('#J_ItemList').data('t');
        localStorage.setItem('target'+t, target);
        localStorage.setItem('template_more' + t, template_more);
        localStorage.setItem('lock' + t, lock);
        localStorage.setItem('variables.last' + t, variables.last);
        localStorage.setItem('settings' + t, JSON.stringify(settings));
    }
    $.fn.reload_data = function () {
        var t = $('#J_ItemList').data('t');
        if (localStorage.getItem('target' + t)) { target = $(this); }
        if (localStorage.getItem('template_more' + t)) { template_more = localStorage.getItem('template_more' + t); }
        if (localStorage.getItem('lock' + t)) { lock = localStorage.getItem('lock' + t); }
        if (localStorage.getItem('variables.last' + t)) { variables.last = localStorage.getItem('variables.last' + t); }
        if (localStorage.getItem('settings' + t)) { settings = JSON.parse(localStorage.getItem('settings' + t)); }
        $(this).find('.get_more').click(function () {
            methods.get_data();
        });
    }
    $.fn.remove_data = function () {
        var t = $('#J_ItemList').data('t');
        if (localStorage.getItem('target' + t)) { localStorage.removeItem('target' + t); }
        if (localStorage.getItem('template_more' + t)) { localStorage.removeItem('template_more' + t); }
        if (localStorage.getItem('lock' + t)) { localStorage.removeItem('lock' + t); }
        if (localStorage.getItem('variables.last' + t)) { localStorage.removeItem('variables.last' + t); }
        if (localStorage.getItem('settings' + t)) { localStorage.removeItem('settings' + t); }
        
    }
    function supports_html5_storage() {
        try {
            return 'localStorage' in window && window['localStorage'] !== null;
        } catch (e) {
            return false;
        }
    }
})(jQuery)
/**
 * Author: Venkat.P
 * */

function onFunction() {
    let removeelem = document.getElementById("offlinemod");
    removeelem.classList.remove("show");
    removeelem.innerHTML = "";

    let elem = document.getElementById("online");
    elem.innerHTML = "Back to online"
    elem.classList.add("show");
}

function offFunction() {
    let removeelem1 = document.getElementById("online");
    removeelem1.classList.remove("show");
    removeelem1.innerHTML = "";

    let elem1 = document.getElementById("offlinemod");
    elem1.innerHTML = "No internet connection"
    elem1.classList.add("show");
}

//function showStickyToast(type, message) {
//    StickyToast(type, message, false);
//}
//function showStickyToast(type, message, IsParmenent) {
//    var val;
//    var time;
//    if (type == true)
//        val = 'success';
//    else
//        val = 'error';
//    $().toastmessage('showToast', {
//        stayTime: 9600,
//        text: message,
//        sticky: IsParmenent,
//        position: 'bottom-right',
//        type: val,
//        closeText: '',
//        close: function () {
//        },

//    });

//}




//$(window).on('load', function () {
//    $('inv-preloader').show();
//});
$(document).ready(function () {
    $('loader').hide();
    if (location.href.toLowerCase().indexOf('locationtree.aspx') == -1) { //Exception for Location Page. Because that page works on document positions. positions will differ if menu bar is in open state
        MenuState($("#sidebar-toggle"), 0, 0);
    }

    $("#sidebar-toggle").click(function () {
        MenuState($("#sidebar-toggle"), 1, 1);
    });

    //adding Active class to clicked Menu Item on page Load;

    var MenuItem1 = $('.spanMenuID').html();
    if (MenuItem1 != "") {
        if ($('.' + MenuItem1).parent().parent().attr('class').indexOf('main-sub-menu') > -1) {
            $('.' + MenuItem1).parent().parent().css('display', 'block');
        }
        else {
            $('.' + MenuItem1).parent().parent().css('display', 'block');
            $('.' + MenuItem1).parent().parent().parent().parent().css('display', 'block');
        }
    }

});
$(function () {

    $('.accordian .icon_block:eq(1), .icon_block:eq(2), .icon_block:eq(3), .icon_block:eq(4), .icon_block:eq(5), .icon_block:eq(6), .icon_block:eq(8)').on('click', function () { });

});

function MenuState($cnt, SetCookie, Toggle) {

    var MenuState = $.cookie("MenuState");

    if (MenuState == '' || MenuState == undefined) {
        $.cookie('MenuState', '1');
        Toggle = 1;
    }
    else if (MenuState == "0") {
        if (SetCookie == 1)
            $.cookie('MenuState', '1');
    }
    else if (MenuState == "1") {
        Toggle = 1;
        if (SetCookie == 1)
            $.cookie('MenuState', '0');
    }


    if (Toggle == 1) {
        ToggleMenus($cnt);

    }


}


//coockies classes
//$('.content').toggleClass('active-acordian');
//$('.loader-block').toggleClass('addloader__');
//$('.Header').toggleClass('active-acordian-menu');
//$('.ftoor').toggleClass('acfooter');
function ToggleMenus($cnt) {
    $cnt.toggleClass('humburge');
    var toggle_el = $cnt.data("toggle");
    //$(toggle_el).toggleClass("open-sidebar");
    //$('.swipe-area').toggleClass('accordian');
    //$('.swipe-area').toggleClass('hoverIsActive');
    // $('.hamburger--arrowturn').toggleClass('is-active');
    //$('.ftoor').toggleClass('acfooter');
    // $('.content').toggleClass('active-acordian');
    //$('.loader-block').toggleClass('addloader__');
    //$('.Header').toggleClass('active-acordian-menu');
    // $('.scroll').toggleClass('scrollDexced');
    // $('.scrollable div').toggleClass('frezScroll');
}

$('.check-box').append('<span class="checkmark"></span>');

$('.chkIsActive, .checkbox').find('input[type="checkbox"]').each(function () {
    var $ids = $(this).attr('id');
    $(this).next().attr('for', $ids);
});

$('.md-radio').find('input[type="radio"]').each(function () {
    var $ids__ = $(this).attr('id');
    $(this).next().attr('for', $ids__);
});

$('.checkbox .NoPrint').append('<label for=""></label>');

$('.NoPrint').find('input[type="checkbox"]').each(function () {

    var $idds = $(this).attr('id');
    console.log($idds);
    $(this).next().attr('for', $idds);
});

$('scrollTop').on('click', function (event) {

    var target = $($(this).attr('href'));

    if (target.length) {
        event.preventDefault();
        $('html, body').animate({
            scrollTop: target.offset().top
        }, 500);
    }

});


$(function () {

    var onSampleResized = function (e) {
        var columns = $(e.currentTarget).find("td");
        var rows = $(e.currentTarget).find("tr");
        var Cloumnsize;
        var rowsize;
        columns.each(function () {
            Cloumnsize += $(this).attr('id') + "" + $(this).width() + "" + $(this).height() + ";";
        });
        rows.each(function () {
            rowsize += $(this).attr('id') + "" + $(this).width() + "" + $(this).height() + ";";
        });
        document.getElementById("hf_columndata").value = Cloumnsize;
        document.getElementById("hf_rowdata").value = rowsize;
    };


});




$(function () {
    // logo added dynamically logo 


    $('.dropdown').find('.main-sub-menu:visible').each(function () {
        $(this).prev('.acceed').children('.rotator').addClass('isRotate');
    });

    var clientLogo = $('#Image1').attr('src');
    document.documentElement.style.setProperty('--client-logo', 'url(' + clientLogo + ')');

    // input validations
    var appededline = function () {
        $('.flex input[type="text"], .flex input[type="password"],.flex input[type="number"],.flex input[type="date"],.flex select,.flex textarea').siblings('label').before('<div class="mdd-select-underline"></div>');

    }

    setTimeout(appededline, 1000);


    var explode = function () {

        $('.flex').on('keyup keypress keydown ', 'input[type="text"], input[type="password"], input[type="number"], input[type="date"], select, textarea', function () {

            let gettenval = $(this).val();

            if (gettenval == 0) {
                $(this).siblings('.mdd-select-underline').toggleClass('activ');
            }


            if (!gettenval == 0) {
                $(this).siblings('.mdd-select-underline').addClass('activ');
            }
        });

        $('.flex').on('change blur focusout propertychange paste ', 'input[type="text"], input[type="password"], input[type="number"], input[type="date"], select, textarea', function () {

            let gettenval = $(this).val();

            if (!gettenval == 0) {
                $(this).siblings('.mdd-select-underline').removeClass('activ');
            }
        });



        $('.flex').on('change blur keyup keypress focusout keydown propertychange paste', 'input[type="text"], input[type="password"], input[type="number"], input[type="date"], select, textarea', function () {

            let gettenval = $(this).val();
            if (gettenval == 0) {
                $(this).siblings('.errorMsg:visible, .errormsg:visible').siblings('.mdd-select-underline').prev('input').addClass('errorInfo');
                $(this).siblings('.errorMsg:visible, .errormsg:visible').siblings('.mdd-select-underline').addClass('afterError');
            }
            else if (!gettenval == 0) {
                $(this).siblings('.errorMsg:visible, .errormsg:visible').siblings('.mdd-select-underline').prev('input').removeClass('errorInfo');
                $(this).siblings('.errorMsg:visible, .errormsg:visible').siblings('.mdd-select-underline').removeClass('afterError');

            }

            $(this).on('focus', function () {
                $(this).not('.hasDatepicker').siblings('.errorMsg:visible, .errormsg:visible').siblings('.mdd-select-underline').removeClass('afterError');
            });

        });
    };
    setTimeout(explode, 1100);


    //$('textarea').autoResize();

});

// icons - suggestion
$(function () {
    var explodeeeee = function () {
        $('table').find('.material-icons').each(function () {
            var innerText = $(this).text();
            if (innerText === "mode_edit" | innerText === "edit") {
                $(this).after('<em class="sugg-tooltis">Edit</em>');
            }

            else if (innerText === "delete") {
                $(this).after('<em class="sugg-tooltis" style="left: 32px;">Delete</em>');
            }

            else if (innerText === "content_copy") {
                $(this).after('<em class="sugg-tooltis">Copy</em>');
            }

            else if (innerText === "print") {
                $(this).after('<em class="sugg-tooltis">Print</em>');
            }

            else if (innerText === "mode_delete") {
                $(this).after('<em class="sugg-tooltis">Delete</em>');
            }

            else if (innerText === "touch_app") {
                $(this).after('<em class="sugg-tooltis">Pick</em>');
            }

            else if (innerText === "launch") {
                $(this).after('<em class="sugg-tooltis" style="left: -65px;">Launch</em>');
            }

            else if (innerText === "settings") {
                $(this).after('<em class="sugg-tooltis" style="left: -65px;">Change</em>');
            }

            else if (innerText === "highlight_off") {
                $(this).after('<em class="sugg-tooltis" style="left: unset;right:-55px;">Close</em>');
            }

        });

    }

    setTimeout(explodeeeee, 1000);

});


(function (a) { a.fn.autoResize = function (j) { var b = a.extend({ onResize: function () { }, animate: true, animateDuration: 150, animateCallback: function () { }, extraSpace: 20, limit: 1000 }, j); this.filter('textarea').each(function () { var c = a(this).css({ resize: 'none', 'overflow-y': 'hidden' }), k = c.height(), f = (function () { var l = ['height', 'width', 'lineHeight', 'textDecoration', 'letterSpacing'], h = {}; a.each(l, function (d, e) { h[e] = c.css(e) }); return c.clone().removeAttr('id').removeAttr('name').css({ position: 'absolute', top: 0, left: -9999 }).css(h).attr('tabIndex', '-1').insertBefore(c) })(), i = null, g = function () { f.height(0).val(a(this).val()).scrollTop(10000); var d = Math.max(f.scrollTop(), k) + b.extraSpace, e = a(this).add(f); if (i === d) { return } i = d; if (d >= b.limit) { a(this).css('overflow-y', ''); return } b.onResize.call(this); b.animate && c.css('display') === 'block' ? e.stop().animate({ height: d }, b.animateDuration, b.animateCallback) : e.height(d) }; c.unbind('.dynSiz').bind('keyup.dynSiz', g).bind('keydown.dynSiz', g).bind('change.dynSiz', g) }); return this } })(jQuery);



$(() => {

    $('#MenuContainer').find('.menu-level-one').each(function () {
        let dropDownHeight = $(this).height();
        $(this).attr('data-menu-heigt', dropDownHeight);
        if (dropDownHeight > 175) {
            // $(this).attr('data-heigt-type', dropDownHeight);
        }
    });

    $('.accordian').append('<div class="updown"><div class="uparrow"><i class="material-icons">keyboard_arrow_up</i ></div><div class="downarrow"><i class="material-icons">keyboard_arrow_down</i ></div></div>');
    var scrolled = 0;
    $(".menu-level-one").on('click', '.uparrow', function () {

        scrolled = scrolled - 30;
        $('.menu-level-one').animate({
            scrollTop: scrolled
        });
        return false;

    });

    $(".menu-level-one").on('click', '.downarrow', function () {
        scrolled = scrolled + 30;
        $('.menu-level-one').animate({
            scrollTop: scrolled
        });
        return false;
    });

});

var textarea = document.querySelector('textarea');


function autosize() {
    var el = this;
    setTimeout(function () {
        el.style.cssText = 'height:auto; padding:0';
        // for box-sizing other than "content-box" use:
        // el.style.cssText = '-moz-box-sizing:content-box';
        el.style.cssText = 'height:' + el.scrollHeight + 'px';
    }, 0);
}

//new script 

$(document).ready(function () {
    $('button preloader').append('<svg version="1.1" id="loader-1" xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" x="0px" y="0px" width="40px" height="40px" viewBox="0 0 50 50" style="enable-background:new 0 0 50 50;" xml:space="preserve"><path fill="#fff" d="M43.935,25.145c0-10.318-8.364-18.683-18.683-18.683c-10.318,0-18.683,8.365-18.683,18.683h4.068c0-8.071,6.543-14.615,14.615-14.615c8.072,0,14.615,6.543,14.615,14.615H43.935z" transform="rotate(229.588 25 25)"><animateTransform attributeType="xml" attributeName="transform" type="rotate" from="0 25 25" to="360 25 25" dur="0.3s" repeatCount="indefinite"></animateTransform></path></svg>&nbsp;&nbsp;Loading...');
    $(".iconiciy").click(function () {
        $('.card22').toggleClass('addTransform');
        $(this).parent().toggleClass('adjustble');
    });

    $('sort').siblings('dropdown').before('<filter class="filter"><i class="material-icons">settings_input_component</i></filter>');
    $('dropdown').append('<span class="closeBlock" ng-click="closeColumnSearch($event)"><i class="material-icons">highlight_off</i></span>')
    $('.filter').click(function () {
        $('.divclass').hide();
        $(this).siblings('.divclass').toggle();
        $(this).siblings('.divclass').find('input').click();
    });
    $('body').on('click', '.ui-autocomplete .ui-corner-all', function () {
        $('.filter').siblings('.divclass').find('input').change();
    });

    $('dropdown').on('click', '.closeBlock', function () {
        $(this).closest('.divclass').toggle();
        $(this).closest('.divclass input').val("");
    });

});

function createCookie(name, value, days) {
    if (days) {
        var date = new Date();
        date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
        var expires = "; expires=" + date.toGMTString();
    }
    else var expires = "";
    document.cookie = name + "=" + value + expires + "; path=/";
}

function readCookie(name) {
    var nameEQ = name + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') c = c.substring(1, c.length);
        if (c.indexOf(nameEQ) == 0) return c.substring(nameEQ.length, c.length);
    }
    return null;
}

function eraseCookie(name) {
    createCookie(name, "", -1);
}


$('.colorInvert').on('click', function () {
    $('html').toggleClass('invertColor');

});


function MenuToggleEvents() {
    $(".acceed").on('click', function () {
        $('.intarow').removeClass('anintarow');
        $(".accordian ul .main-sub-menu").slideUp(1);
        if (!$(this).next().is(":visible")) {
            $(this).next().slideDown(1).parent();
            $(this).children('.accordian .intarow').addClass('anintarow');
        } else {
            $('.intarow').removeClass('anintarow');
        }
    });


    $(".acceed_next").on('click', function () {
        $('.intarow').removeClass('anintarow');
        $(".accordian ul .level-2").slideUp(1);
        if (!$(this).next().is(":visible")) {
            // $(this).next().addClass('isvisibledNow');
            $(this).children('.acceed_next .intarow').addClass('anintarow');
            $(this).next().slideDown(1).parent();
        } else {
            $('.intarow').removeClass('anintarow');
        }
    });
}


MenuToggleEvents();

$('.chkIsActive, .checkbox').find('input[type="checkbox"]').each(function () {
    var $ids = $(this).attr('id');
    $(this).next().attr('for', $ids);
});

$('.md-radio').find('input[type="radio"]').each(function () {
    var $ids__ = $(this).attr('id');
    $(this).next().attr('for', $ids__);
});

$('.checkbox .NoPrint').append('<label for=""></label>');

$('.NoPrint').find('input[type="checkbox"]').each(function () {

    var $idds = $(this).attr('id');
    console.log($idds);
    $(this).next().attr('for', $idds);
});


$(function () {

    $('body InputBlock[required]').find('input').after('<required></required>');
    $('body InputBlock[required]').find('input ~ required').after('<message>this field is required</message>');
    $('body InputBlock').find('input').siblings('label').after('<underline></underline>');
    $('model-block').append('<close close><i class="material-icons">shuffle</i></close>');
    $('body InputBlock').find('input').blur(function () {
        let inputData = $(this).val();
        if (inputData == null || inputData == undefined || inputData == 0) {
            $(this).next('required').addClass('invalid');
        }
    });


    $('#delete').on('click', function () {
        $('#delete-model').fadeIn(100);
    });

    $('[close]').on('click', function () {
        $(this).closest('model').fadeOut(100);
    });

    //$('.hasTimeEntry').on('focus load', function () {

    //    let currentVal = $(this).val();
    //    if (currentVal !== null || currentVal !== undefined || currentVal !== "") {
    //        $(this).closest('span').addClass('labelActive');
    //    }
    //});

    //$('.hasTimeEntry').on('focusout blur load', function () {
    //    debugger;
    //    let currentVal = $(this).val();
    //    if (currentVal !== null || currentVal !== undefined || currentVal !== "") {
    //        $(this).closest('span').addClass('labelActive');
    //    }

    //    if (currentVal == null || currentVal == undefined || currentVal == "") {
    //        $(this).closest('span').removeClass('labelActive');
    //    }
    //});
});


//function init() {
//    const inputs = [].slice.call(document.querySelectorAll('.controls input')).forEach(input => input.addEventListener('change', handleUpdate));
//    function handleUpdate(e) {
//        let colorCode = document.getElementById('sideNav-bg').value;
//        document.documentElement.style.setProperty('--sideNav-bg', colorCode);
//        createCookie("pree", colorCode, 1000);
//    }
//    var coockiethemecolor = $.cookie('pree');
//    if (coockiethemecolor == undefined) {
//        coockiethemecolor = '';
//    }
//    document.documentElement.style.setProperty('--sideNav-bg', coockiethemecolor);
//}

//window.onload = init();


const inputs = [].slice.call(document.querySelectorAll('.controls input')).forEach(input => input.addEventListener('change', handleUpdate));

function handleUpdate(e) {
    var favColor = document.getElementById('sideNav-bg').value;
    document.documentElement.style.setProperty('--sideNav-bg', favColor);
    localStorage.setItem('color', favColor);

    console.log(favColor)
}



document.addEventListener('DOMContentLoaded', function GetFavColor() {
    var favColor = document.body.style.backgroundColor;
    var color = localStorage.getItem('color');

    if (color === '') {
        document.documentElement.style.setProperty('--sideNav-bg', favColor);
    } else {
        document.documentElement.style.setProperty('--sideNav-bg', color);
    }
});










/*
 *Experimental native - webcomponents
 *Author: venkat
 *Date: 08/03/2019
 * using ES6/7/8
*/

class BreadCrumb extends HTMLElement {
    constructor() {
        super();

        const shadowRoot = this.attachShadow({ mode: 'open' });
        const parent = this.getAttribute('parent');
        const child = this.getAttribute('child');
        const subchild = this.getAttribute('subchild');
        shadowRoot.innerHTML = `
            <style>
                @import url( '../Content/app.css' );
                .istoggleCon{display:none !important;}.istoggleConnull{display:none !important;}
            </style>
            <div class="module_yellow">
                <div class="ModuleHeader">
                    <a href="../Default.aspx">Home</a> 
                    <i class="material-icons istoggleCon${parent}">arrow_right</i> 
                    <a href="#" class="istoggleCon${parent}">${parent}</a> 
                    <i class="material-icons istoggleCon${child}">arrow_right</i>
                    <span class="breadcrumbd istoggleCon${child}" style="display: inline-block;">${child}</span>
                    <i class="material-icons istoggleCon${subchild}">arrow_right</i>
                    <span class="breadcrumbd istoggleCon${subchild}" style="display: inline-block;">${subchild}</span>
                </div>
            </div>
        `;
    }
}
//<bread-crumb parent="EAM" child="CNTInbound"></bread-crumb> example bread crumb component
customElements.define('bread-crumb', BreadCrumb);


class Button extends HTMLElement {
    constructor() {
        super();
        const shadowRoot = this.attachShadow({ mode: 'open' });
        const color = this.getAttribute('color');

        shadowRoot.innerHTML = ` 
            <style>
                @import url( '../Content/app.css' );   
            </style>
            <button type="button" class=${(function () {

                if (color == "primary") {
                    return '"btn btn-primary"'
                }
            })()}><slot></slot></button>
        `;
    }

    connectedCallback() {

    }
}
//<my-button color="primary">Search <i class="material-icons">search</i></my-button> example button
customElements.define('my-button', Button);

class Icon extends HTMLElement {
    constructor() {
        super()
        const shadowRoot = this.attachShadow({ mode: 'open' });
        const help = this.getAttribute('help');
        const is = this.getAttribute('is');
        const position = this.getAttribute('position');
        const setColor = this.getAttribute('color');
        const sub = this.getAttribute('verticle')
        shadowRoot.innerHTML = `
            <style>
                @import url( '../Content/Site.css' );   
                .material-icons{
                    font-size: 19px;
                     color:#0e0e0e !important;
                }
                help{
                    background: #000;
                    color: #fff;
                    position: absolute;
                   
                    width: max-content;
                    padding: 5px 12px;
                    font-style: normal;
                    box-shadow: var(--z1);
                    border-radius: 30px;
                    font-size: 11px;
                    display: inline-block;  
                    opacity:0;
                    visibility:hidden;
                }

                    .left{
                        right: 0;
                        top: 0;
                        transform: translateX(-50%);
                    }

                    .right{
                        top: 0;
                        right: -6px;
                        transform: translate(100%);
                    }

                    .bottom{
                        bottom: 0;
                        transform: translate(-50%, 100%);
                    }

                    .top{
                        top: 0;
                        transform: translate(-50%, -100%);
                    }

                    .isVisible:hover help{
                        opacity:1;
                        visibility:visible;
                     }

                    .isVisible{
                        display: -webkit-inline-box; 
                        position:relative}
.null{
display:none;
}

.subright{
    margin-right: 5px;
    font-size: 19px;
    transform: rotate(0.03deg);
}

            </style>
            <div class="isVisible">
                <i class="material-icons ${sub}right" style="color: ${setColor} !important; vertical-align:${sub}">${is}</i>
                <help class="${position} ${help}" >${help}</help>
            </div>
        `;
    }
}

customElements.define('inv-icon', Icon);




class FontAwesome extends HTMLElement {
    constructor() {
        super()
        const shadowRoot = this.attachShadow({ mode: 'open' });

        const is = this.getAttribute('is');

        const setColor = this.getAttribute('color');
        const sub = this.getAttribute('verticle');
        shadowRoot.innerHTML = `
            <style>
                @import url( 'https://use.fontawesome.com/releases/v5.8.1/css/all.css' );   
             
                   
.null{
display:none;
}

.subright{
    margin-right: 5px;
    font-size: 22px;
    transform: rotate(0.03deg);
}

            </style>
            <div class="isVisible">
             
               <i class="fas ${is} ${sub}right"></i>
            </div>
        `;
    }
}

customElements.define('i-con', FontAwesome);


class Ripple extends HTMLElement {
    constructor() {
        super()
        const shadowRoot = this.attachShadow({ mode: 'open' });
        shadowRoot.innerHTML = `
<style>
.rippleeffect{
    display: inline-block;
    width: 30px;
    height: 30px;
    position: absolute;
    background: #ebebeb;
    border-radius: 100%;
}
</style>
            <div class="rippleeffect"></div>
        `;
    }
}

customElements.define('ripple-effect', Ripple);




class ThTooltip extends HTMLElement {
    constructor() {
        super()
        const shadowRoot = this.attachShadow({ mode: 'open' });
        const help = this.getAttribute('help');
        shadowRoot.innerHTML = `
            <style>
                help{
                  background: #000;
                  color: #fff;
                  position: absolute;
                  width: max-content;
                  padding: 5px 12px;
                  font-style: normal;
                  box-shadow: var(--z1);
                  border-radius: 30px;
                  font-size: 11px;
                  display: inline-block;      
                  top: -30px;
                  z-index:999999999;
                }

                help::after {
                  content: " ";
                  position: absolute;
                  top: 100%; /* At the bottom of the tooltip */
                  left: 50%;
                  margin-left: -5px;
                  border-width: 5px;
                  border-style: solid;
                  border-color: black transparent transparent transparent;
                }

                .relative{
                   position:relative}
                  .hidden{
                  display:none;
                }

                .is-need-tooltip:hover .hidden{
                    display:block;
                }
            </style>
            <div class="relative is-need-tooltip">
                <help class="istoggle hidden">${help}</help>
                <slot></slot>
            </div>
        `;
    }

    connectedCallback() {
        //this.shadowRoot.querySelector('.is-need-tooltip').addEventListener('click', function (event) {
        //    debugger;
        //    this.querySelector('.istoggle').classList.toggle('hidden');
        //});
    }
}
// <th-help help="Material quantity">M.Qty</th-help>
customElements.define('th-help', ThTooltip);

class Preloader extends HTMLElement {
    constructor() {
        super()
        const shadowRoot = this.attachShadow({ mode: 'open' });
        const hide = this.getAttribute('is');
        shadowRoot.innerHTML = `
            <style>
                [loader] {
                    width: 60px;
                    transform: rotate(180deg);
                }
                loader {
                    display: block;
                    height: 100%;
                    width: 100%;
                    background: #000000ad;
                    position: fixed;
                    left: 0;
                    right: 0;
                    bottom: 0;
                    top: 0px;
                    z-index: 9999999;
                    left: 0px;
                    border-radius: 0px;
                }

                    loader img {
                        width: 60px;
                        transform: rotate(180deg);
                    }

                    loader div {
                        position: absolute;
                        left: 0;
                        right: 0;
                        bottom: 0;
                        top: 0;
                        display: flex;
                        justify-content: center;
                        align-items: center;
                    }

                </style>
            <loader><div><img src="data:image/svg+xml;base64,DQo8c3ZnIHhtbG5zPSJodHRwOi8vd3d3LnczLm9yZy8yMDAwL3N2ZyIgdmlld0JveD0iMCAwIDEwMCAxMDAiIHZlcnNpb249IjEuMSIgaWQ9ImVsX3E5VEpoNkQ5TSIgPg0KICA8c3R5bGU+DQogICAgQC13ZWJraXQta2V5ZnJhbWVzIGVsX1I5VVc0LVA3YWhfY0ZUdEwwOWItX0FuaW1hdGlvbnswJXstd2Via2l0LXRyYW5zZm9ybTogdHJhbnNsYXRlKDUwLjYwOTM3MzgwNzkwNzEwNHB4LA0KICAgIDUwLjEyMzEzNjA4ODI1MjA3cHgpIHJvdGF0ZSgwZGVnKSB0cmFuc2xhdGUoLTUwLjYwOTM3MzgwNzkwNzEwNHB4LCAtNTAuMTIzMTM2MDg4MjUyMDdweCk7dHJhbnNmb3JtOg0KICAgIHRyYW5zbGF0ZSg1MC42MDkzNzM4MDc5MDcxMDRweCwgNTAuMTIzMTM2MDg4MjUyMDdweCkgcm90YXRlKDBkZWcpIHRyYW5zbGF0ZSgtNTAuNjA5MzczODA3OTA3MTA0cHgsDQogICAgLTUwLjEyMzEzNjA4ODI1MjA3cHgpO30xMDAley13ZWJraXQtdHJhbnNmb3JtOiB0cmFuc2xhdGUoNTAuNjA5MzczODA3OTA3MTA0cHgsIDUwLjEyMzEzNjA4ODI1MjA3cHgpIHJvdGF0ZSgzNjBkZWcpDQogICAgdHJhbnNsYXRlKC01MC42MDkzNzM4MDc5MDcxMDRweCwgLTUwLjEyMzEzNjA4ODI1MjA3cHgpO3RyYW5zZm9ybTogdHJhbnNsYXRlKDUwLjYwOTM3MzgwNzkwNzEwNHB4LA0KICAgIDUwLjEyMzEzNjA4ODI1MjA3cHgpIHJvdGF0ZSgzNjBkZWcpIHRyYW5zbGF0ZSgtNTAuNjA5MzczODA3OTA3MTA0cHgsIC01MC4xMjMxMzYwODgyNTIwN3B4KTt9fUBrZXlmcmFtZXMNCiAgICBlbF9SOVVXNC1QN2FoX2NGVHRMMDliLV9BbmltYXRpb257MCV7LXdlYmtpdC10cmFuc2Zvcm06IHRyYW5zbGF0ZSg1MC42MDkzNzM4MDc5MDcxMDRweCwgNTAuMTIzMTM2MDg4MjUyMDdweCkNCiAgICByb3RhdGUoMGRlZykgdHJhbnNsYXRlKC01MC42MDkzNzM4MDc5MDcxMDRweCwgLTUwLjEyMzEzNjA4ODI1MjA3cHgpO3RyYW5zZm9ybTogdHJhbnNsYXRlKDUwLjYwOTM3MzgwNzkwNzEwNHB4LA0KICAgIDUwLjEyMzEzNjA4ODI1MjA3cHgpIHJvdGF0ZSgwZGVnKSB0cmFuc2xhdGUoLTUwLjYwOTM3MzgwNzkwNzEwNHB4LCAtNTAuMTIzMTM2MDg4MjUyMDdweCk7fTEwMCV7LXdlYmtpdC10cmFuc2Zvcm06DQogICAgdHJhbnNsYXRlKDUwLjYwOTM3MzgwNzkwNzEwNHB4LCA1MC4xMjMxMzYwODgyNTIwN3B4KSByb3RhdGUoMzYwZGVnKSB0cmFuc2xhdGUoLTUwLjYwOTM3MzgwNzkwNzEwNHB4LA0KICAgIC01MC4xMjMxMzYwODgyNTIwN3B4KTt0cmFuc2Zvcm06IHRyYW5zbGF0ZSg1MC42MDkzNzM4MDc5MDcxMDRweCwgNTAuMTIzMTM2MDg4MjUyMDdweCkgcm90YXRlKDM2MGRlZykNCiAgICB0cmFuc2xhdGUoLTUwLjYwOTM3MzgwNzkwNzEwNHB4LCAtNTAuMTIzMTM2MDg4MjUyMDdweCk7fX0jZWxfcTlUSmg2RDlNICp7LXdlYmtpdC1hbmltYXRpb24tZHVyYXRpb246DQogICAgM3M7YW5pbWF0aW9uLWR1cmF0aW9uOiAzczstd2Via2l0LWFuaW1hdGlvbi1pdGVyYXRpb24tY291bnQ6IGluZmluaXRlO2FuaW1hdGlvbi1pdGVyYXRpb24tY291bnQ6DQogICAgaW5maW5pdGU7LXdlYmtpdC1hbmltYXRpb24tdGltaW5nLWZ1bmN0aW9uOiBjdWJpYy1iZXppZXIoMCwgMCwgMSwgMSk7YW5pbWF0aW9uLXRpbWluZy1mdW5jdGlvbjogY3ViaWMtYmV6aWVyKDAsIDAsDQogICAgMSwgMSk7fSNlbF9TLTJkOXlRYTVMe3N0cm9rZTogbm9uZTtzdHJva2Utd2lkdGg6IDE7ZmlsbDogbm9uZTt9I2VsX1I5VVc0LVA3YWh7LXdlYmtpdC10cmFuc2Zvcm06IHRyYW5zbGF0ZSgxMnB4LA0KICAgIDZweCk7dHJhbnNmb3JtOiB0cmFuc2xhdGUoMTJweCwgNnB4KTtmaWxsOiAjZmZmO30jZWxfeG12TFdfWXJXSXstd2Via2l0LXRyYW5zZm9ybTogdHJhbnNsYXRlKDI4cHgsDQogICAgMzlweCk7dHJhbnNmb3JtOiB0cmFuc2xhdGUoMjhweCwgMzlweCk7fSNlbF9ucS1jU2c4a1pfe2Rpc3BsYXk6bm9uZTtmaWxsOiAjMDMyOTljO30jZWxfOHNGM284Z2FqbntkaXNwbGF5Om5vbmU7ZmlsbDoNCiAgICAjRkY5ODAwO30jZWxfVkNIR3VxZUZyV3tkaXNwbGF5Om5vbmU7ZmlsbDogIzAzMjk5Yzt9I2VsX2tMM1pHLXpBcjJ7ZGlzcGxheTpub25lO2ZpbGw6DQogICAgI0Y1OTEyMTt9I2VsX1I5VVc0LVA3YWhfY0ZUdEwwOWItey13ZWJraXQtYW5pbWF0aW9uLW5hbWU6IGVsX1I5VVc0LVA3YWhfY0ZUdEwwOWItX0FuaW1hdGlvbjthbmltYXRpb24tbmFtZToNCiAgICBlbF9SOVVXNC1QN2FoX2NGVHRMMDliLV9BbmltYXRpb247LXdlYmtpdC10cmFuc2Zvcm06IHRyYW5zbGF0ZSg1MC42MDkzNzM4MDc5MDcxMDRweCwgNTAuMTIzMTM2MDg4MjUyMDdweCkNCiAgICByb3RhdGUoMGRlZykgdHJhbnNsYXRlKC01MC42MDkzNzM4MDc5MDcxMDRweCwgLTUwLjEyMzEzNjA4ODI1MjA3cHgpO3RyYW5zZm9ybTogdHJhbnNsYXRlKDUwLjYwOTM3MzgwNzkwNzEwNHB4LA0KICAgIDUwLjEyMzEzNjA4ODI1MjA3cHgpIHJvdGF0ZSgwZGVnKSB0cmFuc2xhdGUoLTUwLjYwOTM3MzgwNzkwNzEwNHB4LCAtNTAuMTIzMTM2MDg4MjUyMDdweCk7fQ0KICA8L3N0eWxlPg0KICANCiAgPGcgaWQ9ImVsX1MtMmQ5eVFhNUwiIGZpbGwtcnVsZT0iZXZlbm9kZCI+DQogICAgPGcgaWQ9ImVsX1I5VVc0LVA3YWhfY0ZUdEwwOWItIiBkYXRhLWFuaW1hdG9yLWdyb3VwPSJ0cnVlIiBkYXRhLWFuaW1hdG9yLXR5cGU9IjEiPg0KICAgICAgPGcgaWQ9ImVsX1I5VVc0LVA3YWgiIGZpbGwtcnVsZT0ibm9uemVybyI+DQogICAgICAgIDxwYXRoIGQ9Ik0yLjA0MTgxMzYyLDE5LjkzNTI3NDggQzMuMDc2NTE2NDYsMTguMDMxNDIxNiAxNC4wNzE5NTg3LDAuMzQ0OTAwOTQ5IDM4LjQ2MzM1MzgsMC4xNjU1NTI0NTUgQzY0LjA1NTAwNDIsLTAuMDEzNzk2MDM3OSA3NS4zNTM5NTkzLDIwLjI1MjU4MzcgNzUuNzMzMzUwMywyMS4xMjE3MzQxIEM3Ny40MTY0NjY5LDI1LjAwNTMxODggNzYuNjk5MDczLDI5Ljk5MjU4NjUgNzMuMTMyNzk3MiwzMS43NzIyNzU0IEM2OS4wNDIyNzE5LDMzLjgxNDA4OSA2NC43NjU1MDAxLDMyLjU4NjI0MTYgNjIuNDgyMjU1OSwyOS4zNTc5Njg4IEM1Ny43NzA5MDg5LDIyLjczNTg3MDUgNTcuMjUzNTU3NSwxNC4zODkyNjc2IDQ1LjEzMzczODEsOS40NDMzODc5NyBDMTguODY2MDgxOSwtMS4yOTY4Mjc1NyAwLjk2NTcyMjY1NiwyMS44NTk4MjIxIDIuMDIxMTE5NTYsMTkuOTI4Mzc2OCBMMi4wNDE4MTM2MiwxOS45MzUyNzQ4IFogTTc1LjE4ODQwNjgsNjguMzMxNzc1OSBDNzQuMTY3NSw3MC4yMjE4MzMxIDYzLjE3MjA1NzgsODcuOTIyMTQ5OCAzOC43ODA2NjI3LDg4LjA4MDgwNDMgQzEzLjE4OTAxMjMsODguMjUzMjU0NyAxLjg3NjI2MTE2LDY4LjAwNzU2OTEgMS41MDM3NjgxNCw2Ny4xMTA4MjY2IEMtMC4yMDY5NDA1NjksNjMuMjQ3OTM2IDAuNTE3MzUxNDIzLDU4LjI2NzU2NjMgNC4wNjk4MzExOSw1Ni40ODc4Nzc0IEM4LjE2NzI1NDQ2LDU0LjQ0NjA2MzggMTIuNDUwOTI0Miw1NS42ODA4MDkyIDE0LjcyNzI3MDUsNTguOTAyMTg0IEMxOS40NDU1MTU1LDY1LjUzODA3ODMgMTkuOTU1OTY4OSw3My44NzA4ODUyIDMyLjA4MjY4NjIsNzguODIzNjYyOCBDNTguMzY0MTM4NSw4OS41NDMxODQzIDc2LjI3MTM5NTgsNjYuNDA3MjI4NyA3NS4xOTUzMDQ4LDY4LjMzODY3NCBMNzUuMTg4NDA2OCw2OC4zMzE3NzU5IFoiIGlkPSJlbF9qOEIzbGZLYmE2Ii8+DQogICAgICA8L2c+DQogICAgPC9nPg0KICAgIDxnIGlkPSJlbF94bXZMV19ZcldJIj4NCiAgICAgIDxwYXRoIGQ9Ik0zOS4xMTM4NCwwLjIwMDA0IEM0Mi45NTcyNCwwLjIwMDA0IDQ1Ljc1OTI0LDMuODI5MDQgNDQuODAzNjQsNy41NTE2NCBDNDMuNzA4NjQsMTEuODE4MDQgNDEuNjg3NDQsMTUuNTA3MDQgMzkuMDYyNjQsMTguMTg2MjQgQzM1LjU1MTY0LDE4LjA3NDQ0IDMyLjM1Njg0LDE2LjcwMjY0IDI5LjkxMzY0LDE0LjUwNzQ0IEMzMS41NTAwNCwxMS44MjU2NCAzMi43MzYyNCw4LjYzMzQ0IDMzLjMxNzQ0LDUuMTM0NDQgQzMzLjc4OTQ0LDIuMjkyMjQgMzYuMjMyNjQsMC4yMDAwNCAzOS4xMTM4NCwwLjIwMDA0IiBpZD0iZWxfbnEtY1NnOGtaXyIvPg0KICAgICAgPHBhdGggZD0iTTI5LjkxMzU0LDE0LjUwNzM2IEMyNy41MDA1NCwxMi4zMzc5NiAyNS44MjQ3NCw5LjM2Mzc2IDI1LjMwNzc0LDYuMDA4NTYgQzI1LjExMTk0LDQuNzM4NTYgMjQuMDI0NzQsMy43OTg5NiAyMi43Mzk3NCwzLjc5ODk2IEMyMS40NjAxNCwzLjc5ODk2IDIwLjM2Nzc0LDQuNzI5OTYgMjAuMTc0MTQsNS45OTQ5NiBDMTkuNjM3NzQsOS41MDAxNiAxNy44MzY5NCwxMi41ODc3NiAxNS4yNTAzNCwxNC43NzkzNiBDMTcuMTYxNzQsMTcuODA2MzYgMTkuNjUyNzQsMjAuMTUzNTYgMjIuNDk1OTQsMjEuNTAxMTYgQzI0LjI1NTM0LDIyLjMzNjc2IDI2LjE0Njc0LDIyLjc5Mjc2IDI4LjEyMjE0LDIyLjc5Mjc2IEwyOC44NjU5NCwyMi43OTI3NiBDMzIuNzAwNzQsMjIuNzkyNzYgMzYuMjMxNTQsMjEuMDc3MzYgMzkuMDYyNTQsMTguMTg2MzYgQzM1LjU1MTc0LDE4LjA3NDM2IDMyLjM1Njc0LDE2LjcwMjc2IDI5LjkxMzU0LDE0LjUwNzM2IiBpZD0iZWxfOHNGM284Z2FqbiIvPg0KICAgICAgPHBhdGggZD0iTTIyLjQ5NTk2LDIxLjUwMTMgQzIwLjczNjU2LDIyLjMzNjkgMTguODQ1MTYsMjIuNzkyNyAxNi44Njk3NiwyMi43OTI3IEwxNi4xMjU5NiwyMi43OTI3IEMxMi4yOTUxNiwyMi43OTI3IDguNzY4MzYsMjEuMDgxMyA1Ljk0MTM2LDE4LjE5ODEgTDUuOTQ1MzYsMTguMTk0MyBDOS40OTE5NiwxOC4xOTQzIDEyLjczODk2LDE2LjkxMDcgMTUuMjUwMTYsMTQuNzc5NSBDMTcuMTYxNzYsMTcuODA2MyAxOS42NTI3NiwyMC4xNTM3IDIyLjQ5NTk2LDIxLjUwMTMiIGlkPSJlbF9WQ0hHdXFlRnJXIi8+DQogICAgICA8cGF0aCBkPSJNMTEuNjcxMzYsNS4xMTE0NiBDMTIuMjcxOTYsOC43Mzk2NiAxMy41MjM1NiwxMi4wMzg4NiAxNS4yNTAzNiwxNC43Nzk0NiBDMTIuNzM4OTYsMTYuOTEwNjYgOS40OTIxNiwxOC4xOTQyNiA1Ljk0NTM2LDE4LjE5NDI2IEw1Ljk0MTM2LDE4LjE5ODI2IEMzLjMxMDM2LDE1LjUxNzg2IDEuMjg0OTYsMTEuODI1NDYgMC4xODgzNiw3LjU1MzY2IEMtMC43Njc0NCwzLjgzMDA2IDIuMDM0MTYsMC4yMDAwNiA1Ljg3ODU2LDAuMjAwMDYgQzguNzUwOTYsMC4yMDAwNiAxMS4yMDIzNiwyLjI3NzY2IDExLjY3MTM2LDUuMTExNDYiIGlkPSJlbF9rTDNaRy16QXIyIi8+DQogICAgPC9nPg0KICA8L2c+DQo8L3N2Zz4=" /></div></loader>`;
    }
}

customElements.define('inv-preloader', Preloader);

//class Accordian extends HTMLElement {
//    constructor() {
//        super()
//        const shadowRoot = this.attachShadow({ mode: 'open' });
//        shadowRoot.innerHTML = `

//            <style>
//                @import url('https://cdnjs.cloudflare.com/ajax/libs/materialize/1.0.0/css/materialize.css')

//            </style>
//            <ul class="collapsible">
//                 <li>
//                    <accordian-head class="collapsible-header">
//                        <slot name="header"></slot>
//                    </accordian-head>
//                    <accordian-body class="collapsible-body">
//                        <slot name="body"></slot>
//                    </accordian-body>
//                </li>
//            </ul>
//            <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.3.1/jquery.min.js"></script>
//        `;
//    }

//    connectedCallback() {
//        $(document).ready(function () {
//            $('.collapsible').collapsible();
//        });
//    }
//}

//customElements.define('accordian-box', Accordian);

//class AccordianHead extends HTMLElement {
//    constructor() {
//        super()
//        const shadowRoot = this.attachShadow({ mode: 'open' });
//        shadowRoot.innerHTML = `
//            <div>
//                <slot></slot>
//            </div>
//        `;
//    }
//}

//customElements.define('accordian-head', AccordianHead);

//class AccordianBody extends HTMLElement {
//    constructor() {
//        super()
//        const shadowRoot = this.attachShadow({ mode: 'open' });
//        shadowRoot.innerHTML = `

//            <div>
//                  <slot name="hello"></slot>
//            </div>
//        `;
//    }
//}

//customElements.define('accordian-body', AccordianBody);


class BackToList extends HTMLElement {
    constructor() {

        super()
        const shadowRoot = this.attachShadow({ mode: 'open' });
        var urlss = this.getAttribute('to');
        var splitDotBackword = urlss;
        if (urlss != null) {
            if (urlss.indexOf("./") > -1) {
                splitDotBackword = splitDotBackword.split('./');
                splitDotBackword = splitDotBackword[1].split('.aspx')
                splitDotBackword = splitDotBackword.filter(function (el) {
                    return el != "";
                });
            }

            if (splitDotBackword.indexOf(".asp") > -1) {
                splitDotBackword = splitDotBackword.split('.aspx')
                splitDotBackword = splitDotBackword.filter(function (el) {
                    return el != "";
                });
            }


        } else {
            splitDotBackword = 'null'
        }


        var url = splitDotBackword

        shadowRoot.innerHTML = `
            <style> 
                  @import url( '../Content/Site.css' );   
                   
                    .mailblock{  
                           text-align: center;
                            width: 110px;
                            position: fixed;
                            cursor: pointer !important;
                            z-index: 9999999999999;
                            overflow: hidden;
                            bottom: 0px;
                            left: 50%;
                     }
                     .backtoliat{
                        background-color: var(--sideNav-bg);
                        box-shadow: var(--z2);
                        color: #fff;
                        display: inline-block;
                        padding: 7px 17px;
                        border-radius: 30px;
                    }

                    .backtoliat .material-icons{
                        display: inline-block;
                        -webkit-transition: all 0.3s;
                        -webkit-backface-visibility: hidden;
                        -moz-transition: all 0.3s;
                        -moz-backface-visibility: hidden;
                        transition: all 0.3s;
                        backface-visibility: hidden;
                        transition:0.2s all;
                        transform: translateY(-300%);    position: absolute;
                        left: calc(50% - 12px );
                        top: calc(0px + 5px );
                    }
                
                    .backtoliat span{    
                        display: inline-block;
                        width: 100%;
                        height: 100%;
                        -webkit-transition: all 0.3s;
                        -webkit-backface-visibility: hidden;
                        -moz-transition: all 0.3s;
                        -moz-backface-visibility: hidden;
                        transition: all 0.3s;
                        backface-visibility: hidden;
                        transform: translateY(3%);

                    }

                    .mailblock:hover span{
                        transition:0s all;
                        transform: translateY(300%);
                    }
                    .mailblock:hover .material-icons{transition:0.3s all;  transform: translateY(0%);}
                    .null{display:none;}
                    .show${url}{display:none;}
                    .shownull{display:inline-block}
                    a[href="n"]{display:none}
                    .shown{display:block;}
            </style>
            <div class="mailblock">
                  <a href="${url}" class="backtoliat ${url}"><i class="material-icons">keyboard_backspace</i><span>Back to List</span></a>
                  <a   class="backtoliat show${url}"><i class="material-icons">keyboard_backspace</i><span>Back to List</span></a>
            </div>
        `;
    }
}

customElements.define('back-to-list', BackToList);


class ButtonComponent extends HTMLElement {
    constructor() {
        super()
        const shadowRoot = this.attachShadow({ mode: 'open' });
        const icon = this.getAttribute('icon');

        shadowRoot.innerHTML = `
            <style> @import url( '../Content/Site.css' );   </style>
            <div class="button-container">
                  <button type="button" class="button">
                    <slot></slot>
                    <i class="material-icons">${icon}</i>
                  </button>
            </div>
        `;
    }
}

customElements.define('paper-button', ButtonComponent);


$(function () {
    $('accordian-header').on('click', function () {
        $(this).toggleClass('isActiveAccordian');
        $(this).next('accordian-body').slideToggle()
    })
});


class Footer extends HTMLElement {
    constructor() {
        super()
        const shadowRoot = this.attachShadow({ mode: 'open' });
        const version = this.getAttribute('version');
        const img = this.getAttribute("logo");
        shadowRoot.innerHTML = `
           <style>
                
                footer{
                    position: sticky;
                    left:0px;
                    right:0px;
                    bottom:0px;
                    padding-left:10px;
                    background:#fff;
                    z-index:999;
                    display:flex; justify-content:space-between;align-items:center;
                    box-sizing:border-box;
                    box-shadow: 0 2px 2px 0 rgba(0,0,0,0.16), 0 0 0 1px rgba(0,0,0,0.08);
                }  

                footer * {box-sizing:border-box}
                @media(max-width: 800px){footer{padding-left:calc(60px + 10px );}}
           </style>
           <footer>
                <div><div style="color: rgb(95, 99, 104); font-size: 13px !important; box-sizing: unset;">${version}</div></div>
                <div><img src="${img}" width="90" /></div> 
           </footer>
        `;
    }
}

customElements.define('footer-bar', Footer);

class Container extends HTMLElement {
    constructor() {
        super()
        const shadowRoot = this.attachShadow({ mode: 'open' });
        const version = this.getAttribute('version');
        const img = this.getAttribute("logo");
        shadowRoot.innerHTML = `
           <style>
                                  @import url( '../Content/Site.css' );   
                    @import url( '../Content/app.css' );  
               
           </style>
           <div class="container">
                <slot></slot>
           </div>
        `;
    }
}

customElements.define('container-box', Container);

//function setAttrSize(a) {
//    return  a
//}

//const tablewidth = document.querySelector('table').clientWidth;

//const setAttrToTable = setAttrSize(tablewidth);
//const targetTable = document.querySelector('table');
//    targetTable.setAttribute('table-width', setAttrToTable)

//const tableseti = document.querySelector('table').getAttribute('table-width')

//const comTab = document.querySelector('table')

//var windowWidth__ = screen.width - 180;
//console.log(windowWidth__ )

//if (tableseti <= windowWidth__) {
//    debugger;
//    comTab.insertAdjacentHTML('afterend', '<div id="right" class="arrow_anim"><i class="material-icons">chevron_right</i></div>');
//    comTab.closest('div').classList.add('is-relative');
//    document.documentElement.style.setProperty('--left-sideWidth', tablewidth + 'px');
//} 


/**
 * expiremental input component 
 * */

function setAttributes(el, attrs) {
    for (var key in attrs) {
        el.setAttribute(key, attrs[key]);
    }
}


function ready() {

    var VALID_CLASS = 'inv-valid',
        INVALID_CLASS = 'inv-invalid',
        PRISTINE_CLASS = 'inv-pristine',
        DIRTY_CLASS = 'inv-dirty',
        UNTOUCHED_CLASS = 'inv-untouched',
        TOUCHED_CLASS = 'inv-touched',
        EMPTY_CLASS = 'inv-empty',
        NOT_EMPTY_CLASS = 'inv-not-empty';
    INVALID_REQUIRED_CLASS = 'inv-invalid-required'


    const inputTypeText = document.querySelectorAll('inv-input input[type="text"]');

    var template__ = ``;

    inputTypeText.forEach(eachInput => {
        //debugger;
        const labelName = eachInput.parentNode.getAttribute('label');
        const validate = eachInput.parentNode.getAttribute('validate');



        if (validate == 'true') {
            eachInput.classList.add('input-validate-is-true');
            eachInput.setAttribute('is_check', 'input-validate-is-true');
            template__ = `
            <span class= "highlight" ></span >
            <span class="animated_bar validate-is-true"></span>
            <label>${labelName}</label>`;
            eachInput.closest('inv-input').classList.add()
        }
        else if (validate == 'false') {
            eachInput.classList.add('input-validate-is-false');
            eachInput.setAttribute('is_check', 'input-validate-is-false');
            template__ = `
            <span class= "highlight" ></span >
            <span class="animated_bar validate-is-false"></span>
            <label>${labelName}</label>`
        }
        else if (validate == "") {
            eachInput.classList.add('input-validate-is-not-defined');
            eachInput.setAttribute('is_check', 'input-validate-is-not-defined');
            template__ = `
            <span class= "highlight" ></span >
            <span class="animated_bar validate-is-not-defined"></span>
            <label>${labelName}</label>`

        }
        else if (validate == undefined) {
            eachInput.classList.add('input-not-validate');
            eachInput.setAttribute('is_check', 'input-not-validate');
            template__ = `
            <span class= "highlight" ></span >
            <span class="animated_bar not-validate"></span>
            <label>${labelName}</label>`
        }

        eachInput.insertAdjacentHTML('afterend', template__);
        eachInput.classList.add(UNTOUCHED_CLASS, EMPTY_CLASS, PRISTINE_CLASS);
        setAttributes(eachInput, { 'required': 'true', 'aria-labelledby': 'inv-label-v-one', 'autocapitalize': 'off', 'autocomplete': 'off', 'autocorrect': "off" });

        eachInput.addEventListener('focusout', function () {
            debugger;
            const currentAttrVal = eachInput.value;
            if (currentAttrVal == undefined || currentAttrVal == "" || currentAttrVal == null) {
                eachInput.parentNode.classList.add('closest_' + INVALID_CLASS, 'closest_' + NOT_EMPTY_CLASS, 'closest' + DIRTY_CLASS);
                eachInput.classList.remove(UNTOUCHED_CLASS, EMPTY_CLASS, PRISTINE_CLASS);
                eachInput.classList.add(INVALID_CLASS, NOT_EMPTY_CLASS, DIRTY_CLASS);
            }
        });


        const breadBlock = document.getElementsByTagName('bread-block', function () {
            const attribut = this.attributes;
            const mapBred = attribut.map(each => `<i class="material-icons">arrow_right</i><span class="breadcrumbd" style="display: inline-block;">${each}</span>`)
        });

    });

}

document.addEventListener("DOMContentLoaded", ready);
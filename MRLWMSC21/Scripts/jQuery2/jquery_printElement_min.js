<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<!DOCTYPE HTML>><HTML><HEAD>
<META content="text/html; charset=utf-8" http-equiv="Content-Type"></HEAD>
<BODY><PRE>/// &lt;reference path="http://code.jquery.com/jquery-1.4.1-vsdoc.js" /&gt;
/*
* Print Element Plugin 1.2
*
* Copyright (c) 2010 Erik Zaadi
*
* Inspired by PrintArea (http://plugins.jquery.com/project/PrintArea) and
* http://stackoverflow.com/questions/472951/how-do-i-print-an-iframe-from-javascript-in-safari-chrome
*
*  Home Page : http://projects.erikzaadi/jQueryPlugins/jQuery.printElement 
*  Issues (bug reporting) : http://github.com/erikzaadi/jQueryPlugins/issues/labels/printElement
*  jQuery plugin page : http://plugins.jquery.com/project/printElement 
*  
*  Thanks to David B (http://github.com/ungenio) and icgJohn (http://www.blogger.com/profile/11881116857076484100)
*  For their great contributions!
* 
* Dual licensed under the MIT and GPL licenses:
*   http://www.opensource.org/licenses/mit-license.php
*   http://www.gnu.org/licenses/gpl.html
*   
*   Note, Iframe Printing is not supported in Opera and Chrome 3.0, a popup window will be shown instead
*/
;(function(g){function k(c){c&amp;&amp;c.printPage?c.printPage():setTimeout(function(){k(c)},50)}function l(c){c=a(c);a(":checked",c).each(function(){this.setAttribute("checked","checked")});a("input[type='text']",c).each(function(){this.setAttribute("value",a(this).val())});a("select",c).each(function(){var b=a(this);a("option",b).each(function(){b.val()==a(this).val()&amp;&amp;this.setAttribute("selected","selected")})});a("textarea",c).each(function(){var b=a(this).attr("value");if(a.browser.b&amp;&amp;this.firstChild)this.firstChild.textContent=
b;else this.innerHTML=b});return a("&lt;div&gt;&lt;/div&gt;").append(c.clone()).html()}function m(c,b){var i=a(c);c=l(c);var d=[];d.push("&lt;html&gt;&lt;head&gt;&lt;title&gt;"+b.pageTitle+"&lt;/title&gt;");if(b.overrideElementCSS){if(b.overrideElementCSS.length&gt;0)for(var f=0;f&lt;b.overrideElementCSS.length;f++){var e=b.overrideElementCSS[f];typeof e=="string"?d.push('&lt;link type="text/css" rel="stylesheet" href="'+e+'" &gt;'):d.push('&lt;link type="text/css" rel="stylesheet" href="'+e.href+'" media="'+e.media+'" &gt;')}}else a("link",j).filter(function(){return a(this).attr("rel").toLowerCase()==
"stylesheet"}).each(function(){d.push('&lt;link type="text/css" rel="stylesheet" href="'+a(this).attr("href")+'" media="'+a(this).attr("media")+'" &gt;')});d.push('&lt;base href="'+(g.location.protocol+"//"+g.location.hostname+(g.location.port?":"+g.location.port:"")+g.location.pathname)+'" /&gt;');d.push('&lt;/head&gt;&lt;body style="'+b.printBodyOptions.styleToAdd+'" class="'+b.printBodyOptions.classNameToAdd+'"&gt;');d.push('&lt;div class="'+i.attr("class")+'"&gt;'+c+"&lt;/div&gt;");d.push('&lt;script type="text/javascript"&gt;function printPage(){focus();print();'+
(!a.browser.opera&amp;&amp;!b.leaveOpen&amp;&amp;b.printMode.toLowerCase()=="popup"?"close();":"")+"}&lt;\/script&gt;");d.push("&lt;/body&gt;&lt;/html&gt;");return d.join("")}var j=g.document,a=g.jQuery;a.fn.printElement=function(c){var b=a.extend({},a.fn.printElement.defaults,c);if(b.printMode=="iframe")if(a.browser.opera||/chrome/.test(navigator.userAgent.toLowerCase()))b.printMode="popup";a("[id^='printElement_']").remove();return this.each(function(){var i=a.a?a.extend({},b,a(this).data()):b,d=a(this);d=m(d,i);var f=null,e=null;
if(i.printMode.toLowerCase()=="popup"){f=g.open("about:blank","printElementWindow","width=650,height=440,scrollbars=yes");e=f.document}else{f="printElement_"+Math.round(Math.random()*99999).toString();var h=j.createElement("IFRAME");a(h).attr({style:i.iframeElementOptions.styleToAdd,id:f,className:i.iframeElementOptions.classNameToAdd,frameBorder:0,scrolling:"no",src:"about:blank"});j.body.appendChild(h);e=h.contentWindow||h.contentDocument;if(e.document)e=e.document;h=j.frames?j.frames[f]:j.getElementById(f);
f=h.contentWindow||h}focus();e.open();e.write(d);e.close();k(f)})};a.fn.printElement.defaults={printMode:"iframe",pageTitle:"",overrideElementCSS:null,printBodyOptions:{styleToAdd:"padding:10px;margin:10px;",classNameToAdd:""},leaveOpen:false,iframeElementOptions:{styleToAdd:"border:none;position:absolute;width:0px;height:0px;bottom:0px;left:0px;",classNameToAdd:""}};a.fn.printElement.cssElement={href:"",media:""}})(window);
</PRE></BODY></HTML>

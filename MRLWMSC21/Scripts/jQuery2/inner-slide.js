//$(document).ready(function() {
$(window).load(function() {
/* Language Country Menu*/
 if(document.getElementById('leadstory_wrapper') != null)
 {
						
 $('#leadstory_wrapper').cycle({ 
    fx:     'fade', 
    timeout: 6000,
	speed: 1000,
	next:   '#leadstory_next', 
    prev:   '#leadstory_prev',
	pause:       1,
	//autostop:    1,
	pager:  '#leadstory_pager'
 });
 
 }
});


//googlesearch
	function inputFocus() {
	document.getElementById('query-input').style['background'] = '';
}

	function inputBlur() {
	var queryInput = document.getElementById('query-input');
	if (!queryInput.value) {
	queryInput.style['background'] =
	'white url(http://www.google.com/coop/images/'
	+ 'google_custom_search_watermark.gif) no-repeat 0% 50%';
}
}

	function init() {
	google.search.CustomSearchControl.attachAutoCompletion(
	'005859023444970944761:qp4m5xcvqek',
	document.getElementById('query-input'),
	'two-page-form');
	inputBlur();
}

	function submitQuery() {
	window.location = 'http://www.silicus.com/search_results.html.php?q='
	+ encodeURIComponent(
	document.getElementById('query-input').value);
	return false;
}
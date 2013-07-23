$(document).ready(function() {
	$('.user-container a').on('click', function (e) {
		e.preventDefault();
		var actionDiv = $('.hidden-field').detach();
		$('.user-main-info', this).after(actionDiv);
		actionDiv.css('display', 'block');
	});
});
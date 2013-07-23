$(document).ready(function () {
	$('.user-container a').on('click', function (e) {
		e.preventDefault();
		var self = e.target;
		if (self.tagName.toUpperCase() != 'DIV') {
			return;
		}
		$('.hidden-field').css('display', 'none');
		if ($('.user-row', this).width() == 230) {
			$('.user-row', this).animate({
				width: "100px"
			}, 1000);
		} else {
			$('.user-row').animate({
				width: "100px"
			}, { duration: 1000, queue: false });
			$('.user-row', this).animate({
				width: "230px"
			}, { duration: 1000, queue: true });
			var actionDiv = $('.hidden-field').detach();
			$('.user-main-info', this).after(actionDiv);
			setTimeout(function () { actionDiv.css('display', 'block'); }, 1000);

		}
	});
});
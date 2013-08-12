$(document).ready(function () {
	$(document).on('click', '.user-container a', function (e) {
		e.preventDefault();
		var self = e.target;
		if (self.tagName.toUpperCase() != 'DIV') {
			return;
		}
		var smaller = 100;
		var larger = 230;
		
		$('.hidden-field').css('display', 'none');
		if ($('.user-row', this).hasClass('active')) {
			$('.user-row', this).animate({
				width: smaller
			}, 1000).removeClass('active');
		} else {
			$('.active').animate({
				width: smaller
			}, { duration: 1000, queue: false }).removeClass('active');
			$('.user-row', this).animate({
				width: larger
			}, { duration: 1000, queue: true }).addClass('active');
			
			var actionDiv = $('.hidden-field').detach();
			$('.user-main-info', this).after(actionDiv);
			setTimeout(function () { actionDiv.css('display', 'block'); }, 1000);
		}
	});
});
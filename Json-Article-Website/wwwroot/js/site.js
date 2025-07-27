// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


/* load moe articles */
$(document).on('click', '.load-more-article button', function (e) {
	e.preventDefault();
	let me = $(this);
	var url = $(me).attr('data-url');
	$.ajax({
		url: url,
		type: 'GET',
		beforeSend: function () {
			$(me).prop('disabled', true);
			// add spinner
			$(me).html('<span class="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span> Loading...');
		},
		success: function (data) {
			$(me).parent().remove();

			if (data) {
				$('.list-group').append(data);
			}
		},
		error: function () {
			$(me).parent().remove();
			//alert('Error loading more articles.');
		}
	});
});


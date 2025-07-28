// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

/* dynamic boostrap model generator */


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

/* Delete artcile */
$(document).on('click', '.delete-article', function (e) {
    e.preventDefault();
    let me = $(this);
    var url = $(me).attr('href');
    if (confirm('Are you sure you want to delete this article?')) {
        $.ajax({
            url: url,
            success: function (data) {
                if (data.success) {
                    $(me).closest('.list-group-item').remove();
                } else {
                    alert(data.message);
                }
            },
            error: function () {
                alert('Error deleting article.');
            }
        });
    }
});


               
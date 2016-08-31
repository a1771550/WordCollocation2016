$(function () {
	$('#Name-error').text('');
	$('#Name').bind('change paste', function() {
		$('#Name-error').text('');
	});

	$('#btnSubmit').click(function(e) {
		e.preventDefault();
		var canSubmit = true;

		if ($('#Name').val() === null || $('#Name').val() === '') {
			var msg = $('#NameRequired').val();
			$('#Name-error').text(msg);
			canSubmit = false;
		}

		if (canSubmit) {
			$('#RoleForm').submit();
		}

	});
});
$.fileup({
	url: '/file/upload',
	inputID: 'upload-2',
	dropzoneID: 'upload-2-dropzone',
	queueID: 'upload-2-queue',
	lang: 'ru',
	onSelect: function(file) {
		$('#multiple button').show();
	},
	onRemove: function(file, total) {
		if (file === '*' || total === 1) {
			$('#multiple button').hide();
		}
	},
	onSuccess: function(response, file_number, file) {
		Snarl.addNotification({
			title: 'Upload success',
			text: file.name,
			icon: '<i class="fa fa-check"></i>'
		});
	},
	onError: function(event, file, file_number) {
		Snarl.addNotification({
			title: 'Upload error',
			text: file.name,
			icon: '<i class="fa fa-times"></i>'
		});
	}
});
// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

"use strict";

var custSpinner = ' <i style="position:relative;" class="fa fa-spinner fa-pulse fa-lg"></i>';

//This is used to serialize the data of a form
function getFormData($form) {
	var unindexed_array = $form.serializeArray();
	var indexed_array = {};

	$.map(unindexed_array, function (n, i) {
		if (typeof indexed_array[n['name']] === "undefined") {
			indexed_array[n['name']] = n['value'];
		}
		else if (typeof indexed_array[n['name']] === "object") {
			indexed_array[n['name']].push(n['value']);
		}
		else {
			var oldVal = indexed_array[n['name']];
			indexed_array[n['name']] = [];
			indexed_array[n['name']].push(oldVal);
			indexed_array[n['name']].push(n['value']);
		}
	});

	return indexed_array;
}

//This is used to serialize an entire form and post it to the path in the action attribute of the form
function saveFormData($form, success_callback, error_callback) {

	var passData = JSON.stringify(getFormData($form));
	var urlref = $form.attr('action');
	if (!urlref) {
		console.error("Error in saving form data. no valid action found on the form");
		return;
	}
	var jqxhr = $.ajax({
		url: urlref,
		type: 'post',
		cache: false, //enable this line if you want jquery to add a parameter to not cache this request. Usually post requests are not cached by IE
		data: { 'passData': passData }

	})
		.done(function (data, textStatus) {
			if (success_callback) {
				success_callback(data);
			}
		})
		.fail(function (jqXHR, textStatus, errorThrown) {

			var jsonError = "";

			try {
				jsonError = JSON.parse(jqXHR.responseText);
			}
			catch (err) {
				if (error_callback) {
					error_callback('Unable to parse error response');
				}
				else {
					console.error('Unable to parse error response: ' + err);
				}
				return;
			}

			if (error_callback) {
				error_callback(jsonError);
			}
			else {
				//If JsonResult does not exist then you are not using the ReturnArgs object on the .Net side and should use the fail callback to handle the message on your own.
				ErrorMessage.show(jsonError.JsonResult);
			}
		});
}

function saveFormDataPost($form, success_callback, error_callback) {

	var passData = getFormData($form);

	var urlref = $form.attr('action');
	if (!urlref) {
		console.error("Error in saving form data. no valid action found on the form");
		return;
	}
	var jqxhr = $.ajax({
		url: urlref,
		type: 'post',
		cache: false, //enable this line if you want jquery to add a parameter to not cache this request. Usually post requests are not cached by IE
		data: passData

	})
		.done(function (data, textStatus) {
			if (success_callback) {
				success_callback(data);
			}
		})
		.fail(function (jqXHR, textStatus, errorThrown) {

			var jsonError = "";

			try {
				jsonError = JSON.parse(jqXHR.responseText);
			}
			catch (err) {
				if (error_callback) {
					error_callback('Unable to parse error response');
				}
				else {
					console.error('Unable to parse error response: ' + err);
				}
				return;
			}

			if (error_callback) {
				error_callback(jsonError);
			}
			else {
				//If JsonResult does not exist then you are not using the ReturnArgs object on the .Net side and should use the fail callback to handle the message on your own.
				ErrorMessage.show(jsonError.JsonResult);
			}
		});
}

var isSubmitting = false;

function saveFormDataSpinner($form, $elem, success_callback, error_callback) {

	if (isSubmitting) {
		return false;
	}
	else {
		isSubmitting = true;
	}

	var passData = JSON.stringify(getFormData($form));
	var origTxt = $elem.html();
	var origPosition = $elem.css("position");
	var origHeight = $elem.height();
	var origWidth = $elem.width();

	$elem.html(custSpinner);
	$elem.css("position", "relative");
	$elem.height(origHeight);
	$elem.width(origWidth);
	$elem.prop('disabled', true);
	var urlref = $form.attr('action');
	if (!urlref) {
		console.error("Error in saving form data. no valid action found on the form");
		return;
	}
	var jqxhr = $.ajax({
		url: urlref,
		type: 'post',
		cache: false, //enable this line if you want jquery to add a parameter to not cache this request. Usually post requests are not cached by IE
		data: { 'passData': passData }

	})
		//success
		.done(function (data, textStatus) {
			$elem.find('i').removeClass('fa-spinner');
			$elem.find('i').removeClass('fa-pulse');
			$elem.html(origTxt);
			try {
				if (data) {
					if (success_callback) {
						setTimeout(function () { success_callback(data); }, 100);

					}
					//initJqDatePicker();
					if (data.success) {
						//show a success status if we receive that response from the server
						$('#nk-data-success').show();
					}
				}
			} catch (err) {
				console.error("error in ajax success:", err.message);
				return;
			}
		})
		//error
		.fail(function (jqXHR, textStatus, errorThrown) {
			$elem.find('i').removeClass('fa-spinner');
			$elem.find('i').removeClass('fa-pulse');
			$elem.html(origTxt);
			var jsonError = "";
			try {
				jsonError = JSON.parse(jqXHR.responseText);
			} catch (err) {
				ErrorMessage('An unexpected error has occurred');
				console.error("error parsing error message as JSON:", err.message);
				return;
			}
			if (error_callback) {
				error_callback(jsonError);
			}
			else {
				//If JsonResult does not exist then you are not using the ReturnArgs object on the .Net side and should use the fail callback to handle the message on your own.
				ErrorMessage.show(jsonError.JsonResult);
			}
		})
		//Always
		.always(function () {
			//$elem.removeAttr('disabled', true);
			//$elem.html(origTxt);
			$elem.css("position", origPosition);
			$elem.prop('disabled', null);
			isSubmitting = false; // allows search button to be enabled
		});
}

function saveFormDataSpinnerPost($form, $elem, success_callback, error_callback) {

	if (isSubmitting) {
		return false;
	}
	else {
		isSubmitting = true;
	}

	var passData = getFormData($form);
	var origTxt = $elem.html();
	var origPosition = $elem.css("position");
	var origHeight = $elem.height();
	var origWidth = $elem.width();

	$elem.html(custSpinner);
	$elem.css("position", "relative");
	$elem.height(origHeight);
	$elem.width(origWidth);
	$elem.prop('disabled', true);
	var urlref = $form.attr('action');
	if (!urlref) {
		console.error("Error in saving form data. no valid action found on the form");
		return;
	}
	var jqxhr = $.ajax({
		url: urlref,
		type: 'post',
		cache: false, //enable this line if you want jquery to add a parameter to not cache this request. Usually post requests are not cached by IE
		data: passData

	})
		//success
		.done(function (data, textStatus) {
			$elem.find('i').removeClass('fa-spinner');
			$elem.find('i').removeClass('fa-pulse');
			$elem.html(origTxt);
			try {
				if (data) {
					if (success_callback) {
						setTimeout(function () { success_callback(data); }, 100);

					}
					//initJqDatePicker();
					if (data.success) {
						//show a success status if we receive that response from the server
						$('#nk-data-success').show();
					}
				}
			} catch (err) {
				console.error("error in ajax success:", err.message);
				return;
			}
		})
		//error
		.fail(function (jqXHR, textStatus, errorThrown) {
			$elem.find('i').removeClass('fa-spinner');
			$elem.find('i').removeClass('fa-pulse');
			$elem.html(origTxt);
			var jsonError = "";
			try {
				jsonError = JSON.parse(jqXHR.responseText);
			} catch (err) {
				ErrorMessage('An unexpected error has occurred');
				console.error("error parsing error message as JSON:", err.message);
				return;
			}
			if (error_callback) {
				error_callback(jsonError);
			}
			else {
				//If JsonResult does not exist then you are not using the ReturnArgs object on the .Net side and should use the fail callback to handle the message on your own.
				ErrorMessage.show(jsonError.JsonResult);
			}
		})
		//Always
		.always(function () {
			//$elem.removeAttr('disabled', true);
			//$elem.html(origTxt);
			$elem.css("position", origPosition);
			$elem.prop('disabled', null);
			isSubmitting = false; // allows search button to be enabled
		});
}

//This now works. You can parse you object from passData, and also user Request.Files to manage your files.
function saveFormDataWithFile($form, success_callback, error_callback) {
	var passData = JSON.stringify(getFormData($form));

	var fd = new FormData();
	fd.append("passData", passData);

	$form.find('input[type="file"]').each(function (i, ui) {
		$.each(ui.files, function (j, ui) {
			fd.append('file_' + i + '_' + j, this);
		});
	});

	console.log("Form Data: " + fd.passData);

	var urlref = $form.attr('action');
	if (!urlref) {
		console.error("Error in saving form data. no valid action found on the form");
		return;
	}
	$.ajax({
		url: urlref,
		type: 'post',
		processData: false, //this needs to be false if you are including a file
		contentType: false, //this needs to be false if you are including a file
		//cache: false, //enable this line if you want jquery to add a parameter to not cache this request. Usually post requests are not cached by IE
		data: fd
	})
		.done(function (data, textStatus) {
			if (success_callback) {
				success_callback(data);
			}
		})
		.fail(function (jqXHR, textStatus, errorThrown) {

			var jsonError = "";

			try {
				jsonError = JSON.parse(jqXHR.responseText);
			}
			catch (err) {
				if (error_callback) {
					error_callback('Unable to parse error response');
				}
				else {
					console.error('Unable to parse error response: ' + err);
				}
				return;
			}

			if (error_callback) {
				error_callback(jqXHR.responseText);
			}
		});
}

function saveFormDataWithFilePost($form, success_callback, error_callback) {
	var passData = getFormData($form);

	var fd = new FormData();
	fd.append("passData", passData);
		
	$form.find('input[type="file"]').each(function (i, ui) {
		$.each(ui.files, function (j, ui) {
			fd.append('file_' + i + '_' + j, this);
		});
	});

	console.log(fd);

	var urlref = $form.attr('action');
	if (!urlref) {
		console.error("Error in saving form data. no valid action found on the form");
		return;
	}
	$.ajax({
		url: urlref,
		type: 'post',
		processData: false, //this needs to be false if you are including a file
		contentType: false, //this needs to be false if you are including a file
		//cache: false, //enable this line if you want jquery to add a parameter to not cache this request. Usually post requests are not cached by IE
		data: fd
	})
		.done(function (data, textStatus) {
			if (success_callback) {
				success_callback(data);
			}
		})
		.fail(function (jqXHR, textStatus, errorThrown) {

			var jsonError = "";

			try {
				jsonError = JSON.parse(jqXHR.responseText);
			}
			catch (err) {
				if (error_callback) {
					error_callback('Unable to parse error response');
				}
				else {
					console.error('Unable to parse error response: ' + err);
				}
				return;
			}

			if (error_callback) {
				error_callback(jqXHR.responseText);
			}
		});
}

//This now works. You can parse you object from passData, and also user Request.Files to manage your files.
function saveCustomDataWithFile($form, customData, success_callback, error_callback) {
	var passData = JSON.stringify(customData);

	var fd = new FormData();
	fd.append("passData", passData);

	$form.find('input[type="file"]').each(function (i, ui) {
		$.each(ui.files, function (j, ui) {
			fd.append('file_' + i + '_' + j, this);
		});
	});

	var urlref = $form.attr('action');
	if (!urlref) {
		console.error("Error in saving form data. no valid action found on the form");
		return;
	}
	$.ajax({
		url: urlref,
		type: 'post',
		processData: false, //this needs to be false if you are including a file
		contentType: false, //this needs to be false if you are including a file
		//cache: false, //enable this line if you want jquery to add a parameter to not cache this request. Usually post requests are not cached by IE
		data: fd
	})
		.done(function (data, textStatus) {
			if (success_callback) {
				success_callback(data);
			}
		})
		.fail(function (jqXHR, textStatus, errorThrown) {

			var jsonError = "";

			try {
				jsonError = JSON.parse(jqXHR.responseText);
			}
			catch (err) {
				if (error_callback) {
					error_callback('Unable to parse error response');
				}
				else {
					console.error('Unable to parse error response: ' + err);
				}
				return;
			}

			if (error_callback) {
				error_callback(jqXHR.responseText);
			}
		});
}

function saveFormDataWithFileSpinner($form, $elem, success_callback, error_callback) {
	var passData = JSON.stringify(getFormData($form));

	var fd = new FormData();
	fd.append("passData", passData);

	$form.find('input[type="file"]').each(function (i, ui) {
		$.each(ui.files, function (j, ui) {
			fd.append('file_' + i + '_' + j, this);
		});
	});

	var origTxt = $elem.html();
	var origPosition = $elem.css("position");
	var origHeight = $elem.height();
	var origWidth = $elem.width();

	$elem.html(custSpinner);
	$elem.css("position", "relative");
	$elem.height(origHeight);
	$elem.width(origWidth);
	$elem.prop('disabled', true);

	var urlref = $form.attr('action');
	if (!urlref) {
		console.error("Error in saving form data. no valid action found on the form");
		return;
	}
	$.ajax({
		url: urlref,
		type: 'post',
		processData: false, //this needs to be false if you are including a file
		contentType: false, //this needs to be false if you are including a file
		//cache: false, //enable this line if you want jquery to add a parameter to not cache this request. Usually post requests are not cached by IE
		data: fd
	})
		.done(function (data, textStatus) {
			$elem.find('i').removeClass('fa-spinner');
			$elem.find('i').removeClass('fa-pulse');
			$elem.html(origTxt);
			try {
				if (data) {
					if (success_callback) {
						setTimeout(function () { success_callback(data); }, 100);

					}
					//initJqDatePicker();
					if (data.success) {
						//show a success status if we receive that response from the server
						$('#nk-data-success').show();
					}

				}
			} catch (err) {
				console.error("error in ajax success:", err.message);
				return;
			}
		})
		//error
		.fail(function (jqXHR, textStatus, errorThrown) {
			$elem.find('i').removeClass('fa-spinner');
			$elem.find('i').removeClass('fa-pulse');
			$elem.html(origTxt);
			var jsonError = "";
			try {
				jsonError = JSON.parse(jqXHR.responseText);
			} catch (err) {
				ErrorMessage('An unexpected error has occurred');
				console.error("error parsing error message as JSON:", err.message);
				return;
			}
			if (error_callback) {
				error_callback(jsonError);
			}
			else {
				//If JsonResult does not exist then you are not using the ReturnArgs object on the .Net side and should use the fail callback to handle the message on your own.
				ErrorMessage.show(jsonError.JsonResult);
			}
		})
		//Always
		.always(function () {
			//$elem.removeAttr('disabled', true);
			//$elem.html(origTxt);
			$elem.css("position", origPosition);
			$elem.prop('disabled', null);
		});
}

function saveFormDataWithFileSpinnerPost($form, $elem, success_callback, error_callback) {
	var passData = getFormData($form);

	var fd = new FormData();
	fd.append("passData", passData);

	$form.find('input[type="file"]').each(function (i, ui) {
		$.each(ui.files, function (j, ui) {
			fd.append('file_' + i + '_' + j, this);
		});
	});

	var origTxt = $elem.html();
	var origPosition = $elem.css("position");
	var origHeight = $elem.height();
	var origWidth = $elem.width();

	$elem.html(custSpinner);
	$elem.css("position", "relative");
	$elem.height(origHeight);
	$elem.width(origWidth);
	$elem.prop('disabled', true);

	var urlref = $form.attr('action');
	if (!urlref) {
		console.error("Error in saving form data. no valid action found on the form");
		return;
	}
	$.ajax({
		url: urlref,
		type: 'post',
		processData: false, //this needs to be false if you are including a file
		contentType: false, //this needs to be false if you are including a file
		//cache: false, //enable this line if you want jquery to add a parameter to not cache this request. Usually post requests are not cached by IE
		data: fd
	})
		.done(function (data, textStatus) {
			$elem.find('i').removeClass('fa-spinner');
			$elem.find('i').removeClass('fa-pulse');
			$elem.html(origTxt);
			try {
				if (data) {
					if (success_callback) {
						setTimeout(function () { success_callback(data); }, 100);

					}
					//initJqDatePicker();
					if (data.success) {
						//show a success status if we receive that response from the server
						$('#nk-data-success').show();
					}

				}
			} catch (err) {
				console.error("error in ajax success:", err.message);
				return;
			}
		})
		//error
		.fail(function (jqXHR, textStatus, errorThrown) {
			$elem.find('i').removeClass('fa-spinner');
			$elem.find('i').removeClass('fa-pulse');
			$elem.html(origTxt);
			var jsonError = "";
			try {
				jsonError = JSON.parse(jqXHR.responseText);
			} catch (err) {
				ErrorMessage('An unexpected error has occurred');
				console.error("error parsing error message as JSON:", err.message);
				return;
			}
			if (error_callback) {
				error_callback(jsonError);
			}
			else {
				//If JsonResult does not exist then you are not using the ReturnArgs object on the .Net side and should use the fail callback to handle the message on your own.
				ErrorMessage.show(jsonError.JsonResult);
			}
		})
		//Always
		.always(function () {
			//$elem.removeAttr('disabled', true);
			//$elem.html(origTxt);
			$elem.css("position", origPosition);
			$elem.prop('disabled', null);
		});
}

//This is used when you would like to post to the server, but do not have a form.
function postAjax(url, paramsObj, successCallback, failCallback) {
	$.ajax({
		url: url,
		type: "post",
		cache: false,
		data: paramsObj
	}).done(function (data) {
		if (successCallback) {
			if (typeof data === 'string') {
				if (data.trim().substring(0, 1) !== '<') {
					data = JSON.parse(data);
				}
			}
			successCallback(data);
		}
	}).fail(function (jqXHR, textStatus) {
		if (failCallback) {
			var retVal = jqXHR.responseText;
			if (typeof retVal === 'string') {
				retVal = JSON.parse(retVal);
			}
			if (failCallback) {
				failCallback(retVal);
			}
		} else {
			//If JsonResult does not exist then you are not using the ReturnArgs object on the .Net side and should use the fail callback to handle the message on your own.
			ErrorMessage.show(JSON.parse(jqXHR.responseText).JsonResult);
		}
	});
}

//This is going to be use to get content but not need to open a modal, maybe its already open and we need to flip content
function getAjax(url, elem, successCallback, failCallback) {
	var success = false;
	var jqxhr = $.ajax(
		{
			url: url,
			type: 'get',
			cache: false
		})
		.done(function (data, textStatus) {
			if (successCallback) {
				if (data && data.view) {
					elem.html(data.view); //replace the container with received content
					addRequiredMarks();
					success = true;
				} else if (data) {
					elem.html(data); //replace the container with received content
					addRequiredMarks();
					success = true;
				} else {
					console.error("Unable to parse ajax response", data);
					ErrorMessage.show('There was an error retrieving your data!');
				}
				successCallback(data);
			}
		})
		.fail(function (jqXHR, textStatus, errorThrown) {
			var jsonError = "";
			try {
				jsonError = JSON.parse(jqXHR.responseText);
			} catch (err) {
				console.error("Unable to parse ajax error response as JSON", err.message);
				ErrorMessage.show('There was an error retrieving your data!: ' + err.message);
				return;
			}
			if (failCallback) {
				failCallback(jsonError);
			} else {
				if (typeof jsonError !== 'undefined' && jsonError.JsonResult) {
					ErrorMessage.show(jsonError.JsonResult);
					return;
				}
				else {
					alert('There was an error retrieving your data!');
				}
			}
		});
}

var isAjaxSubmitting = false;
function postAjaxSpinner(url, paramsObj, $elem, successCallback, failCallback) {

	$elem.prop('disabled', true);
	var origTxt = $elem.html();
	var origPosition = $elem.css("position");
	var origHeight = $elem.height();
	var origWidth = $elem.width();

	$elem.html(custSpinner);
	$elem.css("position", "relative");
	$elem.height(origHeight);
	$elem.width(origWidth);

	$.ajax({
		url: url /*+ '?_=' + new Date().getTime()*/, //We can not assume there will not be parameters added to the URL. The time stamp fails if there are.
		type: "post",
		cache: false,
		//contentType: 'application/json',
		data: paramsObj
	}).done(function (data) {
		$elem.find('i').removeClass('fa-spinner');
		$elem.find('i').removeClass('fa-pulse');
		$elem.html(origTxt);
		//Why not parse the data here so it is not a string in the callback?
		//Just check for < starting the result.
		if (successCallback) { successCallback(data); }
	}).fail(function (jqXHR, textStatus) {
		$elem.find('i').removeClass('fa-spinner');
		$elem.find('i').removeClass('fa-pulse');
		$elem.html(origTxt);
		var jsonError = "";
		try {
			jsonError = JSON.parse(jqXHR.responseText);
		} catch (err) {
			ErrorMessage.show('An unexpected error has occurred');
			console.error("error parsing error message as JSON:", err.message);
			return;
		}
		if (failCallback) {
			failCallback(jsonError);
		}
		else {
			//If JsonResult does not exist then you are not using the ReturnArgs object on the .Net side and should use the fail callback to handle the message on your own.
			ErrorMessage.show(jsonError.JsonResult);
		}
	}).always(function () {
		$elem.css("position", origPosition);
		$($elem).prop('disabled', null);
	});

}

//This ca be used to select a specific parameter from the current document href
function getParameterByName(name, url) {
	if (!url) url = window.location.href;
	name = name.replace(/[\[\]]/g, "\\$&");
	var regex = new RegExp("[?&]" + name + "(=([^&#]*)|&|#|$)"),
		results = regex.exec(url);
	if (!results) return null;
	if (!results[2]) return '';
	return decodeURIComponent(results[2].replace(/\+/g, " "));
}

//This is used to open a modal where the layout is to be parsed on the server using razor.
//Use the callback to set up form validation or any other after action setup
function openModal(modalURL, callback) {
	var success = false;
	var
		jqxhr = $.ajax({
			url: modalURL,
			type: 'get',
			cache: false
		})
			.done(function (data, textStatus) {
				$("#modalAddEdit").html(data); //replace the modal with received content
				addRequiredMarks();
				success = true;
			})
			.fail(function (jqXHR, textStatus, errorThrown) {
				var jsonError = "";
				try {
					jsonError = JSON.parse(jqXHR.responseText);
				} catch (err) {
					console.error("Unable to parse ajax error response as JSON", err.message);
					ErrorMessage.show('There was an error retrieving your data!: ' + err.message);
					return;
				}
				if (typeof jsonError !== 'undefined' && jsonError.JsonResult) {
					ErrorMessage.show(jsonError.JsonResult);
					return;
				}
				else {
					alert('There was an error retrieving your data!');
				}
			})
			.always(function (data, textStatus) {
				if (textStatus !== "error") {
					$('#modalAddEdit').modal();
					if (callback) { callback(success); }
				}
			});

}

//This is used to open a confirmation modal where the layout is to be parsed on the server using razor.
//Use the callback to set up form validation or any other after action setup
function openConfirmModal(modalURL, callback) {
	var success = false;
	var
		jqxhr = $.ajax({
			url: modalURL,
			type: 'get',
			cache: false
		})
			.done(function (data, textStatus) {
				$("#modalConfirm").html(data); //replace the modal with received content
				addRequiredMarks();
				success = true;
			})
			.fail(function (jqXHR, textStatus, errorThrown) {
				var jsonError = "";
				try {
					jsonError = JSON.parse(jqXHR.responseText);
				} catch (err) {
					console.error("Unable to parse ajax error response as JSON", err.message);
					ErrorMessage.show('There was an error retrieving your data!: ' + err.message);
					return;
				}
				if (typeof jsonError !== 'undefined' && jsonError.JsonResult) {
					ErrorMessage.show(jsonError.JsonResult);
					return;
				}
				else {
					alert('There was an error retrieving your data!');
				}
			})
			.always(function (data, textStatus) {
				if (textStatus !== "error") {
					$('#modalConfirm').modal();
					if (callback) { callback(success); }
				}
			});

}

//This is used to open a modal, nested inside of another modal, where the layout is to be parsed on the server using razor.
//Use the callback to set up form validation or any other after action setup
function openNestedModal(modalURL, callback) {
	var success = false;
	var
		jqxhr = $.ajax({
			url: modalURL,
			type: 'get',
			cache: false
		})
			.done(function (data, textStatus) {
				$("#modalAddEditNested").html(data); //replace the modal with received content
				addRequiredMarks();
				success = true;
			})
			.fail(function (jqXHR, textStatus, errorThrown) {
				var jsonError = "";
				try {
					jsonError = JSON.parse(jqXHR.responseText);
				} catch (err) {
					console.error("Unable to parse ajax error response as JSON", err.message);
					ErrorMessage.show('There was an error retrieving your data!: ' + err.message);
					return;
				}
				if (typeof jsonError !== 'undefined' && jsonError.JsonResult) {
					ErrorMessage.show(jsonError.JsonResult);
					return;
				}
				else {
					alert('There was an error retrieving your data!');
				}
			})
			.always(function (data, textStatus) {
				if (textStatus !== "error") {
					$('#modalAddEditNested').modal();
					if (callback) { callback(success); }
				}
			});

}

//This is names poorly since any message can be sent to is. It does not need to be an error.
var ErrorMessage = {
	show: function (message, title, closeCallback) {
		var selector = 'errormessage';
		var btn = $('#' + selector).find('[type="button"]');
		if (closeCallback) {
			$(btn).on('click', function () {
				$('#errormessage').modal('hide');
				//If we have a callback, wait just a little longer than the fadeout time on the modal and then call it.
				setTimeout(closeCallback, 410);
			});
		}
		else {
			$(btn).off('click');
		}

		$('#errormessageTitle').html(title || "Error:");
		$('#errormessageContent').html(message || "");
		$('#errormessage').modal();
	}
};

//This is used to replace the browsers built in confirmation
var Confirm = function (message, title, callback) {
	$.confirm({
		animation: 'top',
		title: title,
		content: message,
		buttons: {
			confirm: {
				btnClass: 'btn-primary',
				action: function () {
					callback(true);
				}
			},
			cancel: {
				btnClass: 'btn-secondary',
				action: function () {
					callback(false);
				}
			}
		}
	});
};

// This is used to show and simulate a mobile success message using jquery
//There is no layout for this. If you are adding to the template please make sure you include all elements 
//and a demo so people understand how it works and if it is what they are looking for or not.
//var SuccessMessage = {
//    show: function (message, header = 'Success') {
//        $('#mobileSuccessMessage .inner-header').html('').html(header);
//        $('#mobileSuccessMessage .inner-message').html('').html(message);
//        $('#mobileSuccessMessage').fadeIn();
//        setTimeout(function () {
//            $('#mobileSuccessMessage').fadeOut();
//        }, 4000);
//    }
//};

//This can be used to limit allowed key presses on number fields
//use as onkeypress="return isNumber(event)"
function isNumber(evt) {
	evt = (evt) ? evt : window.event;
	var charCode = (evt.which) ? evt.which : evt.keyCode;
	if (charCode > 31 && (charCode < 48 || charCode > 57)) {
		return false;
	}
	return true;
}

//This is a jQuery plugin that allows for the tablesorter to be unset from a table.
//Use like $('table').unbindTablesorter().tablesorter()
(function ($) {
	$.fn.unbindTablesorter = function () {
		$(this).unbind('appendCache applyWidgetId applyWidgets sorton update updateCell')
			//.removeClass('tablesorter')
			.find('thead th')
			.unbind('click mousedown')
			.removeClass('header headerSortDown headerSortUp');
		return this;
	};

	$.fn.bindTablesorter = function () {
		$(this).unbindTablesorter().tablesorter();
		return this;
	};

	//The plugin returns a bool value and will style the element if it fails before returning false.
	//We have to be using ParslyJs for this to work.
	$.fn.validateSingle = function () {
		$(this).parsley().reset();
		if ($(this).parsley().isValid()) {
			return true;
		}
		else {
			$(this).parsley().validate();
			return false;
		}
	}
})(jQuery);

//This will take just about anything and return a treue or false.
//Note that if an object or array is passed, the result will always be false.
//Usage: if(val && parseBool(val)){ /*Do some stuff*/ }
function parseBool(val) { return val === true || val.toLowercase() === "true" }

//This function will replace all instance of found text, instead of the default javascript behavior of just replacing the first occurance it locates.
String.prototype.replaceAll = function (target, value) {
	var exit = false;
	var retVal = this;
	if (target) {
		while (!exit) {
			if (retVal.indexOf(target) > -1) {
				retVal = retVal.replace(target, value);
			}
			else {
				exit = true;
			}
		}
		return retVal;
	}
};

//This is a more robust version of the above function: isNumber
//It will condition the value of a text input based on attributes added to the element that define the allowed precision of the number being inputed.
var numericInputMasker = {
	KeyPressHandler: function (evt) {

		var el = this;

		var wholePrecision = $(el).attr("wholePrecision");
		var decimalPrecision = $(el).attr("decimalPrecision");

		var charCode = (evt.which) ? evt.which : event.keyCode;
		var number = el.value.split('.');

		//dissallow anything thats not a number
		if ((charCode < 48 || charCode > 57) && decimalPrecision === 0) {
			return false;
		}

		//dissallow anything thats not a number, a dot
		if (charCode !== 46 && (charCode < 48 || charCode > 57)) {
			return false;
		}

		//allowing only # of chars after the dot
		var caratPos = numericInputMasker.getSelectionStart(el);
		var dotPos = el.value.indexOf(".");

		if (number[0]) {
			if (number[0].length >= wholePrecision && charCode !== 46 && number[1] === undefined) {
				return false;
			}
		}

		if (number[1]) {
			if (number[1].length >= decimalPrecision && caratPos > dotPos) {
				return false;
			}

			if (caratPos > dotPos && dotPos > -1 && (number[1].length > 1)) {
				return false;
			}
		}

		return true;
	},

	getSelectionStart: function (o) {
		//http://javascript.nwbox.com/cursor_position/
		if (o.createTextRange) {
			var r = document.selection.createRange().duplicate()
			r.moveEnd('character', o.value.length)
			if (r.text === '') return o.value.length
			return o.value.lastIndexOf(r.text)
		} else return o.selectionStart
	}

};

//This can be used when you need to validate a single field using Parsley.
function validateSingle(id, caller, callback) {
	var elem = $('#' + id);
	if (!$(elem).length) {
		var err = 'No ID Sent To Validate';
		console.error(err);
		throw err;
	}
	if ($(elem).parsley().isValid()) { callback(caller); }
	else { $(elem).parsley().validate(); }
}

//I am not really sure why this was added, but it could be useful. 
//This will redirect to the url passed in as the parameter if it is not empty.
//document.location = location is all you need.
function goToPage(location) {
	if (!location || location.length < 1) {
		console.error("invalid redirect url");
		return;
	}
	var url = window.location.protocol + '//' + window.location.hostname + (window.location.port ? ':' + window.location.port : ''); //port is empty for default port

	url = url + location;
	window.location = url;
}

//Keepalive
var keepAliveUrl = '';
var loginUrl = '';
var keepAlive = function (keepAliveUrl, loginUrl) {
	keepAliveUrl = keepAliveUrl; loginUrl = loginUrl;
	setInterval(function () { _keepalive(keepAliveUrl, loginUrl); }, 60000);//Call this function every 60 seconds
};

var _keepalive = function (keepAliveUrl, loginUrl) {
	var pathExceptions = [
		"/login/"//This should cover anything pertaining to login
		/*Add Exceptions Here*/
	];

	function _isPathException() {
		for (var i in pathExceptions) {
			var p = pathExceptions[i];
			if (document.location.href.toLowerCase().indexOf(p.toLowerCase()) > -1) {
				return true;
			}
		}
		return false;
	}

	$.ajax({
		url: keepAliveUrl,
		type: 'post',
		cache: false
	})
		.done(function (data, textStatus) {
			if (_isPathException()) { return; }
			if (data) {
				//If we have a session, success is returned. Otherwise fail.
				if (data === 'fail') {
					document.location = loginUrl;
				}
			}
		})
		.fail(function (jqXHR, textStatus, errorThrown) {
			if (_isPathException()) { return; }
			document.location = loginUrl;
		});
};

var addRequiredMarks = function () {
	var reqArr = $('[required]');

	for (var i in reqArr) {
		var lbl = null;
		var elem = reqArr[i];
		if (i === 'length') { break; }

		// testing for input groups in forms, its a layer deeper than normal form elements
		var inputGroup = $(elem).parent('.input-group');
		if (inputGroup && inputGroup.length > 0) {
			lbl = inputGroup.siblings('label');
		} else {
			lbl = $(elem).prev('label');
			if (!lbl) {
				lbl = $(elem).after('label');
			}
		}

		if (lbl) {
			var spn = $(lbl).find('.requiredMark');
			var hasLbl = $(spn).length && $(spn).length > 0;
			if (!hasLbl) {
				$(lbl).append('<span class="requiredMark" title="A value is required">&nbsp;*</span>');
			}
		}
	}

}
$(function () { addRequiredMarks(); });

var cleanRequiredMarks = function () {
	$('.requiredMark').remove();
	addRequiredMarks();
}

//Trying out this function to pad leading zeroes onto user entered code to fit required string length on Local Government Add.
function paddy(n, p, c) {
	var pad_char = typeof c !== 'undefined' ? c : '0';
	var pad = new Array(1 + p).join(pad_char);
	return (pad + n).slice(-pad.length);
}

// super nifty function to remove duplicates in array
function nkUniqueArray(inArr) {
	return inArr.filter((x, i, a) => a.indexOf(x) == i);
}

/*
 * Use Like:
	//Create a 1 minute timer
	var timy = null
	$(function () {
		timy = new Timer(6000, function (args) {
			alert(args);
			timy.stop();
		});
		timy.start('One minute has passed. Timer will now stop');
		console.log(t);
	});
	
 */
var Timer = function (interval, event) {
	this.timer = null;
	this.interval = interval;
	this.start = function (args) {
		this.args = args;
		this._startTimer();
	};
	this._startTimer = function () {
		this.timer = setInterval((function (args) {
			if (this.eventHandler) {
				this.eventHandler(this.args);
			}
		}).bind(this), this.interval);
	},
		this.stop = function () {
			clearInterval(this.timer);
		};
	this.reset = function (interval, args, eventHandler) {
		this.stop();
		if (interval) {
			this.interval = interval;
		}
		if (args) {
			this.args = args;
		}
		if (eventHandler) {
			this.eventHandler = eventHandler;
		}
		this._startTimer();
	};
	this.eventHandler = event;
	this.args = null;
};

/*
 * Auto fade in page content
 * Create Date Pickers
 */

$(function () {
	$('.auto-fade-in').css({ 'visibility': 'visible', 'display': 'none' }).fadeIn();
	$('input[datepicker]').datepicker({
		uiLibrary: 'bootstrap4'
	}).on('changeDate', function () {
		$(this).datepicker('hide');
	});
	//This should no longer be needed
	//readyCheckboxes();
});

//This should no longer be necessary
//function readyCheckboxes() {
//	//This will set up any checkbox that has the 'autovalue' attribute with an accompanying hidden field.
//	//The hidden field will steal its name and wire up an on click event. It will also look for a value attribute
//	//in order to set its initial state. Example <input type="checkbox" autovalue name="nameMovedToHiddenField" value="true" onclick="$(do a thing)"/>
//	$('input[type="checkbox"][autovalue]').each(function () {
//		//Grab the name off the checkbox
//		var name = $(this).attr('name');
//		//If we have no name, skip this checkbox becuase it is not supposed to post.
//		if (!name || name == '') { return true; }
//		//Remove the name attribute from the checkbox
//		$(this).removeAttr('name');

//		//Grab the value from the data-value attribute. This should be 'true' or 'false'. If it is not there, assume false. 
//		var value = $(this).val() || 'false';

//		//'Create the hidden field
//		var hip = $('<input type="hidden" value="' + value + '" name="' + name + '" />');

//		//If there is a click handler on the checkbox, grab it
//		var clk = $(this)[0].onclick;
//		$(this)[0].onclick = null;
//		$(this).prop('onclick', null);
//		var self = this;
//		//Set the  click handler on this checkbox to:
//		$(this).on('click', function (e) {
//			//Set our value depending on if the checkbox is checked
//			$(hip).val($(self).is(':checked'));
//			//If there was a click handler, call it.
//			if (clk) {
//				clk(e);
//			}

//			//The below chain call sets the checked property on the checkbox to the value it was set to. True is checked.
//		}).prop('checked', (value == 'true' ? 'checked' : null));

//		//Append the hidden field into the DOM right before this checkbox.
//		$(this).before(hip);
//	});

//}

//This can be used to tame events that fire really fast.
function debounce(fn, duration) {
	var timer;
	return function () {
		clearTimeout(timer);
		timer = setTimeout(fn, duration);
	};
}

//Shows a spinner center screen
//Spinner.show()/Spinner.hide()
//If true is passed on show, no mask is shown: Spinner.show(true) no mask is shown.
var Spinner = {
	show: function (noMask) {
		var s = '.spinner';
		if (!noMask) {
			s = '.spinnerMask, ' + s;
		}
		$(s).fadeIn();
	},
	hide: function () {
		$('.spinnerMask, .spinner').fadeOut();
	}
};


//Ajax File Upload
var Upload = function (url, id, file) {
	this.file = file;
	this.id = id;
	this.url = url;
};
Upload.prototype.getType = function () {
	return this.file.type;
};
Upload.prototype.getSize = function () {
	return this.file.size;
};
Upload.prototype.getName = function () {
	return this.file.name;
};
Upload.prototype.doUpload = function (callback) {
	$('#progress-wrp').fadeIn();
	var that = this;
	var formData = new FormData();

	// add assoc key values, this will be posts values
	formData.append("file", this.file, this.getName());
	formData.append("id", this.id);
	formData.append("upload_file", true);

	$.ajax({
		type: "POST",
		url: this.url,
		xhr: function () {
			var myXhr = $.ajaxSettings.xhr();
			if (myXhr.upload) {
				myXhr.upload.addEventListener('progress', that.progressHandling, false);
			}
			return myXhr;
		},
		success: function (data) {
			$('#progress-wrp').fadeOut();
			if (callback) {
				callback(data);
			}
		},
		error: function (jsonError) {
			ErrorMessage.show(jsonError.JsonResult);
			$('#progress-wrp').hide();
		},
		async: true,
		data: formData,
		cache: false,
		contentType: false,
		processData: false,
		timeout: 60000
	});
};
Upload.prototype.progressHandling = function (event) {
	var percent = 0;
	var position = event.loaded || event.position;
	var total = event.total;
	var progress_bar_id = "#progress-wrp";
	if (event.lengthComputable) {
		percent = Math.ceil(position / total * 100);
	}
	// update progressbars classes so it fits your code
	$(progress_bar_id + " .progress-bar").css("width", +percent + "%");
	$(progress_bar_id + " .status").text(percent + "%");
};

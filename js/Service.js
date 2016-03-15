
(function ($) {
	
	$.callWebService = function (options) {
		// Set defaults
		options = $.extend({
			type: 'POST',
			data: {},
			convertData: true,
			isJSON: true,
			success: function () { },
			error: function () { },
			serviceName: null,
			serviceUrl: 'DataService.asmx'
		}, options);

		// Input validation
		if (options.serviceName == null)
			throw "DP | callWebService: Service can't be null";

		// Setup ajax options
		var ajaxOptions = {};
		ajaxOptions.type = options.type;
		ajaxOptions.url = options.serviceUrl + '/' + options.serviceName;
		ajaxOptions.data = options.data;
		if (options.isJSON) {
			if (options.convertData)
				ajaxOptions.data = JSON.stringify(ajaxOptions.data);
			ajaxOptions.contentType = 'application/json; charset=utf-8';
			ajaxOptions.dataType = 'json';
		}
		ajaxOptions.success = function (data) {
			options.success.call(this, data.d);
		};

		ajaxOptions.error = function () {
			options.error.call(this);
			if (options.errorOptions != null && options.errorElement != null)
				options.errorElement.writeError(options.errorOptions);
		};

		// The ajax call
		return $.ajax(ajaxOptions);
	};
})(jQuery);

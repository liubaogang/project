(function () {
    $(function () {
        var input = $('#input_apiKey');

        input.attr("placeholder", "授权码");
        input.off();
        input.on('change', function () {
            var key = this.value;
            if (key && key.trim() !== '') {
                swaggerUi.api.clientAuthorizations.add("key", new SwaggerClient.ApiKeyAuthorization("Authentication", key, "header"));
            }
        });

        setTimeout(function () {
            if (window.SwaggerTranslator) {
                window.SwaggerTranslator.translate();
            }
        }, 1000);
    });
})();
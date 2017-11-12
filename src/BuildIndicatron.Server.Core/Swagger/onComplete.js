$("#input_apiKey").unbind();

$('#input_apiKey').change(function() {
    var key = "Bearer " + $('#input_apiKey')[0].value;
    if (key && key.trim() != "") {
        swaggerUi.api.clientAuthorizations.add("key", new SwaggerClient.ApiKeyAuthorization("Authorization",  key, "header"));
    }
});

$("#loginButton").unbind();
$("#loginButton").click(function (event) {
    event.preventDefault();
    $.ajax({
        type: "POST",
        url: "../../token",
        data: $("#loginForm").serialize(),
        success: function(returnval) {
            key = "Bearer " + returnval.access_token;
            $("#input_apiKey").val(returnval.access_token);
            swaggerUi.api.clientAuthorizations.add("key", new SwaggerClient.ApiKeyAuthorization("Authorization",  key, "header"));
            
            $('#LoginFormSection').hide('slow');
        },
        error: function(returnval) {
            $("#loginButton").wiggle();
        },
        dataType: 'json'
    });

});
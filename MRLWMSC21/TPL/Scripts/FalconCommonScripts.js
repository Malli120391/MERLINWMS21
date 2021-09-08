function fnLoadMCodeData() {

    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
    function EndRequestHandler(sender, args) {
        if (args.get_error() == undefined) {
            fnLoadMCode();
        }
    }

    function fnLoadMCode() {

        $(document).ready(function () {
            $('.MCodePicker').autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadMCodeData") %>',
                        data: "{ 'prefix': '" + request.term + "'}",
                        dataType: "json",
                        type: "POST",
                        async: true,
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            response(data.d)
                        },
                        error: function (response) {
                            alert(response.responseText);
                        },
                        failure: function (response) {
                            alert(response.responseText);
                        }

                    });
                },
                minLength: 0
            });

        });

        $('.DateBoxCSS_small').datepicker({ dateFormat: 'dd/M/yyyy' });
    }

    fnLoadMCode();
}
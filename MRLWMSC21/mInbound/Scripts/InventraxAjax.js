var InventraxAjax = InventraxAjax || {};

InventraxAjax.AjaxResultExecute = function (ServiceURL, InputData, onsuccess, onerror, onfailure) {
    this._response = new ExecutionResponse("success", '', null);
    //debugger
    $.ajax({
        url: ServiceURL,
        data: InputData,
        dataType: "json",
        type: "POST",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            //debugger
            this._response = new ExecutionResponse(data.d.Status, data.d.Message, data.d);
            if (onsuccess != null) {
                window[onsuccess](this._response);
            }
        },
        error: function (response) {
            this._response = new ExecutionResponse(false, response.responseText, response);
            if (onerror != null) {
                window[onerror](this._response);
            }
        },
        failure: function (response) {
            this._response = new ExecutionResponse(false, response.responseText, response);
            if (onfailure != null) {
                window[onfailure](this._response);
             

            }
        }
    });
}

function ExecutionResponse(status, message, response) {
    this.Status = status;
    this.Message = message;
    this.Result = response;
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;

namespace MRLWMSC21
{
    [Serializable]
    [ScriptService]
    public class ExecutionResponse
    {
        bool _status;

        public ExecutionResponse() { }
        public ExecutionResponse(bool status) { this.Status = status; }
        public ExecutionResponse(bool status, string message) { this.Status = status; this.Message = message; }
        public bool Status { get { return _status; } set { _status = value; } }
        public string Message { get; set; }

    }
}
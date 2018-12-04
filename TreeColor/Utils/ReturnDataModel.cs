using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TreeColor.Utils
{
    public class ReturnDataModel
    {
        public Exception Exception { get; set; }
        public string Message { get; set; }

        public bool IsSuccess => Exception == null && string.IsNullOrWhiteSpace(Message);

        public void LoadException(Exception ex)
        {
            Exception = ex;
            Message = ex.Message;
        }
    }

    public class ReturnDataModel<T> : ReturnDataModel
    {
        public T Data { get; set; }
    }
}
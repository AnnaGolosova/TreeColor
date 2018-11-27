using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ThreeColor.Server.Helpers
{
    public class ReturnDataModel<T>  : ReturnDataModel where T : new()
    {
        public T Data;
    }

    public class ReturnDataModel
    {
        public Exception Exception;
        public string ErrorMessage;

        public bool IsSuccess
        {
            get
            {
                return string.IsNullOrEmpty(ErrorMessage) &&
                    Exception == null;
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileMoverUI
{
    public class ActionOutCome
    {
        public ActionOutCome(bool success, string message, object dataObject = null)
        {
            Message = message;
            Success = success;
            DataObject = dataObject; 
        }
        public string Message { get; private set; }
        public bool Success { get; private set; }
        public object DataObject { get; private set; }
    }
}

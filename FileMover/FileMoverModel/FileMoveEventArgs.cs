using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileMoverModel
{
    public class FileMoveEventArgs : EventArgs
    {
        public FileMoveEventArgs(decimal progress)
        {
            Progress = progress;
        }
        public decimal Progress { get; private set; }

        public bool Cancelled { get; set; }
    }
}

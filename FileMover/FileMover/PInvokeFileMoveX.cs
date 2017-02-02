using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileMover
{
    public static class PInvokeFileMoveX
    {
        public static Task<bool> MoveFileWithProgress()
        {
            return Task.FromResult<bool>(true);
        }
    }
}

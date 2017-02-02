using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileMover
{
    public class FileMover_PInvoke : IFileMover
    {
        private bool _cancelled;
        public string DestinationPath { get; set; }

        public bool IsMoving { get; private set; }
        public string NewFileName { get; set; }

        public decimal Progress { get; set; }

        public Action<decimal> ProgressUpdater { private get; set; }

        public string ReturnMessage { get; set; }

        public string SourcePath { get; set; }

        public bool Success { get; set; }

        public void Cancel()
        {
            _cancelled = true;
        }

        public async Task<bool> MoveAsync()
        {
            _cancelled = false;
            IsMoving = true;
            var success = false;
            try
            {
                await PInvokeFileMoveX.MoveFileWithProgress();
            }
            catch (Exception x)
            {

            }
            finally
            {
                IsMoving = false;
            }
            Success = success;
            return success;

        }

    }
}

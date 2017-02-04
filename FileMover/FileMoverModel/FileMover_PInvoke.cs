using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileMoverModel
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

        public void ProgressCallback(FileMoveEventArgs fileMoveEventArgs)
        {
            ProgressUpdater(fileMoveEventArgs.Progress);
            if(_cancelled)
            {
                fileMoveEventArgs.Cancelled = true;
            }
        }

        public async Task<bool> MoveAsync()
        {
            ReturnMessage = "";
            Success = false;
            _cancelled = false;
            IsMoving = true;
            var success = false;
            try
            {
                var mover = new PInvokeFileMoveX(SourcePath, DestinationPath, ProgressCallback);
                success = await mover.MoveFile();
            }
            catch (Exception x)
            {
                ReturnMessage = x.Message;
                success = false;
            }
            finally
            {
                IsMoving = false;
            }
            Success = success;
            if (success)
            {
                ReturnMessage = "Moved Successfully";
            }
            else if (!success && _cancelled)
            {
                ReturnMessage = "Cancelled";
            }
            else
            {
                //shouldnt get here as the mover.movefile should an throw exception if it fails and it wasnt cancelled but just incase
                ReturnMessage = "An error occurred";
            }
            return success;

        }

    }
}

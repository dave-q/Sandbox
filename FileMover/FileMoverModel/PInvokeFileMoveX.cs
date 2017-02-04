using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace FileMoverModel
{
    
    public class PInvokeFileMoveX
    {
        bool _cancelled;
        public Action<FileMoveEventArgs> ProgressCallback { get; set; }
        public string DestinationPath { get; private set; }
        public string SourcePath { get; private set; }

        public PInvokeFileMoveX(string sourcePath, string destinationPath, Action<FileMoveEventArgs> progressCallback)
        {
            this.DestinationPath = destinationPath;
            this.SourcePath = sourcePath;
            this.ProgressCallback = progressCallback;
            InitialiseCopyProgressRoutine();
        }

        private void InitialiseCopyProgressRoutine()
        {
            CopyProgressFunc =
            (
                TotalFileSize,
                TotalBytesTransferred,
                StreamSize, 
                StreamBytesTransferred,
                notUsed,
                CallbackReason,
                hSourceFile,
                hDestinationFile,
                lpData
            ) =>
            {
                var progressResult = CopyProgressResult.PROGRESS_CONTINUE;
                if (CallbackReason == CopyProgressCallbackReason.CALLBACK_CHUNK_FINISHED)
                {
                    var progress = CalculateProgress(TotalFileSize, TotalBytesTransferred);
                    var fileMoveEventArgs = new FileMoveEventArgs(progress);
                    ProgressCallback(fileMoveEventArgs);

                    if (fileMoveEventArgs.Cancelled)
                    {
                        progressResult = CopyProgressResult.PROGRESS_CANCEL;
                        _cancelled = true;
                    }
                   
                }
                return progressResult;
            };
        }
        delegate CopyProgressResult CopyProgressRoutine(long TotalFileSize, long TotalBytesTransferred, long StreamSize, long StreamBytesTransferred, uint notUsed, CopyProgressCallbackReason CallbackReason, IntPtr hSourceFile, IntPtr hDestinationFile, IntPtr lpData);

        public async Task<bool> MoveFile()
        {
            var success = await Task.Run<bool>(()=> MoveFileWithProgress(SourcePath, DestinationPath, new CopyProgressRoutine(CopyProgressFunc), IntPtr.Zero, MoveFileFlags.MOVE_FILE_COPY_ALLOWED | MoveFileFlags.MOVE_FILE_REPLACE_EXISTSING | MoveFileFlags.MOVE_FILE_WRITE_THROUGH));
            if (!success && !_cancelled)
            {
                var failedEventArgs = new FileMoveEventArgs(0M);
                ProgressCallback(failedEventArgs);
                ThrowLastWin32Exception();
            }
            else if (!success && _cancelled)
            {
                var cancelledEventArgs = new FileMoveEventArgs(0M);
                ProgressCallback(cancelledEventArgs);
            }
            else
            {
                //doing this as small files, never get called back from the Win32 method and hence the progress is never updated, so this is make sure 100% is return
                var completeEventArgs = new FileMoveEventArgs(100M);
                ProgressCallback(completeEventArgs);
            }
            return success;
        }

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern bool MoveFileWithProgress(
            string existingFileName, 
            string newFileName,
            CopyProgressRoutine progressRoutine, 
            IntPtr notNeeded, 
            MoveFileFlags CopyFlags);

        Func<long, long, long, long, uint, CopyProgressCallbackReason, IntPtr, IntPtr, IntPtr, CopyProgressResult> CopyProgressFunc;

        private decimal CalculateProgress(long totalSize, long transferredSize)
        {
            decimal progress = 0;
            if (totalSize > 0)
            {
                progress = ((decimal)transferredSize / (decimal)totalSize) * 100;
            }
            return progress;
        }


        private static void ThrowLastWin32Exception()
        {
            throw new Win32Exception(Marshal.GetLastWin32Error());
        }

        enum CopyProgressResult : uint
        {
            PROGRESS_CONTINUE = 0,
            PROGRESS_CANCEL = 1,
            PROGRESS_STOP = 2,
            PROGRESS_QUIET = 3
        }

        enum CopyProgressCallbackReason : uint
        {
            CALLBACK_CHUNK_FINISHED = 0x00000000,
            CALLBACK_STREAM_SWITCH = 0x00000001
        }

        [Flags]
        enum MoveFileFlags : uint
        {
            MOVE_FILE_REPLACE_EXISTSING = 0x00000001,
            MOVE_FILE_COPY_ALLOWED = 0x00000002,
            MOVE_FILE_DELAY_UNTIL_REBOOT = 0x00000004,
            MOVE_FILE_WRITE_THROUGH = 0x00000008,
            MOVE_FILE_CREATE_HARDLINK = 0x00000010,
            MOVE_FILE_FAIL_IF_NOT_TRACKABLE = 0x00000020
        }
        //delegate CopyProgressResult CopyProgressRoutine(
        //long TotalFileSize,
        //long TotalBytesTransferred,
        //long StreamSize,
        //long StreamBytesTransferred,
        //uint dwStreamNumber,
        //CopyProgressCallbackReason dwCallbackReason,
        //IntPtr hSourceFile,
        //IntPtr hDestinationFile,
        //IntPtr lpData);
    }
}

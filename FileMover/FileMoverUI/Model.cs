using FileMover;
using System;
using System.Threading.Tasks;

namespace FileMoverUI
{
    public class Model : ICancelled
    {
        public bool IsCancelled { get; set; }
        private bool _isMoving;

        public bool CanMove
        {
            get
            {
                return !_isMoving;
            }
        }

        public async Task<bool> MoveAsync(string sourcePath, string destinationPath, Action<decimal> updateProgressAsPct)
        {
            this.IsCancelled = false;
            this.UpdateProgressAsPct = updateProgressAsPct;
            try
            {
                _isMoving = true;
                return await FileWithProgress.MoveAsync(sourcePath, destinationPath, ProgressCallback, this);
            }
            finally
            {
                _isMoving = false;
            }
        }

        Action<decimal> UpdateProgressAsPct;

        public void ProgressCallback(long totalbytes, long transferredBytes)
        {
            decimal pctDone = 0;
            if (totalbytes > 0)
            {
                pctDone = ((decimal)transferredBytes / (decimal)totalbytes) * 100;
            }

            UpdateProgressAsPct(pctDone);
        }

        public void Cancel()
        {
            this.IsCancelled = true;
        }

        internal async Task<bool> CopyAsync(string sourcePath, string destinationPath, Action<decimal> updateProgressBar)
        {
            this.IsCancelled = false;
            this.UpdateProgressAsPct = updateProgressBar;
            try
            {
                _isMoving = true;
                return await FileWithProgress.CopyAsync(sourcePath, destinationPath, ProgressCallback, this);
            }
            finally
            {
                _isMoving = false;
            }
        }
    }
}

using FileMover;
using FileMoverModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileMover
{
    public class Model
    {
        public IFileMover FileMover { get; private set; }
        public Model(IFileMover fileMover)
        {
            FileMover = fileMover;
        }

        public async Task<ActionOutCome> MoveAsync(string sourcePath, string destinationPath, Action<decimal> UpdateProgress)
        {
            var fileMover = FileMover;
            fileMover.SourcePath = sourcePath;
            fileMover.DestinationPath = destinationPath;
            fileMover.ProgressUpdater = UpdateProgress;
            await fileMover.MoveAsync();
            return new ActionOutCome(fileMover.Success, fileMover.ReturnMessage);
        }

        public void Cancel()
        {
            if (FileMover.IsMoving)
            {
                FileMover.Cancel();
            }
        }


    }
}

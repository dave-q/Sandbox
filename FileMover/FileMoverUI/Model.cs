﻿using FileMover;
using FileMoverModel;
using System;
using System.Threading.Tasks;

namespace FileMoverUI
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
            var success = await fileMover.MoveAsync();
            return new ActionOutCome(success, fileMover.ReturnMessage);
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

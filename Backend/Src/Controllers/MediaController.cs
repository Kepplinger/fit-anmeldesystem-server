using Backend.Core.Contracts;
using Backend.Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using StoreService.Persistence;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json", "application/xml")]
    public class MediaController
    {
        private IUnitOfWork _unitOfWork;

        public MediaController(IUnitOfWork uow)
        {
            this._unitOfWork = uow;
        }

        [HttpGet]
        [Route("event/{id}")]
        [Authorize(Policy = "FitAdmin")]
        [Produces("application/zip")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public FileContentResult ZipArchive(int id)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");
            var configuration = builder.Build();
            string imagePath = configuration["ImageFilePaths:ServerImages"];
            string tmpDirectory = imagePath + "\\tmp";

            string[] directories = System.IO.Directory.GetDirectories(imagePath);

            if (System.IO.File.Exists(imagePath + "\\images.zip"))
                System.IO.File.Delete(imagePath + "\\images.zip");

            if (System.IO.Directory.Exists(tmpDirectory))
                System.IO.Directory.Delete(tmpDirectory, true);


            // Copy the files and overwrite destination files if they already exist.
            foreach (string directory in directories)
            {
                // Use static Path methods to extract only the file name from the path.
                string directoryName = System.IO.Path.GetFileName(directory);

                if (directoryName.StartsWith(id + "_"))
                {
                    string destDirectory = System.IO.Path.Combine(tmpDirectory, directoryName);
                    DirectoryCopy(directory, destDirectory, true);
                }
            }
            
            ZipFile.CreateFromDirectory(tmpDirectory, imagePath + "\\images.zip");

            FileContentResult result = new FileContentResult(System.IO.File.ReadAllBytes(imagePath + "\\images.zip"), "application/zip")
            {
                FileDownloadName = "images.zip"
            };

            return result;
        }

        private static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            DirectoryInfo[] dirs = dir.GetDirectories();
            // If the destination directory doesn't exist, create it.
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string temppath = Path.Combine(destDirName, file.Name);
                file.CopyTo(temppath, false);
            }

            // If copying subdirectories, copy them and their contents to new location.
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string temppath = Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, temppath, copySubDirs);
                }
            }
        }
    }
}

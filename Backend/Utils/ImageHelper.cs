using Backend.Core.Entities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Utils
{
    public class ImageHelper
    {

        /// <summary>
        /// My utils
        /// </summary>
        /// <param name="area"></param>
        /// <returns></returns>
        public string ImageParsing(Area area)
        {
            // parse from 64 String all image infos
            string dataFormat = String.Empty;
            int indexof = area.GraphicURL.IndexOf("base64,");
            string start = area.GraphicURL.Substring(0, indexof);
            string baseString = area.GraphicURL.Substring(indexof + 7);

            // Check image file format
            if (start.ToLower().Contains("png"))
                dataFormat = ".png";
            else if (start.ToLower().Contains("jpg"))
                dataFormat = ".jpg";
            else if (start.ToLower().Contains("jpeg"))
                dataFormat = ".jpeg";

            //Read filepath from appsetting.json
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");
            var configuration = builder.Build();
            string filepath = configuration["ImageFilePaths:SakalWindows"];

            string baseurl = configuration["Urls:DefaultUrl"];

            //set filepath name
            string filename = this.GetHash() + dataFormat;
            filepath = filepath + filename;

            // filepath
            //string filepath = @"C:\Users\andis\Desktop\Projects\fit-anmeldesystem-server\Backend\bin\Debug\netcoreapp2.0\images\" + area.Designation + dataFormat;

            // Save image to disk
            this.Base64ToImage(baseString, filepath);

            return filename;
        }
        public object Base64ToImage(string basestr, string filepath)
        {
            byte[] imageBytes = Convert.FromBase64String(basestr);
            //MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
            //ms.Write(imageBytes, 0, imageBytes.Length);
            using (var imageFile = new FileStream(filepath, FileMode.Create))
            {
                imageFile.Write(imageBytes, 0, imageBytes.Length);
                imageFile.Flush();
                return imageFile;
                //images/name.jpg
            }
        }
        public string GetHash()
        {
            return Guid.NewGuid().ToString();
        }
    }
}

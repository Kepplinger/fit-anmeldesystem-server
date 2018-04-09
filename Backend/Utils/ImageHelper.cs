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
        /// Imaage Utils (for parsing from base64 to image and from image to base64 string)
        /// </summary>
        /// <param name="area"></param>
        /// <returns></returns>
        public string ImageParsing(Area area)
        {
            // parse from 64 String all image infos
            string dataFormat = String.Empty;
            int indexof = area.GraphicUrl.IndexOf("base64,");
            string start = area.GraphicUrl.Substring(0, indexof);
            string baseString = area.GraphicUrl.Substring(indexof + 7);

            // Check image file format
            if (start.ToLower().Contains("png"))
                dataFormat = ".png";
            else if (start.ToLower().Contains("jpg"))
                dataFormat = ".jpg";
            else if (start.ToLower().Contains("jpeg"))
                dataFormat = ".jpeg";
            else if (start.ToLower().Contains("gif"))
                dataFormat = ".gif";

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

            return baseurl + "/images/" + filename;
        }
        public object Base64ToImage(string basestr, string filepath)
        {
            byte[] imageBytes = Convert.FromBase64String(basestr);
            using (var imageFile = new FileStream(filepath, FileMode.Create))
            {
                imageFile.Write(imageBytes, 0, imageBytes.Length);
                imageFile.Flush();
                return imageFile;
            }
        }
        public string GetHash()
        {
            return Guid.NewGuid().ToString();
        }


        public string BookingImages(Booking booking)
        {
            var builder = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json");
            var configuration = builder.Build();
            string filepath = configuration["ImageFilePaths:SakalWindows"];
            filepath = filepath + booking.Company.Name;
            string baseurl = configuration["Urls:DefaultUrl"];

            System.IO.Directory.CreateDirectory(filepath);


            int logoIndexOf = booking.Logo.IndexOf("base64,");
            string logoStart = booking.Logo.Substring(0, logoIndexOf);
            string logoBaseString = booking.Logo.Substring(logoIndexOf + 7);
            string logoDataFormat = checkDataFormat(logoStart);

            string logoFilePath = filepath + "companyLogo" + logoDataFormat;

            this.Base64ToImage(logoBaseString, logoFilePath);


            for (int i = 0; i < booking.Representatives.Count; i++)
            {
                int represIndexOf = booking.Representatives[i].ImageUrl.IndexOf("base64,");
                string represStart = booking.Representatives[i].ImageUrl.Substring(0, logoIndexOf);
                string represBaseString = booking.Representatives[i].ImageUrl.Substring(logoIndexOf + 7);
                string represDataFormat = checkDataFormat(represStart);
                string represFilePath = filepath + "contact" + Convert.ToString(i) + represDataFormat;
                this.Base64ToImage(represBaseString, represFilePath);
            }


            return baseurl + "/images/" + booking.Company.Name;
        }



        public string checkDataFormat(string start)
        {
            string dataFormat = String.Empty;
            if (start.ToLower().Contains("png"))
                dataFormat = ".png";
            else if (start.ToLower().Contains("jpg"))
                dataFormat = ".jpg";
            else if (start.ToLower().Contains("jpeg"))
                dataFormat = ".jpeg";
            else if (start.ToLower().Contains("gif"))
                dataFormat = ".gif";


            return dataFormat;
        }
    }
}

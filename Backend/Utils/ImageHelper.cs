using Backend.Core.Contracts;
using Backend.Core.Entities;
using Microsoft.Extensions.Configuration;
using StoreService.Persistence;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Utils
{
    public static class ImageHelper
    {
        /// <summary>
        /// Image Utils (for parsing from base64 to image and from image to base64 string)
        /// </summary>
        /// <param name="area"></param>
        /// <returns></returns>
        public static string ImageParsing(Area area)
        {
            // parse from 64 String all image infos
            int indexof = area.Graphic.DataUrl.IndexOf("base64,");
            string start = area.Graphic.DataUrl.Substring(0, indexof);
            string baseString = area.Graphic.DataUrl.Substring(indexof + 7);

            string dataFormat = getDataFormat(start);
            
            //Read filepath from appsetting.json
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");
            var configuration = builder.Build();
            string filepath = configuration["ImageFilePaths:ServerImages"];

            string baseurl = configuration["Urls:ServerUrl"];

            //set filepath name
            string filename = ImageHelper.GetHash() + dataFormat;
            filepath = filepath + filename;

            // filepath
            //string filepath = @"C:\Users\andis\Desktop\Projects\fit-anmeldesystem-server\Backend\bin\Debug\netcoreapp2.0\images\" + area.Designation + dataFormat;

            // Save image to disk
            ImageHelper.Base64ToImage(baseString, filepath);

            return baseurl + "/images/" + filename;
        }

        public static string BookingImages(Booking booking)
        {
            if (booking.Logo != null && booking.Logo.DataUrl != null && booking.Logo.DataUrl.Contains("base64,"))
            {
                Company company;

                using (IUnitOfWork uow = new UnitOfWork())
                {
                    company = uow.CompanyRepository.Get(comp => comp.Id == booking.fk_Company).FirstOrDefault();
                }

                // Create directory for company
                var builder = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json");
                var configuration = builder.Build();
                string filepath = configuration["ImageFilePaths:ServerImages"];
                filepath = filepath + company.Name;
                string baseurl = configuration["Urls:ServerUrl"];

                System.IO.Directory.CreateDirectory(filepath);
                
                int logoIndexOf = booking.Logo.DataUrl.IndexOf("base64,");
                string logoStart = booking.Logo.DataUrl.Substring(0, logoIndexOf);
                string logoBaseString = booking.Logo.DataUrl.Substring(logoIndexOf + 7);
                string logoDataFormat = getDataFormat(logoStart);

                string logoFilePath = filepath + "/companyLogo" + logoDataFormat;

                ImageHelper.Base64ToImage(logoBaseString, logoFilePath);
                
                for (int i = 0; i < booking.Representatives.Count; i++)
                {
                    int represIndexOf = booking.Representatives[i].Image.DataUrl.IndexOf("base64,");
                    string represStart = booking.Representatives[i].Image.DataUrl.Substring(0, represIndexOf);
                    string represBaseString = booking.Representatives[i].Image.DataUrl.Substring(represIndexOf + 7);
                    string represDataFormat = getDataFormat(represStart);
                    string represFilePath = filepath + "/contact" + Convert.ToString(i) + represDataFormat;
                    ImageHelper.Base64ToImage(represBaseString, represFilePath);
                }
                
                return baseurl + "/images/" + company.Name;
            }
            else return "noImageAdded";
        }

        private static string GetHash()
        {
            return Guid.NewGuid().ToString();
        }

        private static object Base64ToImage(string basestr, string filepath)
        {
            byte[] imageBytes = Convert.FromBase64String(basestr);
            using (var imageFile = new FileStream(filepath, FileMode.Create))
            {
                imageFile.Write(imageBytes, 0, imageBytes.Length);
                imageFile.Flush();
                return imageFile;
            }
        }

        private static string getDataFormat(string start)
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

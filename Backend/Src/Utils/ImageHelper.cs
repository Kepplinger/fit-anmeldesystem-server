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
        public static string ManageAreaGraphic(DataFile image)
        {
            //Read filepath from appsetting.json
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");
            var configuration = builder.Build();

            string filepath = configuration["ImageFilePaths:ServerImages"];
            string baseurl = configuration["Urls:ServerUrl"];
            
            return baseurl + "/images/" + ManageImage(image, filepath);
        }

        public static void ManageBookingImages(Booking booking)
        {
            // Create directory for company
            var builder = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json");
            var configuration = builder.Build();

            string baseUrl = configuration["Urls:ServerUrl"] + "\\images";
            string filePath = configuration["ImageFilePaths:ServerImages"];

            using (IUnitOfWork uow = new UnitOfWork())
            {
                Company company = uow.CompanyRepository.Get(comp => comp.Id == booking.fk_Company).FirstOrDefault();
                filePath = filePath + company.Name;
                baseUrl = baseUrl + "\\" + company.Name;
            }

            System.IO.Directory.CreateDirectory(filePath);

            if (booking.Logo != null && booking.Logo.DataUrl != null && booking.Logo.DataUrl.Contains("base64,"))
            {
                booking.Logo.DataUrl = baseUrl + "\\" + ManageImage(booking.Logo, filePath);
            }

            for (int i = 0; i < booking.Representatives.Count; i++)
            {
                if (booking.Representatives[i].Image != null && booking.Representatives[i].Image.DataUrl != null && booking.Representatives[i].Image.DataUrl.Contains("base64,"))
                {
                    booking.Representatives[i].Image.DataUrl = baseUrl + "\\" + ManageImage(booking.Representatives[i].Image, filePath);
                }
            }
        }

        private static string ManageImage(DataFile image, string filePath)
        {
            int base64Index = image.DataUrl.IndexOf("base64,");

            string descriptionPart = image.DataUrl.Substring(0, base64Index);
            string base64Part = image.DataUrl.Substring(base64Index + 7);

            string fileName = ImageHelper.GetHash() + getDataFormat(descriptionPart);

            ImageHelper.Base64ToImage(base64Part, filePath + "\\" + fileName);

            return fileName;
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

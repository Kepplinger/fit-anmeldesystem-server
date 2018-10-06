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
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Backend.Utils {
    public static class ImageHelper {
        /// <summary>
        /// Image Utils (for parsing from base64 to image and from image to base64 string)
        /// </summary>
        /// <param name="area"></param>
        /// <returns></returns>
        public static void ManageEventGraphic(Event fitEvent) {
            //Read filepath from appsetting.json
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");
            var configuration = builder.Build();

            string filePath = configuration["ImageFilePaths:ServerImages"] + "\\EVENT_" + fitEvent.Id;
            string baseurl = configuration["Urls:ServerUrl"] + "\\images\\EVENT_" + fitEvent.Id;

            System.IO.Directory.CreateDirectory(filePath);

            foreach (Area area in fitEvent.Areas) {
                if (area.Graphic != null && area.Graphic.DataUrl != null) {
                    if (area.Graphic.DataUrl.Contains("base64,")) {
                        area.Graphic.DataUrl = baseurl + "\\" + ManageImage(area.Graphic, filePath);
                    } else if (!area.Graphic.DataUrl.Contains("EVENT_" + fitEvent.Id)) {
                        int index = area.Graphic.DataUrl.IndexOf("\\EVENT_");

                        string oldImage = area.Graphic.DataUrl.Substring(index);
                        string newImage = Regex.Replace(oldImage, @"EVENT_.+\\", "EVENT_" + fitEvent.Id + "\\");
                        System.IO.File.Copy(
                            configuration["ImageFilePaths:ServerImages"] + oldImage,
                            configuration["ImageFilePaths:ServerImages"] + newImage,
                            true);
                        area.Graphic.DataUrl = configuration["Urls:ServerUrl"] + "\\images" + newImage;
                    }

                }
            }

            CleanEventDirectoryOfOldFiles(filePath, fitEvent);
        }

        public static void ManageBookingFiles(Booking booking) {
            // Create directory for company
            var builder = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json");
            var configuration = builder.Build();

            string baseUrl = configuration["Urls:ServerUrl"] + "\\images";
            string filePath = configuration["ImageFilePaths:ServerImages"];

            using (IUnitOfWork uow = new UnitOfWork()) {
                Company company = uow.CompanyRepository.Get(comp => comp.Id == booking.fk_Company).FirstOrDefault();
                filePath = filePath + "\\" + booking.fk_Event + "_" + company.Name;
                baseUrl = baseUrl + "\\" + booking.fk_Event + "_" + company.Name;
            }

            System.IO.Directory.CreateDirectory(filePath);

            if (booking.Logo != null && booking.Logo.DataUrl != null && booking.Logo.DataUrl.Contains("base64,")) {
                booking.Logo.DataUrl = baseUrl + "\\" + ManageImage(booking.Logo, filePath);
            }

            for (int i = 0; i < booking.Representatives.Count; i++) {
                if (booking.Representatives[i].Image != null && booking.Representatives[i].Image.DataUrl != null && booking.Representatives[i].Image.DataUrl.Contains("base64,")) {
                    booking.Representatives[i].Image.DataUrl = baseUrl + "\\" + ManageImage(booking.Representatives[i].Image, filePath);
                }
            }

            if (booking.Presentation != null && booking.Presentation.File != null && booking.Presentation.File.DataUrl.Contains("base64,")) {
                booking.Presentation.File.DataUrl = baseUrl + "\\" + ManageImage(booking.Presentation.File, filePath);
            }

            CleanBookingDirectoryOfOldFiles(filePath, booking);
        }

        private static string ManageImage(DataFile image, string filePath) {
            int base64Index = image.DataUrl.IndexOf("base64,");

            string descriptionPart = image.DataUrl.Substring(0, base64Index + 7);
            string base64Part = image.DataUrl.Substring(base64Index + 7);

            string fileName = ImageHelper.GetHash() + getDataFormat(descriptionPart);

            ImageHelper.Base64ToImage(base64Part, filePath + "\\" + fileName);

            return fileName;
        }

        private static void CleanBookingDirectoryOfOldFiles(string filePath, Booking booking) {
            foreach (string file in Directory.EnumerateFiles(filePath)) {
                int index = file.LastIndexOf('\\');
                string fileName = file.Substring(index);
                bool found = false;

                if (booking.Logo != null && booking.Logo.DataUrl != null && booking.Logo.DataUrl.Contains(fileName)) {
                    found = true;
                }

                if (!found && booking.Presentation != null &&
                    booking.Presentation.File != null &&
                    booking.Presentation.File.DataUrl.Contains(fileName)) {
                    found = true;
                }

                if (!found) {
                    for (int i = 0; i < booking.Representatives.Count; i++) {
                        if (booking.Representatives[i].Image != null &&
                            booking.Representatives[i].Image.DataUrl != null &&
                            booking.Representatives[i].Image.DataUrl.Contains(fileName)) {
                            found = true;
                        }
                    }
                }

                if (!found) {
                    File.Delete(file);
                }
            }
        }

        private static void CleanEventDirectoryOfOldFiles(string filePath, Event fitEvent) {
            foreach (string file in Directory.EnumerateFiles(filePath)) {
                int index = file.LastIndexOf('\\');
                string fileName = file.Substring(index);
                bool found = false;

                for (int i = 0; i < fitEvent.Areas.Count; i++) {
                    if (fitEvent.Areas[i].Graphic != null &&
                        fitEvent.Areas[i].Graphic.DataUrl != null &&
                        fitEvent.Areas[i].Graphic.DataUrl.Contains(fileName)) {
                        found = true;
                    }
                }

                if (!found) {
                    File.Delete(file);
                }
            }
        }

        private static string GetHash() {
            return Guid.NewGuid().ToString();
        }

        private static object Base64ToImage(string basestr, string filepath) {
            byte[] imageBytes = Convert.FromBase64String(basestr);
            using (var imageFile = new FileStream(filepath, FileMode.Create)) {
                imageFile.Write(imageBytes, 0, imageBytes.Length);
                imageFile.Flush();
                return imageFile;
            }
        }

        private static string getDataFormat(string start) {
            Match match = Regex.Match(start, "data:(application|image|text)\\/(.+);base64,");
            return "." + match.Groups.Last().Value;
        }
    }
}

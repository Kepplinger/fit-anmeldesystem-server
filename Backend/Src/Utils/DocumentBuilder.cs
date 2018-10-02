using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using System;
using System.Collections.Generic;
using System.Text;
using iText.Kernel.Pdf.Canvas.Draw;
using iText.Layout.Properties;
using iText.Kernel.Font;
using iText.Layout.Borders;
using System.IO;
using Backend.Core.Entities;
using iText.Kernel.Colors;
using Backend.Core.Contracts;
using StoreService.Persistence;

namespace Backend.Utils {
    public class DocumentBuilder {
        private char cchecked = '\u221A';
        private char unccecked = '\u00A8';
        public const string DOCUMENT_DESTINATION = "./Pdfs/";

        private PdfWriter writer;

        public DocumentBuilder() {
            if (!Directory.Exists(DOCUMENT_DESTINATION)) {
                Directory.CreateDirectory(DOCUMENT_DESTINATION);
            }
        }

        public string CreatePdfOfBooking(Booking booking) {
            Color myColor = new DeviceRgb(0, 123, 255);

            string[] internationalArray = booking.EstablishmentsInt.Split(';');
            string[] autArray = booking.EstablishmentsAut.Split(';');

            string beautyInternational = "";
            string beautyNational = "";

            foreach (string tmp in internationalArray) {
                beautyInternational = beautyInternational + tmp + " ";
            }

            foreach (string tmp in autArray) {
                beautyNational = beautyNational + tmp + " ";
            }
            Company company = new Company();
            using (IUnitOfWork uow = new UnitOfWork()) {
                company = uow.CompanyRepository.GetById(booking.fk_Company);
            }

            string file = DOCUMENT_DESTINATION + $"FIT-Anmeldung_{company.Name}_{DateTime.Now.ToString("ddMMyyyy")}.pdf";
            using (IUnitOfWork uow = new UnitOfWork()) {
                Booking boo = uow.BookingRepository.GetById(booking.Id);
                boo.PdfFilePath = file;
                uow.BookingRepository.Update(boo);
                uow.Save();
            }

            try {
                writer = new PdfWriter(file);
            } catch (IOException) { }


            PdfDocument pdf = new PdfDocument(writer);
            pdf.GetDocumentInfo()
                .SetAuthor($"FIT Anmelde-System")
                .SetCreator("FIT Anmelde-System")
                .SetTitle($"Anmeldung zum FIT");

            string fontPath = "./ImagesPdf/Roboto-Regular.ttf";

            PdfFont font = PdfFontFactory.CreateFont(fontPath, iText.IO.Font.PdfEncodings.IDENTITY_H, true);

            // Initialize document
            Document document = new Document(pdf);

            Image imageHeader = new Image(iText.IO.Image.ImageDataFactory.Create("./ImagesPdf/Header.png"));
            imageHeader.ScaleToFit(595, 80);
            imageHeader.SetFixedPosition(0, 785);

            Image imageFooter = new Image(iText.IO.Image.ImageDataFactory.Create("./ImagesPdf/Footer.png"));
            imageFooter.ScaleToFit(595, 90);
            imageFooter.SetFixedPosition(0, 0);

            document.Add(imageHeader);
            document.Add(imageFooter);

            document.Add(
                new Paragraph(booking.Company.Name)
            .SetTextAlignment(TextAlignment.CENTER)
            .SetFontSize(30)
            .SetBold()
                .SetFont(font).SetMarginTop(50)
            );

            //Add paragraph to the document
            document.Add(
                new Paragraph("Buchungsübersicht zum FIT")
                .SetTextAlignment(TextAlignment.CENTER)
                .SetFontSize(20)
                .SetBold()
                .SetFont(font)
            );

            Cell cell = new Cell();
            Table table = new Table(2);
            table.UseAllAvailableWidth();
            cell.Add(new Paragraph("Stammdaten").SetBold());
            cell.SetBackgroundColor(myColor);
            cell.SetFontColor(iText.Kernel.Colors.ColorConstants.WHITE);
            table.AddCell(cell);
            cell = new Cell();
            cell.Add(new Paragraph("").SetBold());
            cell.SetBackgroundColor(myColor);
            cell.SetFontColor(iText.Kernel.Colors.ColorConstants.WHITE);
            table.AddCell(cell);
            cell = new Cell();

            table.SetBackgroundColor(iText.Kernel.Colors.ColorConstants.WHITE);
            table.SetFontColor(iText.Kernel.Colors.ColorConstants.BLACK);
            table.AddCell("Firmen-Name");
            table.AddCell(booking.Company.Name);
            table.AddCell("Ihr gewähltes Paket");
            table.AddCell(booking.FitPackage.Name + " - " + booking.FitPackage.Price + ",00 €");

            if (booking.Location != null) {
                table.AddCell("Ihr Standplatz");
                table.AddCell(booking.Location.Number);
            }

            cell = new Cell(2, 1);
            cell.Add(new Paragraph("Adresse"));
            table.AddCell(cell);

            cell = new Cell(2, 1);
            cell.Add(new Paragraph(booking.Company.Address.Street + " " + booking.Company.Address.StreetNumber + "\n"
                + booking.Company.Address.ZipCode + " " + booking.Company.Address.City));
            table.AddCell(cell);

            table.AddCell("Kontaktperson");
            table.AddCell(booking.Company.Contact.FirstName + " " + booking.Company.Contact.LastName);
            table.AddCell(" - Telefonnummer");
            table.AddCell(booking.Company.Contact.PhoneNumber);
            table.AddCell(" - Email");
            table.AddCell(booking.Company.Contact.Email);
            document.Add(table);

            table = new Table(2);
            table.UseAllAvailableWidth();
            table.SetMarginTop(10);
            cell = new Cell();
            cell.Add(new Paragraph("Buchungsdaten").SetBold());
            cell.SetBackgroundColor(myColor);
            cell.SetFontColor(iText.Kernel.Colors.ColorConstants.WHITE);
            table.AddCell(cell);
            cell = new Cell();
            cell.Add(new Paragraph("").SetBold());
            cell.SetBackgroundColor(myColor);
            cell.SetFontColor(iText.Kernel.Colors.ColorConstants.WHITE);
            table.AddCell(cell);
            cell = new Cell();

            Image imageYes = new Image(iText.IO.Image.ImageDataFactory.Create("./ImagesPdf/checkedcheckbox.png"));
            imageYes.ScaleToFit(15, 15);
            Image imageNo = new Image(iText.IO.Image.ImageDataFactory.Create("./ImagesPdf/checkboxemptry.png"));
            imageNo.ScaleToFit(18, 18);


            table.AddCell("Firmen Telefonnummer");
            table.AddCell(booking.PhoneNumber);
            table.AddCell("Firmen Email");
            table.AddCell(booking.Email);
            table.AddCell("Firmen Homepage");
            table.AddCell(booking.Homepage);
            table.AddCell("Branche");
            table.AddCell(booking.Branch);

            table.AddCell("Standorte Österreich");

            if (booking.EstablishmentsCountAut == 0) {
                table.AddCell(booking.EstablishmentsCountAut.ToString());
            } else {
                table.AddCell(booking.EstablishmentsCountAut + ", " + beautyNational);
            }

            table.AddCell("Standorte International");

            if (booking.EstablishmentsCountInt == 0) {
                table.AddCell(booking.EstablishmentsCountInt.ToString());
            } else {
                table.AddCell(booking.EstablishmentsCountInt + ", " + beautyInternational);
            }

            int respresentativeCount = booking.Representatives.Count;

            if (respresentativeCount > 0) {
                cell = new Cell(respresentativeCount, 1);
                cell.Add(new Paragraph("Vertreter"));
                table.AddCell(cell);

                foreach (Representative representative in booking.Representatives) {
                    table.AddCell(" • " + representative.Name);
                }
            }

            table.AddCell("Sie vergeben Praktika?");
            if (booking.ProvidesSummerJob)
                table.AddCell(imageYes.SetHorizontalAlignment(HorizontalAlignment.CENTER));
            else
                table.AddCell(imageNo.SetHorizontalAlignment(HorizontalAlignment.CENTER));

            table.AddCell("Sie vergeben Diplomarbeiten?");
            if (booking.ProvidesThesis)
                table.AddCell(imageYes.SetHorizontalAlignment(HorizontalAlignment.CENTER));
            else
                table.AddCell(imageNo.SetHorizontalAlignment(HorizontalAlignment.CENTER));

            int branchCount = booking.Branches.Count;

            if (branchCount > 0) {
                cell = new Cell(branchCount, 1);
                cell.Add(new Paragraph("Branchen"));
                table.AddCell(cell);

                foreach (BookingBranch branch in booking.Branches) {
                    table.AddCell(" • " + branch.Branch.Name);
                }
            }

            int resourceCount = booking.Resources.Count;

            if (resourceCount > 0) {
                cell = new Cell(branchCount, 1);
                cell.Add(new Paragraph("Sie benötigen ..."));
                table.AddCell(cell);

                foreach (ResourceBooking resource in booking.Resources) {
                    table.AddCell(" • " + resource.Resource.Name);
                }
            }

            document.Add(table);

            table = new Table(2);
            table.UseAllAvailableWidth();
            table.SetMarginTop(10);
            cell = new Cell();
            cell.Add(new Paragraph("Kontaktperson für den Fit").SetBold());
            cell.SetBackgroundColor(myColor);
            cell.SetFontColor(iText.Kernel.Colors.ColorConstants.WHITE);
            table.AddCell(cell);
            cell = new Cell();
            cell.Add(new Paragraph("").SetBold());
            cell.SetBackgroundColor(myColor);
            cell.SetFontColor(iText.Kernel.Colors.ColorConstants.WHITE);
            table.AddCell(cell);
            cell = new Cell();

            string gender = booking.Company.Contact.Gender == "M" ? "Herr" : "Frau";

            table.AddCell("Name");
            table.AddCell(gender + " " + booking.Company.Contact.FirstName + " " + booking.Company.Contact.LastName);
            table.AddCell("Email");
            table.AddCell(booking.Company.Contact.Email);
            table.AddCell("Telefonnummer");
            table.AddCell(booking.Company.Contact.PhoneNumber);
            document.Add(table);

            if (booking.Presentation != null) {
                table = new Table(2);
                table.UseAllAvailableWidth();
                table.SetMarginTop(10);
                cell.Add(new Paragraph("Vortrag").SetBold());
                cell.SetBackgroundColor(myColor);
                cell.SetFontColor(iText.Kernel.Colors.ColorConstants.WHITE);
                table.AddCell(cell);
                cell = new Cell();
                cell.Add(new Paragraph("").SetBold());
                cell.SetBackgroundColor(myColor);
                cell.SetFontColor(iText.Kernel.Colors.ColorConstants.WHITE);
                table.AddCell(cell);
                cell = new Cell();

                table.AddCell("Titel");
                table.AddCell(booking.Presentation.Title);
                table.AddCell("Beschreibung");
                table.AddCell(booking.Presentation.Description);

                if (booking.Presentation.File != null) {
                    table.AddCell("Datei");
                    table.AddCell(booking.Presentation.File.Name);
                }
                
                document.Add(table);
            }

            document.Close();

            // ignored by
            Table topTable = new Table(UnitValue.CreatePercentArray(new float[] { 100f }), true);

            Table nameAndClassTable = new Table(2, true);
            nameAndClassTable.AddCell(
                new CustomBorderCell() { LeftBorder = Border.NO_BORDER, RightBorder = Border.NO_BORDER }
                .Add(
                    new Paragraph()
                    .Add(new Text("Firmenname"))
                    .SetFontSize(8)
                    .SetBold()
                    .SetFont(font)
                )
            );
            nameAndClassTable.AddCell(
                new CustomBorderCell() { LeftBorder = Border.NO_BORDER, RightBorder = Border.NO_BORDER }
                .Add(
                    new Paragraph()
                    .Add(new Text("Paket"))
                    .SetFontSize(8)
                    .SetBold()
                    .SetTextAlignment(TextAlignment.RIGHT)
                    .SetFont(font)
                )
            );


            nameAndClassTable.AddCell(new CustomBorderCell(CustomBorderCell.CustomBorderType.RIDGE_BORDER) { LeftBorder = Border.NO_BORDER, RightBorder = Border.NO_BORDER }.Add(
            new Paragraph()
                .Add(new Text(booking.Company.Name))
                .SetFontSize(14)
                .SetFont(font)
            ));
            nameAndClassTable.AddCell(new CustomBorderCell(CustomBorderCell.CustomBorderType.RIDGE_BORDER) { LeftBorder = Border.NO_BORDER, RightBorder = Border.NO_BORDER }.Add(
            new Paragraph()
                .Add(new Text(booking.Branch))
                .SetTextAlignment(TextAlignment.RIGHT)
                .SetFontSize(14)
                .SetFont(font)
            ));

            topTable.AddCell(new CustomBorderCell(CustomBorderCell.CustomBorderType.THIN_BORDER, isTop: true).Add(nameAndClassTable));

            //Close document
            document.Close();

            return file;
        }
    }

    public class CustomBorderCell : Cell {
        public readonly static Border THICK_BORDER = new SolidBorder(3);
        public readonly static Border THIN_BORDER = new SolidBorder(1);
        public readonly static Border RIDGE_BORDER = new RidgeBorder(1);


        private Border topBorder;
        private Border bottomBorder;
        private Border rightBorder;
        private Border leftBorder;

        public Border LeftBorder {
            get { return leftBorder; }
            set { leftBorder = value; SetBorderLeft(leftBorder); }
        }


        public Border RightBorder {
            get { return rightBorder; }
            set { rightBorder = value; SetBorderRight(rightBorder); }
        }


        public Border BottomBorder {
            get { return bottomBorder; }
            set { bottomBorder = value; SetBorderBottom(bottomBorder); }
        }


        public Border TopBorder {
            get { return topBorder; }
            set { topBorder = value; SetBorderTop(topBorder); }
        }



        public CustomBorderCell() {
            initBorders();
        }

        public CustomBorderCell(
            CustomBorderType customBorder = CustomBorderType.NO_BORDER,
            bool isTop = false,
            bool isBottom = false) {
            initBorders(customBorder, isTop, isBottom);
        }

        public CustomBorderCell(
            int rowspan,
            int colspan,
            CustomBorderType customBorder = CustomBorderType.NO_BORDER,
            bool isTop = false,
            bool isBottom = false) : base(rowspan, colspan) {
            initBorders(customBorder, isTop, isBottom);
        }

        private void initBorders(
            CustomBorderType borderType = CustomBorderType.NO_BORDER,
            bool isTop = false,
            bool isBottom = false) {
            Border leftRight = THICK_BORDER;
            Border topBottomT1 = Border.NO_BORDER;
            Border topBottomT2 = THIN_BORDER;
            Border topBottomT3 = RIDGE_BORDER;

            Border topBottom = null;

            switch (borderType) {
                case CustomBorderType.NO_BORDER:
                    topBottom = topBottomT1;
                    break;
                case CustomBorderType.THIN_BORDER:
                    topBottom = topBottomT2;
                    break;
                case CustomBorderType.RIDGE_BORDER:
                    topBottom = topBottomT3;
                    break;
                case CustomBorderType.THICK_BORDER:
                    topBottom = leftRight;
                    break;
                default:
                    topBottom = leftRight;
                    break;
            }

            SetBorderTop(isTop ? leftRight : topBottom);
            SetBorderBottom(isBottom ? leftRight : topBottom);
            SetBorderLeft(leftRight);
            SetBorderRight(leftRight);
        }

        public enum CustomBorderType {
            NO_BORDER,
            THIN_BORDER,
            RIDGE_BORDER,
            THICK_BORDER
        }
    }
}
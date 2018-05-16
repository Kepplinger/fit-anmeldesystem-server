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

namespace Backend.Utils
{
    public class DocumentBuilder
    {
        private char cchecked ='\u221A';
        private char unccecked ='\u00A8';
        public const string DOCUMENT_DESTINATION = "./Pdfs/";
    
        private PdfWriter writer;

        public DocumentBuilder()
        {
            if (!Directory.Exists(DOCUMENT_DESTINATION))
            {
                Directory.CreateDirectory(DOCUMENT_DESTINATION);
            }
        }

        public string CreatePdfOfBooking(Booking booking)
        {
            Color myColor = new DeviceRgb(0, 123, 255);

            string[] internationalArray = booking.EstablishmentsInt.Split(';');
            string[] autArray = booking.EstablishmentsAut.Split(';');

            string beautyInternational = "";
            string beautyNational = "";

            foreach (string tmp in internationalArray)
            {
                beautyInternational = beautyInternational + tmp + " ";
            }

            foreach(string tmp in autArray){
                beautyNational = beautyNational + tmp + " ";
            }
            Company company = new Company();
            using (IUnitOfWork uow = new UnitOfWork())
            {
                company = uow.CompanyRepository.GetById(booking.fk_Company);
            }



                string file = DOCUMENT_DESTINATION + $"FIT-Anmeldung_{company.Name}_{DateTime.Now.ToString("ddMMyyyy")}.pdf";
            using (IUnitOfWork uow = new UnitOfWork())
            {
                Booking boo = uow.BookingRepository.GetById(booking.Id);
                boo.PdfFilePath = file;
                uow.BookingRepository.Update(boo);
                uow.Save();
            }
                writer = new PdfWriter(file);

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
            imageHeader.ScaleToFit(595,80);
            imageHeader.SetFixedPosition(0,785);

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
            float[] fuck = { 0, 123, 255, 100 };
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
                table.AddCell("Firmen Name");
                table.AddCell(booking.Company.Name);
            table.AddCell("Ihr gewähltes Paket");
            table.AddCell(booking.FitPackage.Name);
            table.AddCell("Ihr Standplatz");
            table.AddCell(booking.Location.Number);
            table.AddCell("Adresse");
            table.AddCell(booking.Company.Address.Street + "." + booking.Company.Address.StreetNumber + ", " + booking.Company.Address.ZipCode + " " + booking.Company.Address.City);

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
            table.AddCell(booking.EstablishmentsCountAut + ", " + beautyNational);
            table.AddCell("Standorte International");
            table.AddCell(booking.EstablishmentsCountInt + ", " + beautyInternational);

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

            foreach(BookingBranches branch in booking.Branches){
                table.AddCell("Sie vergeben "+branch.Branch.Name+"?");
                    table.AddCell(imageYes.SetHorizontalAlignment(HorizontalAlignment.CENTER));
            }

      


            document.Add(table);

            table = new Table(2);
            table.UseAllAvailableWidth();
            table.SetMarginTop(10);
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

            table.AddCell("Name");
            table.AddCell(booking.Company.Contact.Gender + " " + booking.Company.Contact.FirstName + " " + booking.Company.Contact.LastName);
            table.AddCell("Email");
            table.AddCell(booking.Company.Contact.Email);
            table.AddCell("Telefonnummer");
            table.AddCell(booking.Company.Contact.PhoneNumber);
            document.Add(table);

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


            nameAndClassTable.AddCell(new CustomBorderCell(CustomBorderCell.CustomBorderType.RIDGE_BORDER)
            { LeftBorder = Border.NO_BORDER, RightBorder = Border.NO_BORDER }.Add(
            new Paragraph()
                .Add(new Text(booking.Company.Name))
                .SetFontSize(14)
                .SetFont(font)
            ));
            nameAndClassTable.AddCell(new CustomBorderCell(CustomBorderCell.CustomBorderType.RIDGE_BORDER)
            { LeftBorder = Border.NO_BORDER, RightBorder = Border.NO_BORDER }.Add(
            new Paragraph()
                .Add(new Text(booking.Branch))
                .SetTextAlignment(TextAlignment.RIGHT)
                .SetFontSize(14)
                .SetFont(font)
            ));

            topTable.AddCell(new CustomBorderCell(CustomBorderCell.CustomBorderType.THIN_BORDER, isTop: true).Add(nameAndClassTable));


            /*topTable.AddCell(new CustomBorderCell(CustomBorderCell.CustomBorderType.THIN_BORDER).Add(
                new Paragraph("Ich melde mich zur Semesterprüfung aus")
                .SetFontSize(12)
                .SetMarginTop(10)
                .SetFont(font)
                ));


            topTable.AddCell(new CustomBorderCell(CustomBorderCell.CustomBorderType.RIDGE_BORDER).Add(
                new Paragraph()
                .Add(new Tab())
                .Add(new Text("Gegenstand: ")
                    .SetFontSize(10)
                    .SetFont(font))
                .Add(new Tab())
                .Add(new Text(exam.Subject.SubjectName)
                    .SetFont(font)
                    .SetFontSize(14))
                ));


            topTable.AddCell(new CustomBorderCell().Add(
                new Paragraph()
                .Add(new Tab())
                .Add(new Text("PrüferIn"))
                .SetFontSize(10)
                .SetFont(font)
                ));

            topTable.AddCell(new CustomBorderCell(CustomBorderCell.CustomBorderType.RIDGE_BORDER).Add(
                new Paragraph()
                .Add(new Tab())
                .Add(new Text(exam.Teacher.DisplayName))
                .SetFontSize(14)
                .SetFont(font)
                ));

            topTable.AddCell(new CustomBorderCell(CustomBorderCell.CustomBorderType.RIDGE_BORDER).Add(
                new Paragraph()
                .Add(new Tab())
                .Add(new Text("Semester (der abzulegenden Prüfung):")
                    .SetFontSize(10)
                    .SetFont(font))
                .Add(new Tab())
                .Add(new Text($"{exam.FailedTerm}. Semester")
                    .SetFontSize(14)
                    .SetFont(font))
                ));

            topTable.AddCell(new CustomBorderCell(CustomBorderCell.CustomBorderType.RIDGE_BORDER).Add(
                new Paragraph()
                .Add(new Tab())
                .Add(new Text("Zeugnisdatum (des negativen Semesters):")
                    .SetFontSize(10)
                    .SetFont(font))
                .Add(new Tab())
                .Add(new Text(exam.SemesterEnd.SemesterEndDate.ToString("dd.MM.yyyy"))
                    .SetFontSize(14)
                    .SetFont(font))
                ));

            topTable.AddCell(new CustomBorderCell(CustomBorderCell.CustomBorderType.RIDGE_BORDER).Add(
                new Paragraph()
                .Add(new Tab())
                .Add(new Text("Datum der Semesterprüfung:")
                    .SetFontSize(10)
                    .SetFont(font))
                .Add(new Tab())
                .Add(new Text(exam.Appointment.ExamAppointmentDate.ToString("dd.MM.yyyy"))
                    .SetFontSize(14)
                    .SetFont(font))
                ));

            topTable.AddCell(new CustomBorderCell()
            {
                BottomBorder = CustomBorderCell.THIN_BORDER
            }.Add(
                new Paragraph("an.")
                    .SetFontSize(12)
                .SetFont(font)
                ));



            var p = new Paragraph()
                .SetFontSize(12)
                .SetFont(font);


            p.Add(new Text("Es handelt sich bei der Prüfung um die "));

            switch (exam.Repetition)
            {
                case 0:
                    p.Add(new Text("Semesterprüfung").SetBold());
                    break;
                default:
                    p.Add(new Text($"{exam.Repetition}. Wiederholung").SetBold());
                    break;
            }
            p.Add(new Text("."));

            topTable.AddCell(new CustomBorderCell().Add(p));

            topTable.AddCell(new CustomBorderCell().Add(
                new Paragraph("Ein Nicht - Antritt führt zu einem Terminverlust (eine Verhinderung ist dann gerechtfertigt, wenn eine entsprechende Bestätigung (Gericht, Behörde, Arzt usw.) der Abteilungsadministration unverzüglich vorgelegt wird)!")
                .SetFontSize(10).SetMarginTop(5)));


            Table footerTable = new Table(UnitValue.CreatePercentArray(new float[] { 20f, 80f }), true);
            footerTable.SetBorder(Border.NO_BORDER);
            footerTable.AddCell(new Cell()
                .Add(new Paragraph(DateTime.Now.ToShortDateString()))
                .Add(new Paragraph()
                        .Add(new Text("Datum")
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetFontSize(6)))
                .SetBorder(Border.NO_BORDER)
                .SetTextAlignment(TextAlignment.CENTER)
                .SetPaddingTop(17));

            footerTable.AddCell(new Cell().SetBorder(Border.NO_BORDER).SetPaddingTop(20)
                .Add(new Paragraph().Add(new LineSeparator(new SolidLine()).SetWidth(UnitValue.CreatePercentValue(100))))
                .Add(new Paragraph().Add(new Text("Unterschrift des Erziehungsberechtigten bzw. des volljährigen Schülers/der volljährigen Schülerin")
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetFontSize(6))));


            topTable.AddCell(new CustomBorderCell(CustomBorderCell.CustomBorderType.THIN_BORDER, isBottom: true).Add(footerTable));

            document.Add(topTable);

            topTable.Complete();

            AddFooter(document, font, exam.Student);
            */
            //Close document
            document.Close();

            return file;
        }
        /*
        public string CreatePdfForExamAppointment(ExamAppointment examAppointment, List<Exam> exams)
        {
            string file = $"{DOCUMENT_DESTINATION}Pruefungen_{examAppointment.ExamAppointmentDate.ToString("ddMMyyyy")}.pdf";
            writer = new PdfWriter(file);

            PdfDocument pdf = new PdfDocument(writer);
            pdf.GetDocumentInfo()
                .SetAuthor("HTL Leonding")
                .SetCreator("NOST Semesterprüfungs-Anmeldesystem")
                .SetTitle($"Anmeldung zur Semesterprüfung");

            string fontPath = "./Roboto-Regular.ttf";

            PdfFont font = PdfFontFactory.CreateFont(fontPath, iText.IO.Font.PdfEncodings.IDENTITY_H, true);

            // Initialize document
            Document document = new Document(pdf);

            int cnt = 0;
            foreach (Exam item in exams)
            {
                createPageForExamAppointment(item, font, document);
                cnt++;
                if (cnt != exams.Count)
                {
                    document.Add(new AreaBreak());
                }
            }

            //Close document
            document.Close();

            return file;
        }
        /*
        private void createPageForExamAppointment(Exam exam, PdfFont font, Document document)
        {
            document.Add(
                new Paragraph("Semesterprüfung")
                .SetTextAlignment(TextAlignment.CENTER)
                .SetFontSize(24)
                .SetBold()
                .SetFont(font)
                );
            document.Add(
                new Paragraph($"Abteilung {exam.Department.DepartmentName}")
                .SetTextAlignment(TextAlignment.LEFT)
                .SetFontSize(22)
                .SetBold()
                .SetFont(font)
                );

            document.Add(
                new Paragraph($"Prüfungskandidat {exam.Student.FullName}")
                .SetTextAlignment(TextAlignment.LEFT)
                .SetFontSize(22)
                .SetBold()
                .SetFont(font)
                );

            Table table = new Table(UnitValue.CreatePercentArray(new float[] { 33f, 33f, 33f }), true);

            LineSeparator defaultLine = new LineSeparator(new SolidLine()).SetWidth(UnitValue.CreatePercentValue(100));

            Style descriptionStyle = new Style().SetTextAlignment(TextAlignment.CENTER).SetFontSize(8);
            Style examValueStyle = new Style().SetTextAlignment(TextAlignment.CENTER).SetMarginTop(20);

            //cxy
            CustomBorderCell c00 = new CustomBorderCell(isTop: true);
            CustomBorderCell c10 = new CustomBorderCell(isTop: true);
            CustomBorderCell c20 = new CustomBorderCell(isTop: true);


            CustomBorderCell c01 = new CustomBorderCell() { TopBorder = CustomBorderCell.THICK_BORDER, BottomBorder = CustomBorderCell.THICK_BORDER };
            CustomBorderCell c11 = new CustomBorderCell() { TopBorder = CustomBorderCell.THICK_BORDER, BottomBorder = CustomBorderCell.THICK_BORDER };
            CustomBorderCell c21 = new CustomBorderCell() { TopBorder = CustomBorderCell.THICK_BORDER, BottomBorder = CustomBorderCell.THICK_BORDER };


            CustomBorderCell c02 = new CustomBorderCell() { TopBorder = CustomBorderCell.THICK_BORDER, BottomBorder = CustomBorderCell.THICK_BORDER };
            CustomBorderCell c12 = new CustomBorderCell() { TopBorder = CustomBorderCell.THICK_BORDER, BottomBorder = CustomBorderCell.THICK_BORDER };
            CustomBorderCell c22 = new CustomBorderCell() { TopBorder = CustomBorderCell.THICK_BORDER, BottomBorder = CustomBorderCell.THICK_BORDER };


            CustomBorderCell c03 = new CustomBorderCell() { BottomBorder = CustomBorderCell.THICK_BORDER };
            CustomBorderCell c13 = new CustomBorderCell() { BottomBorder = CustomBorderCell.THICK_BORDER };
            CustomBorderCell c23 = new CustomBorderCell() { BottomBorder = CustomBorderCell.THICK_BORDER };


            //Row 1
            c00.Add(new Paragraph($"{exam.FailedTerm}. Semester").AddStyle(examValueStyle)
                .Add(defaultLine)
                .Add(new Paragraph("Semesterprüfung über Semester").AddStyle(descriptionStyle)));

            c10.Add(new Paragraph($"{exam.SemesterEnd.SemesterEndDate.ToString("dd.MM.yyyy")}").AddStyle(examValueStyle)
                .Add(defaultLine)
                .Add(new Paragraph("Datum negatives Zeugnis").AddStyle(descriptionStyle)));

            c20.Add(new Paragraph($"{exam.Subject.SubjectName}").AddStyle(examValueStyle)
                .Add(defaultLine)
                .Add(new Paragraph("Gegenstand").AddStyle(descriptionStyle)));

            //Row 2
            c01.Add(new Paragraph().AddStyle(examValueStyle)
                .Add(defaultLine)
                .Add(new Paragraph("Name der Lehrkraft, die das Nicht genügend ausgestellt hat").AddStyle(descriptionStyle)));

            c11.Add(new Paragraph($"{exam.FailedClass}").AddStyle(examValueStyle)
                .Add(defaultLine)
                .Add(new Paragraph("Klasse, in welcher das Nicht genügend ausgestellt wurde").AddStyle(descriptionStyle)));

            Paragraph repetitionParagraph;
            switch (exam.Repetition)
            {
                case 0:
                    repetitionParagraph = new Paragraph("Semesterprüfung");
                    break;
                default:
                    repetitionParagraph = new Paragraph($"{exam.Repetition}. Wiederholung");
                    break;
            }

            c21.Add(repetitionParagraph.AddStyle(examValueStyle)
                .Add(defaultLine)
                .Add(new Paragraph("Antritt").AddStyle(descriptionStyle)));


            //Row 3
            c02.Add(new Paragraph($"{exam.Teacher.DisplayName}").AddStyle(examValueStyle)
                .Add(defaultLine)
                .Add(new Paragraph("PrüferIn").AddStyle(descriptionStyle)));

            c12.Add(new Paragraph($"{exam.Appointment.ExamAppointmentDate.ToString("dd.MM.yyyy")}").AddStyle(examValueStyle)
                .Add(defaultLine)
                .Add(new Paragraph("Datum der Prüfung").AddStyle(descriptionStyle)));

            c12.Add(new Paragraph().AddStyle(examValueStyle)
                .Add(new Text($"Beginn: {new String('_', 20)}").AddStyle(descriptionStyle)));

            c12.Add(new Paragraph().AddStyle(examValueStyle)
                .Add(new Text($"Dauer: {new String('_', 20)}").AddStyle(descriptionStyle)));

            c22.Add(new Paragraph().AddStyle(examValueStyle).SetMarginTop(80)
                .Add(defaultLine)
                .Add(new Paragraph("Prüfungssaal").AddStyle(descriptionStyle)));

            //Row 4
            c03.Add(new Paragraph().AddStyle(examValueStyle)
                .Add(defaultLine)
                .Add(new Paragraph("Unterschrift PrüferIn").AddStyle(descriptionStyle)));

            c13.Add(new Paragraph());

            c23.Add(new Paragraph().AddStyle(examValueStyle)
                .Add(defaultLine)
                .Add(new Paragraph("Note").AddStyle(descriptionStyle)));

            table.AddCell(c00);
            table.AddCell(c10);
            table.AddCell(c20);

            table.AddCell(c01);
            table.AddCell(c11);
            table.AddCell(c21);

            table.AddCell(c02);
            table.AddCell(c12);
            table.AddCell(c22);

            table.AddCell(c03);
            table.AddCell(c13);
            table.AddCell(c23);

            document.Add(table);
            table.Complete();

            Image img = new Image(iText.IO.Image.ImageDataFactory.CreateJpeg(File.ReadAllBytes(@".\HTL_Abteilungen.jpg")));
            document.Add(new Paragraph().SetMarginTop(50));
            img.SetHeight(90);

            document.Add(img);

            document.Add(new Paragraph().SetMarginTop(70));
            document.Add(new Paragraph("HBLA Leonding | Limesstraße 12-14, 4060 Leonding | Tel.: +43 (0) 732 67 33 68-0 | Fax.: +43 (0) 732 67 33 24")
                .SetFontSize(10)
                .SetMargin(0)
                .SetTextAlignment(TextAlignment.JUSTIFIED_ALL));
            document.Add(new Paragraph("office@htl-leonding.ac.at | www.htl-leonding.ac.at | UID-Nr.: ATU 4050 3905 | Bankverb.: IBAN AT10 0100 0000 0539 0478 - BIC: BUNDATWW")
                .SetFontSize(8)
                .SetMargin(0)
                .SetTextAlignment(TextAlignment.JUSTIFIED_ALL));*/
        }

        /*public void AddFooter(Document document, PdfFont font, Student stud)
        {
            Paragraph lFooter = new Paragraph()
                .Add(new Text("HTBLA Leonding"))
                .SetFontSize(10)
                .SetFont(font);
            Paragraph rFooter = new Paragraph()
                .Add(new Text($"Erstellt für {stud.UserName}"))
                .SetFontSize(10)
                .SetFont(font);

            for (int i = 1; i <= document.GetPdfDocument().GetNumberOfPages(); i++)
            {
                float lx = 20;
                float rx = document.GetPdfDocument().GetPage(i).GetPageSize().GetWidth() - 20;
                float y = document.GetPdfDocument().GetPage(i).GetPageSize().GetBottom() + 20;

                document.ShowTextAligned(lFooter, lx, y, i,
                        TextAlignment.LEFT, VerticalAlignment.BOTTOM, 0);
                document.ShowTextAligned(rFooter, rx, y, i,
                        TextAlignment.RIGHT, VerticalAlignment.BOTTOM, 0);
            }
        }
    }*/

    public class CustomBorderCell : Cell
    {
        public readonly static Border THICK_BORDER = new SolidBorder(3);
        public readonly static Border THIN_BORDER = new SolidBorder(1);
        public readonly static Border RIDGE_BORDER = new RidgeBorder(1);


        private Border topBorder;
        private Border bottomBorder;
        private Border rightBorder;
        private Border leftBorder;

        public Border LeftBorder
        {
            get { return leftBorder; }
            set { leftBorder = value; SetBorderLeft(leftBorder); }
        }


        public Border RightBorder
        {
            get { return rightBorder; }
            set { rightBorder = value; SetBorderRight(rightBorder); }
        }


        public Border BottomBorder
        {
            get { return bottomBorder; }
            set { bottomBorder = value; SetBorderBottom(bottomBorder); }
        }


        public Border TopBorder
        {
            get { return topBorder; }
            set { topBorder = value; SetBorderTop(topBorder); }
        }



        public CustomBorderCell()
        {
            initBorders();
        }

        public CustomBorderCell(
            CustomBorderType customBorder = CustomBorderType.NO_BORDER,
            bool isTop = false,
            bool isBottom = false)
        {
            initBorders(customBorder, isTop, isBottom);
        }

        public CustomBorderCell(
            int rowspan,
            int colspan,
            CustomBorderType customBorder = CustomBorderType.NO_BORDER,
            bool isTop = false,
            bool isBottom = false) : base(rowspan, colspan)
        {
            initBorders(customBorder, isTop, isBottom);
        }

        private void initBorders(
            CustomBorderType borderType = CustomBorderType.NO_BORDER,
            bool isTop = false,
            bool isBottom = false)
        {
            Border leftRight = THICK_BORDER;
            Border topBottomT1 = Border.NO_BORDER;
            Border topBottomT2 = THIN_BORDER;
            Border topBottomT3 = RIDGE_BORDER;

            Border topBottom = null;

            switch (borderType)
            {
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

        public enum CustomBorderType
        {
            NO_BORDER,
            THIN_BORDER,
            RIDGE_BORDER,
            THICK_BORDER
        }
    }
}
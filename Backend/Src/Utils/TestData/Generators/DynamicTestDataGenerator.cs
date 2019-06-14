using Backend.Core.Entities;
using Backend.Core.Entities.UserManagement;
using Backend.Persistence;
using Backend.Src.Core.Entities;
using Backend.Utils;
using Bogus;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Src.Utils.TestData
{
    public class DynamicTestDataGenerator
    {
        public ApplicationDbContext context;
        public UserManager<FitUser> userManager;

        public DynamicTestDataGenerator(ApplicationDbContext context, UserManager<FitUser> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

        public async Task GenerateGraduates(int amount)
        {
            Console.WriteLine("Insert finished Students...");
            var addressGen = new Faker<Address>()
                .RuleFor(adr => adr.Street, f => f.Address.StreetName())
                .RuleFor(adr => adr.StreetNumber, f => f.Address.StreetAddress())
                .RuleFor(adr => adr.ZipCode, f => f.Address.ZipCode())
                .RuleFor(ard => ard.City, f => f.Address.City())
                .RuleFor(ard => ard.Addition, f => f.Address.SecondaryAddress())
                ;//.FinishWith((f,ard) => Console.WriteLine(ard.Street));

            var graduateGen = new Faker<Graduate>()
                .RuleFor(gr => gr.Gender, f => f.PickRandom<Gender>().ToString())
                .RuleFor(gr => gr.FirstName, f => f.Name.FirstName())
                .RuleFor(gr => gr.LastName, f => f.Name.LastName())
                .RuleFor(gr => gr.Email, (f, u) => f.Internet.ExampleEmail(u.FirstName, u.LastName))
                .RuleFor(gr => gr.PhoneNumber, f => f.Phone.PhoneNumber())
                .RuleFor(gr => gr.GraduationYear, f => f.Random.Number(2000, 2019))
                .RuleFor(gr => gr.RegistrationToken, f => f.Random.String2(12, 12))
                ;//.FinishWith((f,gr) => Console.WriteLine(gr.LastName));

            for (int i = 0; i < amount; i++)
            {
                var graduate = graduateGen.Generate();
                var adr = addressGen.Generate();

                context.Addresses.Add(adr);

                graduate.Address = adr;
                FitUser graduateUser = new FitUser();
                graduateUser.UserName = graduate.RegistrationToken;
                graduateUser.Role = "Member";

                await userManager.CreateAsync(graduateUser, graduate.RegistrationToken);

                graduate.fk_FitUser = graduateUser.Id;
                context.Graduates.Add(graduate);
            }
            context.SaveChanges();
        }

        public async Task InsertRessources(int amount)
        {
            Console.WriteLine("Look for useable Resources for the FIT ...");
            var ress = new Faker<Resource>()
                .RuleFor(r => r.Name, f => f.Hacker.Noun())
                ;//.FinishWith((f, r) => Console.WriteLine(r.Name));
            for (int i = 0; i < amount; i++)
            {
                Resource res = ress.Generate();
                context.Resources.Add(res);
                FillDbHelper.resourceList.Add(res);
            }
            context.SaveChanges();
        }

        public async Task InsertEventAreaLocations(int amountEvents, int amountAreas, int locationsForAreas)
        {
            Console.WriteLine("create Events with there Locations and Areas... ");

            string[] category = new String[] { "A", "B" };
            var locationGen = new Faker<Location>()
                .RuleFor(lo => lo.Category, f => f.Random.Char('A', 'B').ToString())
                .RuleFor(lo => lo.Number, f => f.Random.Number(1, 100).ToString())
                .RuleFor(lo => lo.XCoordinate, f => f.Random.Double(1, 100))
                .RuleFor(lo => lo.YCoordinate, f => f.Random.Double(1, 100));

            var areaGen = new Faker<Area>()
                .RuleFor(ar => ar.Designation, f => f.Name.FullName())
                .RuleFor(ar => ar.Locations, f => new List<Location>())
                .RuleFor(ar => ar.Graphic, f => new DataFile(f.Lorem.Word(), f.Image.PicsumUrl(900, 344)));

            for (int i = 0; i < amountEvents; i++)
            {
                Event e = new Event();
                e.EventDate = DateTime.Now.AddYears(-1 * i).AddDays(1);
                e.PresentationsLocked = false;
                e.RegistrationEnd = e.EventDate.AddMonths(-1);
                e.RegistrationStart = e.RegistrationEnd.AddMonths(-3);
                e.RegistrationState = new RegistrationState();
                e.RegistrationState.IsLocked = false;
                e.RegistrationState.IsCurrent = true;
                e.Areas = new List<Area>();
                context.Events.Add(e);

                for (int j = 0; j < amountAreas; j++)
                {
                    Area area = areaGen.Generate();
                    // some hack to trick the local server save mechanic
                    area.Graphic.DataUrl += "&workaround=EVENT_" + e.Id;
                    for (int k = 0; k < locationsForAreas; k++)
                    {
                        Location loc = locationGen.Generate();
                        // If the number has only 1 digit a zero should be added in front of it
                        // Ex.:  7   ->   07
                        if (loc.Number.Length == 1)
                            loc.Number = "0" + loc.Number;
                        if (loc.Number.Length < 3)
                            loc.Number = j + loc.Number;

                        context.Locations.Add(loc);
                        area.Locations.Add(loc);
                    }
                    e.Areas.Add(area);
                }
                context.SaveChanges();
                FillDbHelper.eventList.Add(e);
            }
        }

        public async Task InsertCompanies(int amount)
        {
            Console.WriteLine("Search for Companies which want to join the FIT ...");
            var companyGen = new Faker<Company>()
                .RuleFor(c => c.Name, f => f.Company.CompanyName())
                .RuleFor(c => c.IsAccepted, f => f.Random.Number(0, 1))
                .RuleFor(c => c.RegistrationToken, f => f.Random.String2(4) + '-' + f.Random.String2(4) + '-' + f.Random.String2(4))
                .RuleFor(c => c.MemberStatus, f => FillDbHelper.memberList.ElementAt(f.Random.Number(1, 3)))
                .RuleFor(c => c.MemberPaymentAmount, (f, c) => c.MemberStatus.DefaultPrice)
                .RuleFor(c => c.Tags, (f, c) => new List<CompanyTag> { new CompanyTag { Comapny = c, Tag = FillDbHelper.tags.ElementAt(f.Random.Number(0, FillDbHelper.tags.Count - 1)) }, new CompanyTag { Comapny = c, Tag = FillDbHelper.tags.ElementAt(f.Random.Number(0, FillDbHelper.tags.Count - 1)) } })
                .RuleFor(c => c.Branches, (f, c) => new List<CompanyBranch> { new CompanyBranch() { Comapny = c, Branch = FillDbHelper.branches.ElementAt(f.Random.Number(0, 2)) } });
            ;//.FinishWith((f,c) => Console.WriteLine(c.CompanyName));
            var addressGen = new Faker<Address>()
                .RuleFor(adr => adr.Street, f => f.Address.StreetName())
                .RuleFor(adr => adr.StreetNumber, f => f.Address.StreetAddress())
                .RuleFor(adr => adr.ZipCode, f => f.Address.ZipCode())
                .RuleFor(ard => ard.City, f => f.Address.City())
                .RuleFor(ard => ard.Addition, f => f.Address.SecondaryAddress());
            ;//.FinishWith((f,ard) => Console.WriteLine(ard.Street));
            var contactGen = new Faker<Contact>()
                .RuleFor(c => c.Gender, f => f.PickRandom<Gender>().ToString())
                .RuleFor(c => c.FirstName, f => f.Name.FirstName())
                .RuleFor(c => c.LastName, f => f.Name.LastName())
                .RuleFor(c => c.PhoneNumber, f => f.Phone.PhoneNumber())
                .RuleFor(c => c.Email, f => f.Internet.ExampleEmail())
                ;//.FinishWith((f, c) => Console.WriteLine(c.LastName));


            for (int i = 0; i < amount; i++)
            {
                var comp = companyGen.Generate();
                comp.Name = comp.Name.Substring(0, Math.Min(comp.Name.Length, 30));

                var add = addressGen.Generate();

                var cont = contactGen.Generate();


                context.Addresses.Add(add);
                context.Contacts.Add(cont);
                context.SaveChanges();

                comp.Contact = cont;
                comp.Address = add;

                FitUser companyUser = new FitUser();
                companyUser.UserName = comp.RegistrationToken;
                companyUser.Role = "Member";

                await userManager.CreateAsync(companyUser, comp.RegistrationToken);

                comp.fk_FitUser = companyUser.Id;

                context.Companies.Add(comp);
            }
            context.SaveChanges();
        }

        public async Task CreateBookings(int eventAmount, int bookingAmount, int resourceAmount)
        {
            Console.WriteLine("create bookings for each company for each FIT...");
            var representativeGen = new Faker<Representative>()
                .RuleFor(r => r.Email, f => f.Internet.ExampleEmail())
                .RuleFor(r => r.Image, f => new DataFile(f.Lorem.Word(), f.Image.PicsumUrl(f.Random.Number(100, 200), f.Random.Number(150, 250))))
                .RuleFor(r => r.Name, f => f.Name.FullName());
            var bookingGen = new Faker<Booking>()
                    .RuleFor(b => b.AdditionalInfo, f => "Additional Information: " + f.Random.String2(10))
                    .RuleFor(b => b.CompanyDescription, f => "Company Description: " + f.Random.String2(10))
                    .RuleFor(b => b.isAccepted, f => f.Random.Number(0, 1))
                    .RuleFor(b => b.ProvidesSummerJob, f => f.Random.Bool())
                    .RuleFor(b => b.ProvidesThesis, f => f.Random.Bool())
                    .RuleFor(b => b.Remarks, f => "Remarks")
                    .RuleFor(b => b.CreationDate, f => f.Date.Past())
                    .RuleFor(b => b.fk_FitPackage, f => f.Random.Number(1, 3))
                    .RuleFor(b => b.Email, f => f.Internet.ExampleEmail())
                    .RuleFor(b => b.Branch, f => "Branche " + f.Random.String2(10))
                    .RuleFor(b => b.EstablishmentsAut, f => f.Address.City())
                    .RuleFor(b => b.EstablishmentsCountAut, f => 1)
                    .RuleFor(b => b.EstablishmentsCountInt, f => 2)
                    .RuleFor(b => b.EstablishmentsInt, f => f.Address.City() + ";" + f.Address.City())
                    .RuleFor(b => b.Homepage, f => f.Internet.Url())
                    .RuleFor(b => b.Logo, f => new DataFile(f.Lorem.Word(), f.Image.PicsumUrl(f.Random.Number(200, 500), f.Random.Number(300, 600))))
                    .RuleFor(b => b.PhoneNumber, f => f.Phone.PhoneNumber())
                    .RuleFor(b => b.Branches, (f, b) => new List<BookingBranch> { new BookingBranch() { Booking = b, Branch = FillDbHelper.branches.ElementAt(f.Random.Number(0, 2)) } });
            ;//.RuleFor(b => b.Presentation, f => p);
            var pressentationGen = new Faker<Presentation>()
                .RuleFor(p => p.Branches, (f, p) => new List<PresentationBranch>() { new PresentationBranch() { Presentation = p, Branch = FillDbHelper.branches.ElementAt(f.Random.Number(0, 2)) } })
                .RuleFor(p => p.Description, f => f.Random.String2(20))
                .RuleFor(p => p.IsAccepted, f => f.Random.Number(-1, 1))
                .RuleFor(p => p.Title, f => "Präsi - " + f.Name.LastName())
                .RuleFor(p => p.File, f => null);
            var resourseBookingGen = new Faker<ResourceBooking>()
                .RuleFor(rb => rb.Resource, f => FillDbHelper.resourceList.ElementAt(f.Random.Number(0, FillDbHelper.resourceList.Count - 1)));
            Booking book;

            int topPos;

            for (int k = 0; k < eventAmount; k++)
            {
                // Get the locations(each floor) for this event 
                // (required to assign a booking to a location)
                var locations = context.Areas.Where(a => a.fk_Event == k + 1).Select(l => l.Locations).ToArray();
                int areaSize = locations[0].Count;
                int floor = 0, location = 0;

                int random = new Random().Next(3, FillDbHelper.NUMBER_COMPANY);
                Console.Write(random + " /");

                topPos = Console.CursorTop;
                for (int i = 0; i < random; i++, location++)
                {
                    //Representatives
                    List<Representative> repre = new List<Representative>();
                    Representative repr = representativeGen.Generate();

                    context.Representatives.Add(repr);
                    repre.Add(repr);

                    Presentation p = pressentationGen.Generate();

                    context.Presentations.Add(p);

                    // Set up Booking
                    Booking booking = bookingGen.Generate();
                    booking.Representatives = repre;
                    booking.Presentation = p;
                    booking.fk_Company = i + 1;
                    booking.fk_Contact = i + 1;
                    booking.fk_Event = k + 1;
                    //All locations in the current floor are occupied
                    if (location >= areaSize)
                    {
                        //Move to the next floor and start again (from 0)
                        floor++;
                        location = 0;
                    }
                    booking.Location = locations[floor][location];
                    book = booking;

                    context.Bookings.Add(booking);
                    context.SaveChanges();

                    //Just for the output in the console..
                    Console.SetCursorPosition(5, Console.CursorTop);
                    Console.Write(i+1);

                    booking.Resources = new List<ResourceBooking>();
                    for (int j = 0; j < resourceAmount; j++)
                    {
                        ResourceBooking rb = resourseBookingGen.Generate();
                        context.ResourceBookings.Add(rb);

                        booking.Resources.Add(rb);
                        context.Bookings.Update(booking);
                    }
                }
                context.SaveChanges();
                Console.WriteLine();
                // ressourceBookingCreaten
            }
        }

        public async Task GenerateTags(int tagAmount)
        {
            Console.WriteLine("Generating random tags");
            var tagGen = new Faker<Tag>()
                .RuleFor(t => t.Value, f => f.Commerce.Department())
                .RuleFor(t => t.IsArchive, f => f.Random.Bool());

            for (int i = 0; i < tagAmount; i++)
            {
                Tag tag = tagGen.Generate();
                context.Tags.Add(tag);
                FillDbHelper.tags.Add(tag);
            }
            context.SaveChanges();
        }
    }
}
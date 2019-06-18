using Backend.Core;
using Backend.Core.Contracts;
using Backend.Core.Entities;
using Backend.Core.Entities.UserManagement;
using Backend.Persistence;
using Backend.Src.Core.Entities;
using Backend.Src.Utils.TestData;
using Backend.Src.Utils.TestData.Generators;
using Bogus;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StoreService.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Utils {
    public static class FillDbHelper {

        public static int NUMBER_GRADUTE = 200;
        public static int NUMBER_COMPANY = 100;
        public static int NUMBER_RESOURCES = 10;

        public static int NUMBER_EVENTS = 6;
        public static int NUMBER_AREAS = 3;
        public static int NUMBER_LOCATIONS_FOR_AREA = 30;

        public static int NUMBER_RESOURCE_BOOKING_FOR_COMPANY = 3;
        public static int NUMBER_TAGS = 15;

        public static List<MemberStatus> memberList = new List<MemberStatus>();
        public static List<Resource> resourceList = new List<Resource>();
        public static List<Event> eventList = new List<Event>();
        public static List<Branch> branches = new List<Branch>();
        public static List<Tag> tags = new List<Tag>();

        public static string[] FLOORS = new string[] { "UG", "EG", "1OG", "2OG" };

        public async static Task CreateTestData(ApplicationDbContext context, UserManager<FitUser> userManager, IUnitOfWork uow) {
            DynamicTestDataGenerator dynamicDataGenerator = new DynamicTestDataGenerator(context, userManager);
            StaticTestDataGenerator staticDataGenerator = new StaticTestDataGenerator(context, userManager);

            try
            {
                await staticDataGenerator.GenerateAdmin();
                await staticDataGenerator.GenerateLockPage();
                await staticDataGenerator.GenerateSmtp();
                await staticDataGenerator.GenerateMemberStati();
                await staticDataGenerator.GeneratePackages();
                await staticDataGenerator.GenerateBranches();
            
                await dynamicDataGenerator.GenerateTags(NUMBER_TAGS);
                await dynamicDataGenerator.InsertEventAreaLocations(NUMBER_EVENTS,NUMBER_AREAS,NUMBER_LOCATIONS_FOR_AREA);
                await dynamicDataGenerator.InsertCompanies(NUMBER_COMPANY);
                await dynamicDataGenerator.InsertRessources(NUMBER_RESOURCES);
                await dynamicDataGenerator.GenerateGraduates(NUMBER_GRADUTE);
                await dynamicDataGenerator.CreateBookings(NUMBER_EVENTS, NUMBER_COMPANY, NUMBER_RESOURCE_BOOKING_FOR_COMPANY);

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
            EmailTestDataGenerator.CreateEmailsAndVariables(uow);
        }
    }
}

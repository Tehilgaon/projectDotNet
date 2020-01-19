using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;

namespace DS
{
    public class DataSource
    {
        public static List<HostingUnit> hostingUnits = new List<HostingUnit>()
        {
            new HostingUnit()
            {
                HostingUnitName="place",
                HostingUnitType=Enums.HostingUnitType.צימר.ToString(),
                Area=Enums.Regions.צפון.ToString(),
                Host=new Host()
                {
                    HostKey="123456789",
                    CollectionClearance=false,
                    PrivateName="Dani",
                    FamilyName="cohen",
                    MailAddress="Dani@gmail.com",
                    PhoneNumber="0505050505",
                    BankAccountNumber="123123",
                    Bankbranch=GetAllBranches()[0],
                }
            },
             new HostingUnit()
             {
                HostingUnitName="villa",
                HostingUnitType=Enums.HostingUnitType.צימר.ToString(),
                Area=Enums.Regions.צפון.ToString(),
                Host=new Host()
                {
                    HostKey="987654321",
                    CollectionClearance=true,
                    PrivateName="Via",
                    FamilyName="Levin",
                    MailAddress="Via@gmail.com",
                    PhoneNumber="0501010101",
                    BankAccountNumber="456456",
                    Bankbranch=GetAllBranches()[1],
                },
            },
            new HostingUnit()
             {
                HostingUnitName="Tejj",
                HostingUnitType=Enums.HostingUnitType .צימר.ToString(),
                Area=Enums.Regions.ירושלים.ToString(),
                Host=new Host()
                {
                    HostKey="121212121",
                    CollectionClearance=true,
                    PrivateName="Tael",
                    FamilyName="Levi",
                    MailAddress="Tael@gmail.com",
                    PhoneNumber="0507777777",
                    BankAccountNumber="777777",
                    Bankbranch=GetAllBranches()[2],
                },
            },
            new HostingUnit()
             {
                HostingUnitName="שלנו",
                HostingUnitType=Enums.HostingUnitType .צימר.ToString(),
                Area=Enums.Regions.ירושלים.ToString(),
                Host=new Host()
                {
                    HostKey="050505055",
                    CollectionClearance=true,
                    PrivateName="תהל",
                    FamilyName="לוי",
                    MailAddress="tata@gmail.com",
                    PhoneNumber="0505050505",
                    BankAccountNumber="777777",
                    Bankbranch=GetAllBranches()[3],
                },
            },
            new HostingUnit()
             {
                HostingUnitName="בית בכפר",
                HostingUnitType=Enums.HostingUnitType .בית_הארחה.ToString(),
                Area=Enums.Regions.אילת.ToString(),
                Host=new Host()
                {
                    HostKey="147258391",
                    CollectionClearance=false,
                    PrivateName="רבקה",
                    FamilyName="לוי",
                    MailAddress="Riv@gmail.com",
                    PhoneNumber="0501774277",
                    BankAccountNumber="454777",
                    Bankbranch=GetAllBranches()[2],
                },
            },
            new HostingUnit()
             {
                HostingUnitName="פריז",
                HostingUnitType=Enums.HostingUnitType .בית_הארחה.ToString(),
                Area=Enums.Regions.שפלה.ToString(),
                Host=new Host()
                {
                    HostKey="315136951",
                    CollectionClearance=true,
                    PrivateName="רחל",
                    FamilyName="דןן",
                    MailAddress="rachel@gmail.com",
                    PhoneNumber="0508947777",
                    BankAccountNumber="789654",
                    Bankbranch=GetAllBranches()[0],
                },
            },
        };

        public static List<Order> orders = new List<Order>();
       
         
        public static List<GuestRequest> guestRequests = new List<GuestRequest>()
        {
            new GuestRequest()
            {
                 PrivateName="yaya",
                 FamilyName="haddad",
                 MailAddress="rut@gmail.com",
                 EntryDate=new DateTime(2020,2,3),
                 ReleaseDate=new DateTime(2020,2,10), 
                 Area=Enums.Regions.צפון.ToString(),
                 
                 Adults=2,
                 Children=2,
                 Type=Enums.HostingUnitType.צימר.ToString(),    
            },
             new GuestRequest()
            {
                 PrivateName="yossi",
                 FamilyName="jossef",
                 MailAddress="rut@gmail.com",
                 EntryDate=new DateTime(2020,3,3),
                 ReleaseDate=new DateTime(2020,3,6),
                 Area=Enums.Regions.ירושלים.ToString(),
                  
                 Adults=2,
                 Children=1,
                 Type=Enums.HostingUnitType.צימר.ToString(),
            },
              new GuestRequest()
            {
                 PrivateName="rut",
                 FamilyName="dahan",
                 MailAddress="rut@gmail.com",
                 EntryDate=new DateTime(2020,3,3),
                 ReleaseDate=new DateTime(2020,3,6),
                 Area=Enums.Regions.צפון.ToString(),
                 
                 Adults=1, 
                 Type=Enums.HostingUnitType.צימר.ToString(),
            },
              new GuestRequest()
            {
                 PrivateName="dina",
                 FamilyName="dan",
                 MailAddress="dina@gmail.com",
                 EntryDate=new DateTime(2020,4,3),
                 ReleaseDate=new DateTime(2020,4,6),
                 Area=Enums.Regions.שפלה.ToString(), 
                 Adults=1,
                 Type=Enums.HostingUnitType.צימר.ToString(),
            },
              new GuestRequest()
            {
                 PrivateName="רון",
                 FamilyName="כהן",
                 MailAddress="ron@gmail.com",
                 EntryDate=new DateTime(2020,6,3),
                 ReleaseDate=new DateTime(2020,6,6),
                 Area=Enums.Regions.אילת.ToString(),

                 Adults=1,
                 Type=Enums.HostingUnitType.בית_מלון.ToString(),
            },
              new GuestRequest()
            {
                 PrivateName="שלום",
                 FamilyName="לוי",
                 MailAddress="RUT@gmail.com",
                 EntryDate=new DateTime(2020,3,5),
                 ReleaseDate=new DateTime(2020,3,7),
                 Area=Enums.Regions.ירושלים.ToString(),

                 Adults=1,
                 Type=Enums.HostingUnitType.צימר.ToString(),
            },
        };


        public DataSource(){}

        public static List<BankBranch> GetAllBranches()
        {
            List<BankBranch> bankAccounts = new List<BankBranch>()
            {
                new BankBranch(12, "Leumi", 200, "Gilo 15", "jerusalem"),
                new BankBranch(12, "Leumi",200, "Gilo 15", "jerusalem"   ),
                new BankBranch(81, "hapoalim",150, "Herzel 30", "Tel Aviv" ),
                new BankBranch(47, "Mizrahi", 411, "Vered 4", "Ashkelon" ),
                new BankBranch(81, "hapoalim", 140, "Ben Guryon 21", "Lod" )
            };
            return bankAccounts;
        }
    }
}

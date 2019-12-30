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
                HostingUnitType=Enums.HostingUnitType.צימר,
                Area=Enums.Regions.North,
                Host=new Host()
                {
                    HostKey="123456789",
                    CollectionClearance=false,
                    PrivatrName="Dani",
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
                HostingUnitType=Enums.HostingUnitType.צימר,
                Area=Enums.Regions.North,
                Host=new Host()
                {
                    HostKey="987654321",
                    CollectionClearance=true,
                    PrivatrName="Via",
                    FamilyName="Levin",
                    MailAddress="Via@gmail.com",
                    PhoneNumber="0501010101",
                    BankAccountNumber="456456",
                    Bankbranch=GetAllBranches()[1],
                },
            },
            new HostingUnit()
             {
                HostingUnitName="Te",
                HostingUnitType=Enums.HostingUnitType .צימר,
                Area=Enums.Regions.Jerusalem,
                Host=new Host()
                {
                    HostKey="121212121",
                    CollectionClearance=true,
                    PrivatrName="Tael",
                    FamilyName="Levi",
                    MailAddress="Tael@gmail.com",
                    PhoneNumber="0507777777",
                    BankAccountNumber="777777",
                    Bankbranch=GetAllBranches()[2],
                },
            }
        };

        public static List<Order> orders = new List<Order>();
       
         
        public static List<GuestRequest> guestRequests = new List<GuestRequest>()
        {
            new GuestRequest()
            {
                 PrivateName="yaya",
                 FamilyName="haddad",
                 MailAddress="ya123@gmail.com",
                 EntryDate=new DateTime(2020,2,3),
                 ReleaseDate=new DateTime(2020,2,10), 
                 Area=Enums.Regions.North.ToString(),
                 Pool=true,
                 Adults=2,
                 Children=2,
                 Type=Enums.HostingUnitType.צימר.ToString(),    
            },
             new GuestRequest()
            {
                 PrivateName="yossi",
                 FamilyName="jossef",
                 MailAddress="jossef@gmail.com",
                 EntryDate=new DateTime(2020,3,3),
                 ReleaseDate=new DateTime(2020,3,6),
                 Area=Enums.Regions.Jerusalem.ToString(),
                 Garden=true,
                 Adults=2,
                 Children=1,
                 Type=Enums.HostingUnitType.צימר.ToString(),
            },
              new GuestRequest()
            {
                 PrivateName="rut",
                 FamilyName="dahan",
                 MailAddress="rutt@gmail.com",
                 EntryDate=new DateTime(2020,3,3),
                 ReleaseDate=new DateTime(2020,3,6),
                 Area=Enums.Regions.North.ToString(),
                 Pool=true,
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

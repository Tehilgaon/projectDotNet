using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Net.Mail;

namespace BE
{
    [Serializable]
    public class GuestRequest
    {
        private string guestRequestKey;
        private string privateName;
        private string familyName;
        private string mailAddress;
        private string status;
        private DateTime registrationDate;
        private DateTime entryDate;
        private DateTime releaseDate;
        private string area;
        private string subArea;
        private string type;
        private int adults;
        private int children;
        private bool? pool;
        private bool? jacuzzi;
        private bool? garden;
        private bool? childrensAttractions;
          

        public string GuestRequestKey { get;}
        public string PrivateName { get => privateName;
            set
            {
                Regex r = new Regex("^[a-zA-Zא-ת]{2,15}$");
                if (!r.IsMatch(value))
                    throw new Exception("Name should contain only letters, Between 2-15.");
                privateName = value;
            }
        }
        public string FamilyName { get => familyName;
            set
            {
                Regex r = new Regex("^[a-zA-Zא-ת]{2,15}$");
                if (!r.IsMatch(value))
                    throw new Exception("Name should contain only letters, Between 2-15.");
                privateName = value;
            } 
        }
        public string MailAddress { get => mailAddress;
            set
            {
                try
                {
                    MailAddress m = new MailAddress(value);
                }
                catch (Exception) { throw new Exception("Invalid email address."); }
                mailAddress = value;
            } 
        }
        public DateTime RegistrationDate { get;}
        public DateTime EntryDate { get => entryDate; }
        public DateTime ReleaseDate { get => releaseDate; }
        public string Status { get; set; }
        public string Area { get; set; }
        public string SubArea { get; set; }
        public string Type { get; set; }
        public int Adults { get; set;}
        public int Children { get; set; }
        public bool? Pool { get; set; }
        public bool? Jacuzzi { get; set; }
        public bool? Garden { get; set; }
        public bool? ChildrensAttractions { get; set; }

          
        public GuestRequest()
        {
            guestRequestKey = Configuration.GuestRequestKey++.ToString();
            status = Enums.GuestRequestStatus.Active.ToString();
            registrationDate = DateTime.Now;
        }


        public override string ToString()
        {
            return MailAddress + Convert.ToString(GuestRequestKey);
        }
    }
}
 

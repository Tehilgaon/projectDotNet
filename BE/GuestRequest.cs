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
        private bool pool;
        private bool jacuzzi;
        private bool garden;
        private bool parking;
        private bool baby_bed;




        public string PrivateName { get => privateName;
            set
            {
                Regex r = new Regex("^[ a-zA-Zא-ת]{2,15}$");
                if (!r.IsMatch(value))
                    throw new Exception("Name should contain only letters, Between 2-15.");
                privateName = value;
            }
        }
        public string FamilyName { get => familyName;
            set
            {
                Regex r = new Regex("^[ a-zA-Zא-ת]{2,15}$");
                if (!r.IsMatch(value))
                    throw new Exception("Name should contain only letters, Between 2-15.");
                familyName = value;
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
        public DateTime RegistrationDate { get; }
        public DateTime EntryDate
        {
            get => entryDate;
            set
            {
                if (value < DateTime.Now)
                    throw new Exception("תאריך לא תקין");
                entryDate = value;
            }
        }
        public DateTime ReleaseDate
        {
            get => releaseDate;
            set
            {
                if (value < DateTime.Now)
                    throw new Exception("תאריך לא תקין");
                releaseDate = value;
            }
        }
       
     
        public string GuestRequestKey { get => guestRequestKey; set => guestRequestKey = value; }
        public string Area { get => area; set => area = value; }
        public string SubArea { get => subArea; set => subArea = value; }
        public string Type { get => type; set => type = value; }
        public int Adults { get => adults; set => adults = value; }
        public int Children { get => children; set => children = value; }
        public string Status { get => status; set => status = value; }
        public bool Pool { get => pool; set => pool = value; }
        public bool Jacuzzi { get => jacuzzi; set => jacuzzi = value; }
        public bool Garden { get => garden; set => garden = value; }
        public bool Parking { get => parking; set => parking = value; }
        public bool Baby_bed { get => baby_bed; set => baby_bed = value; }

        public GuestRequest()
        {
            GuestRequestKey = Configuration.GuestRequestKey++.ToString();
            Status = Enums.GuestRequestStatus.Active.ToString();
            RegistrationDate = DateTime.UtcNow;
 
        }


        public override string ToString()
        {
            return Convert.ToString(GuestRequestKey) + ",  שם פרטי:" + privateName + " ,שם משפחה:" + familyName +
                " , כתובת מייל: " + mailAddress + "  ,סטטוס דרישת לקוח:" + Status + 
                "  ,תאריך התחלה" + EntryDate.ToString("MM/dd/yyyy ") + "  ,תאריך סוף:" + releaseDate.ToString("MM/dd/yyyy ") + "  ,אזור:"
                + Area + "  ,סוג אירוח:" + Type + "  ,מספר מבוגרים:" + Adults + "  ,מספר ילדים:" + Children;

        }
    }
}
 

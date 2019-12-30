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
    public class Host
    {
        private string hostKey;
        private string privatrName;
        private string familyName;
        private string phoneNumber;
        private string mailAddress;
        private BankAccount bankAccount;
        private string bankAccountNumber;
        private bool collectionClearance;
        private double fee;

        public string HostKey { get => hostKey; set => hostKey = value; }
        public string PrivatrName { get => privatrName; set => privatrName = value; }
        public string FamilyName { get => familyName; set => familyName = value; }
        public string PhoneNumber { get => phoneNumber;
            set 
            {
                Regex r = new Regex("(^05(0|[2-9])-?[0-9]{7}$)|(^0(2|3|4|7|8|9)-?[0-9]{7}$)" );
                if (!r.IsMatch(value))
                    throw new Exception("Phone number is incorrect.");
                phoneNumber = value;
            }
        }
        public string MailAddress { get => mailAddress;
            set {
                try
                {
                    MailAddress m = new MailAddress(value);
                }
                catch (Exception) { throw new Exception("Invalid email address."); }
                mailAddress = value;
            }
        }
        public bool CollectionClearance { get => collectionClearance; set => collectionClearance = value; }
        internal BankAccount BankAccount { get => bankAccount; set => bankAccount = value; }
        public string BankAccountNumber { get => bankAccountNumber; set => bankAccountNumber = value; }
        public double Fee { get => fee; set => fee = value; }
        

        public Host() { }

         

        public override string ToString()
        {
            return hostKey;
        }

         
    }
}

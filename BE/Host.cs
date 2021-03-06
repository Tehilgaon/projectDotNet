﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Net.Mail;

namespace BE
{
    [Serializable]
    public class Host : IEquatable<Host>
    {
        private string hostKey;
        private string privateName;
        private string familyName;
        private string phoneNumber;
        private string mailAddress;
        private BankBranch bankBranch;
        private string bankAccountNumber;
        private bool collectionClearance;
        private double fee;
         


        public string HostKey { get => hostKey; set => hostKey = value; }
        public string PrivateName
        {
            get => privateName;
            set
            {
                Regex r = new Regex("^[a-zA-Zא-ת]{2,15}$");
                if (!r.IsMatch(value))
                    throw new Exception("Name should contain only letters, Between 2-15.");
                privateName = value;
            }
        }
        public string FamilyName { get => familyName; set => familyName = value; }
        public string PhoneNumber
        {
            get => phoneNumber;
            set
            {
                Regex r = new Regex("(^05(0|[2-9])-?[0-9]{7}$)|(^0(2|3|4|7|8|9)-?[0-9]{7}$)");
                if (!r.IsMatch(value))
                    throw new Exception("Phone number is incorrect.");
                phoneNumber = value;
            }
        }
        public string MailAddress
        {
            get => mailAddress;
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
        public bool CollectionClearance { get => collectionClearance; set => collectionClearance = value; }
        public BankBranch Bankbranch { get => bankBranch; set => bankBranch = value; }
        public string BankAccountNumber
        {
            get => bankAccountNumber;
            set
            {
                Regex r = new Regex("^[0-9]{6}$");
                if (!r.IsMatch(value))
                    throw new Exception("incorrect number account.");
                bankAccountNumber = value;
            }
        }
        public double Fee { get=> fee; set=>fee=value; }
      

        public Host()
        {
            Bankbranch = new BankBranch();
        }



        public override string ToString()
        {
            return HostKey + "\t" + PrivateName + "\t"
                + FamilyName + "\t" + PhoneNumber + "\t" + MailAddress;
        }

        //IEquatable<Host> methods, for the distinct function in host_grouping
        public bool Equals(Host other)
        {
            return HostKey.Equals(HostKey);
        }
        public override int GetHashCode()
        {
            return HostKey.GetHashCode();
        }
    }
}
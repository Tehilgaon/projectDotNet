using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    [Serializable]
    public class BankBranch
    {
        private int bankNumber;
        private string bankName;
        private int branchNumber;
        private string branchAddress;
        private string branchCity;
         

        public int BankNumber { get => bankNumber; }
        public string BankName { get => bankName; }
        public int BranchNumber { get => branchNumber; }
        public string BranchAddress { get => branchAddress; }
        public string BranchCity { get => branchCity; }
        
        public BankBranch()
        {

        }

        public BankBranch(int BankNum,string BankName,int BranchNum, string BrancAddr,string BranchCity)
        {
            bankNumber = BankNum;
            bankName = BankName;
            branchAddress = BrancAddr;
            branchNumber = BranchNum;
            branchCity = BranchCity;
             
        }
        
        public override string ToString()
        {
            return ",  שם בנק:" + BankName + " ,מספר בנק:" + BankNumber + ",  מספר סניף:" + 
                BranchNumber + " ,כתובת סניף:" + BranchAddress + " " + BranchCity;
        }
    }
}

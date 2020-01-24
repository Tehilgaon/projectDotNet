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
         

        public int BankNumber { get => bankNumber; set => bankNumber=value; }
        public string BankName { get => bankName; set => bankName=value; }
        public int BranchNumber { get => branchNumber; set => branchNumber=value; }
        public string BranchAddress { get => branchAddress; set => branchAddress = value; }
        public string BranchCity { get => branchCity; set => branchCity = value; }
        
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
            return "\t" + BankName + "\t" + BankNumber + "\t" + 
                BranchNumber + "\t" + BranchAddress + " " + BranchCity;
        }
    }
}

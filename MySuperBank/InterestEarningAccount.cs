using classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySuperBank
{
    public class InterestEarningAccount : BankAccount
    {
        /*The compiler doesn't generate a default constructor when you define a constructor yourself. 
         *That means each derived class must explicitly call this constructor.
         *The parameters to this new constructor match the parameter type and names of the base class constructor. 
         *You use the : base() syntax to indicate a call to a base class constructor. Some classes define multiple constructors, 
         *and this syntax enables you to pick which base class constructor you call.*/
        public InterestEarningAccount(string name, decimal initialBalance) : base(name, initialBalance)
        {
        }
        public override void PerformMonthEndTransactions()
        {
            if (Balance > 500m)
            {
                var interest = Balance * 0.05m;
                MakeDeposit(interest, DateTime.Now, "apply monthly interest");
            }
        }
    }
}

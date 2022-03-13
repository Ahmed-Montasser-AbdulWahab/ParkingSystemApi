using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking_System.Classes
{
    public class Tariff
    {
        private double _costUnit = 0.5 ;
        public double CostUnit { get { return _costUnit; } set { _costUnit = value; } }

        public Tariff(double costUnit) { 
            this._costUnit = costUnit;
        }

        public double getTotalCost(double hours)
        {
            return _costUnit * hours;
        }
    }
}

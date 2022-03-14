using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking_System.Classes
{
    public class ParkingTransaction
    {
        private string _cID;
        private int _terminalID;
        private Boolean _result; //Granted:1 , NotGranted:0 
        private DateTime _dateTime;

        public ParkingTransaction(string cID, int terminalID, Boolean result, DateTime dateTime) {
            this._cID = cID;
            this._terminalID = terminalID;
            this._result = result;
            this._dateTime = dateTime;
        }
    }
}

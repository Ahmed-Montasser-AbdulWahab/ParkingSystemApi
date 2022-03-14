using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking_System.Classes
{
    public class ParkingArea
    {
        private string _name;

        //Relationships
        private List<Vehicle> vehicles = new List<Vehicle>() { }; //RelationShip
        private List<Operator> operators = new List<Operator>() { }; //RelationShip
        private List<Terminal> terminals = new List<Terminal>() { }; //RelationShip
        private List<ParkingTransaction> parkingTransactions = new List<ParkingTransaction> { };
        private List<Tariff> tariff = new List<Tariff>() { }; //RelationShip//RelationShip
        /*
         Note : 1) vehicles VS registered Vehicles ?
        2) How to transform 1...* or 0...* ? 
        */
        public ParkingArea(string name)
        {
              _name = name;
        }

        /*
         public void listenFromTerminal(){}
        */

        public Boolean CheckPairing()
        {
            return true;
        }

        public Boolean CheckRegistered()
        {
            return true;
        }

        public ParkingTransaction GenerateTransaction(string cID, int terminalID, Boolean result, DateTime dateTime)
        {
            return new ParkingTransaction(cID, terminalID, result, dateTime);
        }

        public void TransformToOperator()
        {

        }

        public double ComputeBill()
        {
            return 1.5;
        }

        public void sendCID_ToDatabase()
        {

        }

        public void Activate()
        {

        }
    }
}

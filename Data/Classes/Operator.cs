using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking_System.Classes
{
    public enum Status
    {
        online,
        offline
    }
    public class Operator
    {
        private string _userName;
        private string _password;
        private Status _status;

        public Operator(string userName, string password)
        {
            this._userName = userName;
            this._password = password;
            this._status = Status.offline;
        }
        public string UserName { get { return this._userName; } set { this._userName = value; } }
        public string Password { get { return this._password; } set { this._password = value; } }

        public Status Status { get { return this._status; } } // set { this._status = value; } }
        public Boolean Register() { return true; }
        public void Login() { 
            this._status = Status.online;
        }
        public void Logout() {
         this._status = Status.offline;
        }
        public Boolean checkCredentials() { return true; }
    }
}

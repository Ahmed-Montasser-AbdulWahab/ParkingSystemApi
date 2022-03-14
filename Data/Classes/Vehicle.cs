using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking_System.Classes
{
    public class Vehicle
    {
        private string _plateNumber;
        private string _brandName;
        private string _subCategory;
        private string _color;
        private DateTime _validFrom;
        private DateTime _validTo;
        private Boolean _presence = false ;
        private List<Participant> _participants = new List<Participant>() { }; //RelationShip




        public Vehicle(string plateNumber, string brandName, string subCategory, string color, DateTime validFrom, DateTime validTo) { 
            this._plateNumber = plateNumber;
            this._brandName = brandName;
            this._subCategory = subCategory;
            this._color = color;
            this._validFrom = validFrom;
            this._validTo = validTo;
        }
        public string PlateNumber { get { return _plateNumber; } }

        public Boolean Presence { get { return _presence; } }
        public Boolean CheckValidation()
        {
            if (DateTime.Now > _validFrom && DateTime.Now < _validTo)
            {
                return true; //Valid
            }
            return false; //not Valid
        }

        public Boolean Enter()
        {
            _presence = true;
            return _presence;
        }

        public Boolean Exit()
        {
            _presence = false;
            return _presence;
        }


    }
}

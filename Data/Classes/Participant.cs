using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking_System.Classes
{
    public class Participant
    {
        private int _participantID;
        private string _participantName;
        private string _faceID;
        private List<Vehicle> _vehicles = new List<Vehicle>() { }; //RelationShip
        public Participant(int id, string name, string f_id)
        {
            _participantID = id;
            _participantName = name;
            _faceID = f_id;
        }
        public int ParticipantID { get { return _participantID; } }
        public string ParticipantName { get { return _participantName; } }
        public string FaceID { get { return _faceID; } set { _faceID = value; } }
    }
}

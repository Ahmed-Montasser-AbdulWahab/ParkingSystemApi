using System.Collections.Generic;

namespace Parking_System_API.Data.Entities
{
    public class Terminal
    {
        public int Id { get; set; }
        public string ConnectionString { get; set; }
        public bool Service { get; set; } //Online 1 or Offline 0
        public bool Direction { get; set; }   //Entry 1 or Exit 0
        public Camera FaceRecognitionCamera { get; set; }
        public Camera LPCamera { get; set; }
        public Gate Gate { get; set; }
        public virtual ICollection<ParkingTransaction> ParkingTransactions { get; set; }
    }
}

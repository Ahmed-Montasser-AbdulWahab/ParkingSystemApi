namespace Parking_System_API.Data.Models
{
    public class ParticipantResponseModel
    {
        public string Id { get; set; }
        public long NationalId { get; set; }
        public string Name { get; set; }

        public string Email { get; set; }

        public VehicleResponseModel[] Vehicles { get; set; }
        public bool DoProvideFullData { get; set; }
        public bool DoProvidePhoto { get; set; }
        
        public string PhotoUrl { get; set; }
        public bool DoProvideVideo { get; set; }
        public bool DoDetected { get; set; }

        public bool Status
        { //0 : Not Activated , 1: Activated
            get; set;
        }
    }
}

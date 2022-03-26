namespace Parking_System_API.Data.Entities
{
    public class Gate
    {
        //ClosedState => 0 ,OpenedState => 1
        public int Id { get; set; }
        public bool Service { get; set; } //Online 1 -- Offline 0
        public bool State { get; set; } //open:1 close:0
        public Terminal Terminal { get; set; }
    }
}

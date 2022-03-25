namespace Parking_System_API.Data.Entities
{
    public class Gate
    {
        //ClosedState => 0 ,OpenedState => 1
        public int Id { get; set; }
        public bool Service { get; set; }
        public bool State { get; set; }
        public Terminal Terminal { get; set; }
    }
}

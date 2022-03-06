using System.ComponentModel.DataAnnotations;

namespace Parking_System_API.Data.Entities
{
    public class Constant
    {
        [Key]
        public int ID { get; set; }
        public string ConstantName { get; set; }
        public long Value { get; set; }
        public string StringValue   { get; set; }
    }
}

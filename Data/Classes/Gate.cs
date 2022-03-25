using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking_System.Classes
{
    public class Gate
    {
        //ClosedState => 0 ,OpenedState => 1
        private bool _state = false;
        public bool State { get { return _state; } }

        public Gate() { _state = false; }
        public bool Open()
        {
            _state = true;
            return this._state;
        }

        public bool Close()
        {
            _state = false;
            return this._state;
        }
    }
}

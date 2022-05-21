using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ROV_Wpf
{
    public class EngineCommand
    {
        public int pin_1;
        public int pin_2;
        public int speed;

        public EngineCommand(int pin_1, int pin_2, int speed)
        {
            this.pin_1 = pin_1;
            this.pin_2 = pin_2;
            this.speed = speed;
        }
    }
}

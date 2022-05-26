using SharpDX.DirectInput;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ROV_Wpf
{
    class Control
    {
        public static int get_y_value(JoystickUpdate state)
        {
            int speed_value;
            if (state.Value >= 0 && state.Value < 10000)
            {
                speed_value = 254;
            }
            else if (state.Value >= 10000 && state.Value < 28000)
            {
                speed_value = 127;
            }

            else if (state.Value > 32511 && state.Value < 58000)
            {
                speed_value = -127;
            }

            else if (state.Value >= 58000 && state.Value <= 65535)
            {
                speed_value = -254;
            }
            else
            {
                speed_value = 0;
            }
            return speed_value;
        }
    }
}

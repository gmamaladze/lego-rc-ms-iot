// This code is distributed under MIT license. 
// Copyright (c) 2014 George Mamaladze
// See license.txt or http://opensource.org/licenses/mit-license.php

#region usings

using Gma.Netmf.Hardware.Lego.PowerFunctions.Control;

#endregion

namespace Gma.Netmf.Hardware.Lego.PowerFunctions.Actuators
{
    public class Servo : Actuator
    {
        public Servo(Connector connector)
            : base(connector)
        {
        }

        public void SetAngle180(int angle)
        {
            var position = angle.FromAngle();
            RemoteControl.Execute(Output, position);
        }

        public void Center()
        {
            RemoteControl.Execute(Output, PwmSpeed.BreakThenFloat);
        }
    }
}
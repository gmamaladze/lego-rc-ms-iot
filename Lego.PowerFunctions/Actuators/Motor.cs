// This code is distributed under MIT license. 
// Copyright (c) 2014 George Mamaladze
// See license.txt or http://opensource.org/licenses/mit-license.php

#region usings

using Gma.Netmf.Hardware.Lego.PowerFunctions.Control;

#endregion

namespace Gma.Netmf.Hardware.Lego.PowerFunctions.Actuators
{
    public class Motor : Actuator
    {
        public Motor(Connector connector)
            : base(connector)
        {
        }

        public void SetSpeed(int percent)
        {
            var speed = percent.FromPercent();
            RemoteControl.Execute(Output, speed);
        }

        public void IncSpeed()
        {
            RemoteControl.Execute(Output, IncDec.IncrementPwm);
        }

        public void DecSpeed()
        {
            RemoteControl.Execute(Output, IncDec.DecrementPwm);
        }

        public void Brake()
        {
            RemoteControl.Execute(Output, PwmSpeed.BreakThenFloat);
        }
    }
}
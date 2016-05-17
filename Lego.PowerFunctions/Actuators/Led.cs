// This code is distributed under MIT license. 
// Copyright (c) 2014 George Mamaladze
// See license.txt or http://opensource.org/licenses/mit-license.php

using System;
using Gma.Netmf.Hardware.Lego.PowerFunctions.Control;

namespace Gma.Netmf.Hardware.Lego.PowerFunctions.Actuators
{
    public class Led : Actuator
    {
        public Led(Connector connector)
            : base(connector)
        {
        }

        public void Turn(bool on) {
            var value = on ? PwmSpeed.ForwardStep7 : PwmSpeed.BreakThenFloat;
            RemoteControl.Execute(Output, value);
        }

        public void TurnOn() {
            Turn(true);
        }
        public void TurnOff()
        {
            Turn(false);
        }
    }
}

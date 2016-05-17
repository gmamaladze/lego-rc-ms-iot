// This code is distributed under MIT license. 
// Copyright (c) 2014 George Mamaladze
// See license.txt or http://opensource.org/licenses/mit-license.php

#region usings

using Gma.Netmf.Hardware.Lego.PowerFunctions.Control;

#endregion

namespace Gma.Netmf.Hardware.Lego.PowerFunctions.Commands
{
    public struct ComboPwmCmd : ICommand
    {
        public ComboPwmCmd(PwmSpeed redSpeed, PwmSpeed blueSpeed)
            : this()
        {
            RedSpeed = redSpeed;
            BlueSpeed = blueSpeed;
        }

        public PwmSpeed RedSpeed { get; set; }
       
        public PwmSpeed BlueSpeed { get; set; }


        public CommandType CommandType
        {
            get { return CommandType.CompboPwm; }
        }

        public override string ToString()
        {
            return CommandType + " [" + RedSpeed + "," + BlueSpeed + "]";
        }
    }
}
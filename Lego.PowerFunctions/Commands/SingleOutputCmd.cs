// This code is distributed under MIT license. 
// Copyright (c) 2014 George Mamaladze
// See license.txt or http://opensource.org/licenses/mit-license.php

#region usings

using Gma.Netmf.Hardware.Lego.PowerFunctions.Communication;
using Gma.Netmf.Hardware.Lego.PowerFunctions.Control;

#endregion

namespace Gma.Netmf.Hardware.Lego.PowerFunctions.Commands
{
    public struct SingleOutputCmd : ICommand
    {
        public SingleOutputCmd(Output output, PwmSpeed speed)
            : this()
        {
            Output = output;
            SingleOutputMode = SingleOutputMode.Pwm;
            Data = (int) speed;
        }

        public SingleOutputCmd(Output output, IncDec incDec)
            : this()
        {
            Output = output;
            SingleOutputMode = SingleOutputMode.IncDec;
            Data = (int) incDec;
        }

        public CommandType CommandType
        {
            get { return CommandType.SingleOutput; }
        }

        public SingleOutputMode SingleOutputMode { get; set; }

        public Output Output { get; set; }

        public int Data { get; set; }

        public override string ToString()
        {
            return CommandType + " [" + Output + "," + Data + "]";
        }
    }
}
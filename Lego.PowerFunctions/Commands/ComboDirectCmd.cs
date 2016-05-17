// This code is distributed under MIT license. 
// Copyright (c) 2014 George Mamaladze
// See license.txt or http://opensource.org/licenses/mit-license.php

#region usings

using Gma.Netmf.Hardware.Lego.PowerFunctions.Control;

#endregion

namespace Gma.Netmf.Hardware.Lego.PowerFunctions.Commands
{
    public struct ComboDirectCmd : ICommand
    {

        public ComboDirectCmd(DirectState blueState, DirectState redState)
            : this()
        {
            BlueState = blueState;
            RedState = redState;
        }

        public DirectState BlueState { get; set; }

        public DirectState RedState { get; set; }

        public CommandType CommandType
        {
            get { return CommandType.ComboDirect; }
        }

        public override string ToString()
        {
            return CommandType + " [" + RedState + "," + BlueState + "]";
        }
    }
}
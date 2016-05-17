// This code is distributed under MIT license. 
// Copyright (c) 2014 George Mamaladze
// See license.txt or http://opensource.org/licenses/mit-license.php

#region usings

using Gma.Netmf.Hardware.Lego.PowerFunctions.Control;

#endregion

namespace Gma.Netmf.Hardware.Lego.PowerFunctions.Commands
{
    public struct ExtendedCmd : ICommand
    {
        public ExtendedCmd(ExtFunction extFunction) : this()
        {
            ExtFunction = extFunction;
        }

        public CommandType CommandType
        {
            get { return CommandType.Extended; }
        }

        public ExtFunction ExtFunction { get; set; }

        public override string ToString()
        {
            return CommandType + " [" + ExtFunction + "]";
        }
    }
}
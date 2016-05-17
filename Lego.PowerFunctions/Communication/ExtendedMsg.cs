// This code is distributed under MIT license. 
// Copyright (c) 2014 George Mamaladze
// See license.txt or http://opensource.org/licenses/mit-license.php

#region usings

using Gma.Netmf.Hardware.Lego.PowerFunctions.Commands;
using Gma.Netmf.Hardware.Lego.PowerFunctions.Control;

#endregion

namespace Gma.Netmf.Hardware.Lego.PowerFunctions.Communication
{
    internal class ExtendedMsg : Message
    {
        //  Extended mode message format
        //     +--------------+--------------+--------------+-------------+
        //     |   Nibble 1   |   Nibble 2   |   Nibble 3   |   LRC       |
        //     +--------------+--------------+--------------+-------------+
        //     |  T  E  C  C  |  a  0  0  0  |  F  F  F  F  | L  L  L  L  |
        //     +--------------+--------------+--------------+-------------+

        private readonly ExtendedCmd m_Command;

        public ExtendedMsg(Channel channel, Toggle toggle, ExtendedCmd command)
            : base(channel, toggle)
        {
            m_Command = command;
        }

        public override Escape Escape
        {
            get { return 0; }
        }

        protected override int GetNiblle2()
        {
            return Address << 3 | 0x0;
        }

        protected override int GetNiblle3()
        {
            return (int) m_Command.ExtFunction;
        }
    }
}
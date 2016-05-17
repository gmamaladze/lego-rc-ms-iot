// This code is distributed under MIT license. 
// Copyright (c) 2014 George Mamaladze
// See license.txt or http://opensource.org/licenses/mit-license.php

#region usings

using Gma.Netmf.Hardware.Lego.PowerFunctions.Commands;
using Gma.Netmf.Hardware.Lego.PowerFunctions.Control;

#endregion

namespace Gma.Netmf.Hardware.Lego.PowerFunctions.Communication
{
    internal class ComboPwmMsg : Message
    {
        //  Extended mode message format
        //     +--------------+--------------+--------------+-------------+
        //     |   Nibble 1   |   Nibble 2   |   Nibble 3   |   LRC       |
        //     +--------------+--------------+--------------+-------------+
        //     |  T  E  C  C  |  B  B  B  B  |  A  A  A  A  | L  L  L  L  |
        //     +--------------+--------------+--------------+-------------+

        private readonly ComboPwmCmd m_Command;

        public ComboPwmMsg(Channel channel, Toggle toggle, ComboPwmCmd command)
            : base(channel, toggle)
        {
            m_Command = command;
        }

        public override Escape Escape
        {
            get { return Escape.ComboPwmMode1; }
        }

        protected override int GetNiblle2()
        {
            return (byte) m_Command.BlueSpeed;
        }

        protected override int GetNiblle3()
        {
            return (byte) m_Command.RedSpeed;
        }
    }
}
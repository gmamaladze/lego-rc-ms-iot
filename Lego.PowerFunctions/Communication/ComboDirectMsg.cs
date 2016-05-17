// This code is distributed under MIT license. 
// Copyright (c) 2014 George Mamaladze
// See license.txt or http://opensource.org/licenses/mit-license.php

#region usings

using Gma.Netmf.Hardware.Lego.PowerFunctions.Commands;
using Gma.Netmf.Hardware.Lego.PowerFunctions.Control;

#endregion

namespace Gma.Netmf.Hardware.Lego.PowerFunctions.Communication
{
    internal class ComboDirectMsg : Message
    {
        //  Extended mode message format
        //     +--------------+--------------+--------------+-------------+
        //     |   Nibble 1   |   Nibble 2   |   Nibble 3   |   LRC       |
        //     +--------------+--------------+--------------+-------------+
        //     |  T  E  C  C  |  a  0  0  1  |  B  B  A  A  | L  L  L  L  |
        //     +--------------+--------------+--------------+-------------+

        private readonly ComboDirectCmd m_Command;

        public ComboDirectMsg(Channel channel, Toggle toggle, ComboDirectCmd command)
            : base(channel, toggle)
        {
            m_Command = command;
        }

        public override Escape Escape
        {
            get { return Escape.UseMode0; }
        }

        protected override int GetNiblle2()
        {
            return Address << 3 | 0x1; //001
        }

        protected override int GetNiblle3()
        {
            return (int) m_Command.BlueState << 2 | (int) m_Command.RedState;
        }
    }
}
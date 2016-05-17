// This code is distributed under MIT license. 
// Copyright (c) 2014 George Mamaladze
// See license.txt or http://opensource.org/licenses/mit-license.php

#region usings

using Gma.Netmf.Hardware.Lego.PowerFunctions.Commands;
using Gma.Netmf.Hardware.Lego.PowerFunctions.Control;

#endregion

namespace Gma.Netmf.Hardware.Lego.PowerFunctions.Communication
{
    internal class SingleOutputMsg : Message
    {
        //  Single output mode message format
        //     +--------------+--------------+--------------+-------------+
        //     |   Nibble 1   |   Nibble 2   |   Nibble 3   |   LRC       |
        //     +--------------+--------------+--------------+-------------+
        //     |  T  E  C  C  |  a  1  M  O  |  D  D  D  D  | L  L  L  L  |
        //     +--------------+--------------+--------------+-------------+

        private readonly SingleOutputCmd m_Command;

        public SingleOutputMsg(Channel channel, Toggle toggle, SingleOutputCmd command) : base(channel, toggle)
        {
            m_Command = command;
        }

        public override Escape Escape
        {
            get { return Escape.UseMode0; }
        }

        protected override int GetNiblle2()
        {
            return Address << 3 | 1 << 2 | (int) m_Command.SingleOutputMode << 1 | (int) m_Command.Output;
        }

        protected override int GetNiblle3()
        {
            return m_Command.Data;
        }
    }
}
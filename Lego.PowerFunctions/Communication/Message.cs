// This code is distributed under MIT license. 
// Copyright (c) 2014 George Mamaladze
// See license.txt or http://opensource.org/licenses/mit-license.php

#region usings

using System.Reflection;
using System.Text;
using Gma.Netmf.Hardware.Lego.PowerFunctions.Control;

#endregion

namespace Gma.Netmf.Hardware.Lego.PowerFunctions.Communication
{
    /// <summary>
    ///     Accomodates common code for all types of messages.
    /// </summary>
    internal abstract class Message
    {
        //  Generic message format
        //     +--------------+--------------+--------------+-------------+
        //     |   Nibble 1   |   Nibble 2   |   Nibble 3   |   LRC       |
        //     +--------------+--------------+--------------+-------------+
        //     |  T  E  C  C  |  ?  ?  ?  ?  |  ?  ?  ?  ?  | L  L  L  L  |
        //     +--------------+--------------+--------------+-------------+

        /// <summary>
        ///     The address bit is intended for enabling an extra set of 4 channels for future use. The current PF RC Receiver
        ///     expects by default the address bit to be 0.
        /// </summary>
        protected const byte Address = 0;

        private readonly Channel m_Channel;
        private readonly Toggle m_Toggle;

        protected Message(Channel channel, Toggle toggle)
        {
            m_Channel = channel;
            m_Toggle = toggle;
        }


        public Channel Channel
        {
            get { return m_Channel; }
        }

        public Toggle Toggle
        {
            get { return m_Toggle; }
        }

        public abstract Escape Escape { get; }

        public ushort GetData()
        {
            int nibble1 = GetNiblle1();
            int nibble2 = GetNiblle2();
            int nibble3 = GetNiblle3();
            int lrc = CalculateLrc(nibble1, nibble2, nibble3);
            var data = (ushort) ((nibble1 << 12) | (nibble2 << 8) | (nibble3 << 4) | lrc);
            return data;
        }

        private int GetNiblle1()
        {
            return ((byte) Toggle << 3) | ((byte) Escape << 2) | (byte) Channel;
        }

        protected abstract int GetNiblle2();

        protected abstract int GetNiblle3();

        private int CalculateLrc(int nibble1, int nibble2, int nibble3)
        {
            //Longitudinal Redundancy Check
            return 0xf ^ nibble1 ^ nibble2 ^ nibble3;
        }

        public override string ToString()
        {
            var result = new StringBuilder();
            var data = GetData();
            result.Append('|');
            for (int nibbleNr = 0; nibbleNr < 4; nibbleNr++)
            {
                for (int bitNr = 0; bitNr < 4; bitNr++)
                {
                    bool bit = ((data << (nibbleNr * 4 + bitNr)) & 0x8000) != 0;
                    char ch = bit ? '1' : '0';
                    result.Append(ch);
                }
                result.Append('|');
            }
            return result.ToString();
        }
    }
}
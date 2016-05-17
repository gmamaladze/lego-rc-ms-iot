// This code is distributed under MIT license. 
// Copyright (c) 2014 George Mamaladze
// See license.txt or http://opensource.org/licenses/mit-license.php

namespace Gma.Netmf.Hardware.Lego.PowerFunctions.Control
{
    public enum DirectState : byte
    {
        Float = 0x0,
        Forward = 0x1,
        Backward = 0x2,
        Brake = 0x3
    };
}
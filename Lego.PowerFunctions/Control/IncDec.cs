// This code is distributed under MIT license. 
// Copyright (c) 2014 George Mamaladze
// See license.txt or http://opensource.org/licenses/mit-license.php

namespace Gma.Netmf.Hardware.Lego.PowerFunctions.Control
{
    public enum IncDec
    {
        ToggleFullForward = 0,
        ToggleDirection = 1,
        IncrementNumericalPwm = 2,
        DecrementNumericalPwm = 3,
        IncrementPwm = 4,
        DecrementPwm = 5,
        FullForward = 6,
        FullBackward = 7,
        ToggleFullForwardBackward = 8,
        ClearC1 = 9,
        SetC1 = 10,
        ToggleC1 = 11,
        ClearC2 = 12,
        SetC2 = 13,
        ToggleC2 = 14,
        ToggleFullBackward = 15
    }
}
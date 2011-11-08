using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AwesomiumSharp;
using Microsoft.Xna.Framework.Input;

namespace Client.UI
{
    public class KeyMappings
    {
        public static VirtualKey MapKeys(Keys Key)
        {
            switch (Key)
            {
                case Keys.Back: return VirtualKey.BACK;
                case Keys.Delete: return VirtualKey.DELETE;
                case Keys.Tab: return VirtualKey.TAB;
                case Keys.OemClear: return VirtualKey.CLEAR;
                case Keys.Enter: return VirtualKey.RETURN;
                case Keys.Pause: return VirtualKey.PAUSE;
                case Keys.Escape: return VirtualKey.ESCAPE;
                case Keys.Space: return VirtualKey.SPACE;
                case Keys.NumPad0: return VirtualKey.NUMPAD0;
                case Keys.NumPad1: return VirtualKey.NUMPAD1;
                case Keys.NumPad2: return VirtualKey.NUMPAD2;
                case Keys.NumPad3: return VirtualKey.NUMPAD3;
                case Keys.NumPad4: return VirtualKey.NUMPAD4;
                case Keys.NumPad5: return VirtualKey.NUMPAD5;
                case Keys.NumPad6: return VirtualKey.NUMPAD6;
                case Keys.NumPad7: return VirtualKey.NUMPAD7;
                case Keys.NumPad8: return VirtualKey.NUMPAD8;
                case Keys.NumPad9: return VirtualKey.NUMPAD9;
                case Keys.Up: return VirtualKey.UP;
                case Keys.Down: return VirtualKey.DOWN;
                case Keys.Right: return VirtualKey.RIGHT;
                case Keys.Left: return VirtualKey.LEFT;
                case Keys.Insert: return VirtualKey.INSERT;
                case Keys.Home: return VirtualKey.HOME;
                case Keys.End: return VirtualKey.END;
                case Keys.PageUp: return VirtualKey.PRIOR;
                case Keys.PageDown: return VirtualKey.NEXT;
                case Keys.F1: return VirtualKey.F1;
                case Keys.F2: return VirtualKey.F2;
                case Keys.F3: return VirtualKey.F3;
                case Keys.F4: return VirtualKey.F4;
                case Keys.F5: return VirtualKey.F5;
                case Keys.F6: return VirtualKey.F6;
                case Keys.F7: return VirtualKey.F7;
                case Keys.F8: return VirtualKey.F8;
                case Keys.F9: return VirtualKey.F9;
                case Keys.F10: return VirtualKey.F10;
                case Keys.F11: return VirtualKey.F11;
                case Keys.F12: return VirtualKey.F12;
                case Keys.F13: return VirtualKey.F13;
                case Keys.F14: return VirtualKey.F14;
                case Keys.F15: return VirtualKey.F15;
                /*
                case Keys.DoubleQuote: return VirtualKey.OEM_7;
                case Keys.Hash: return VirtualKey.NUM_3;
                case Keys.Dollar: return VirtualKey.NUM_4;
                case Keys.Ampersand: return VirtualKey.NUM_7;
                case Keys.Quote: return VirtualKey.OEM_7;
                case Keys.LeftParen: return VirtualKey.NUM_9;
                case Keys.RightParen: return VirtualKey.NUM_0;
                case Keys.Asterisk: return VirtualKey.NUM_8;
                case Keys.Plus: return VirtualKey.OEM_PLUS;
                case Keys.Comma: return VirtualKey.OEM_COMMA;
                case Keys.Minus: return VirtualKey.OEM_MINUS;
                case Keys.Period: return VirtualKey.OEM_PERIOD;
                case Keys.Slash: return VirtualKey.OEM_2;
                case Keys.Colon: return VirtualKey.OEM_1;
                case Keys.Semicolon: return VirtualKey.OEM_1;
                case Keys.Less: return VirtualKey.OEM_COMMA;
                case Keys.Equals: return VirtualKey.OEM_PLUS;
                case Keys.Greater: return VirtualKey.OEM_PERIOD;
                case Keys.Question: return VirtualKey.OEM_2;
                case Keys.At: return VirtualKey.NUM_2;
                case Keys.LeftBracket: return VirtualKey.OEM_4;
                case Keys.Backslash: return VirtualKey.OEM_102;
                case Keys.RightBracket: return VirtualKey.OEM_6;
                case Keys.Caret: return VirtualKey.NUM_6;
                case Keys.Underscore: return VirtualKey.OEM_MINUS;
                case Keys.BackQuote: return VirtualKey.OEM_3;
                */
                case Keys.A: return VirtualKey.A;
                case Keys.B: return VirtualKey.B;
                case Keys.C: return VirtualKey.C;
                case Keys.D: return VirtualKey.D;
                case Keys.E: return VirtualKey.E;
                case Keys.F: return VirtualKey.F;
                case Keys.G: return VirtualKey.G;
                case Keys.H: return VirtualKey.H;
                case Keys.I: return VirtualKey.I;
                case Keys.J: return VirtualKey.J;
                case Keys.K: return VirtualKey.K;
                case Keys.L: return VirtualKey.L;
                case Keys.M: return VirtualKey.M;
                case Keys.N: return VirtualKey.N;
                case Keys.O: return VirtualKey.O;
                case Keys.P: return VirtualKey.P;
                case Keys.Q: return VirtualKey.Q;
                case Keys.R: return VirtualKey.R;
                case Keys.S: return VirtualKey.S;
                case Keys.T: return VirtualKey.T;
                case Keys.U: return VirtualKey.U;
                case Keys.V: return VirtualKey.V;
                case Keys.W: return VirtualKey.W;
                case Keys.X: return VirtualKey.X;
                case Keys.Y: return VirtualKey.Y;
                case Keys.Z: return VirtualKey.Z;
                case Keys.CapsLock: return VirtualKey.CAPITAL;
                case Keys.RightShift: return VirtualKey.RSHIFT;
                case Keys.LeftShift: return VirtualKey.LSHIFT;
                case Keys.RightControl: return VirtualKey.RCONTROL;
                case Keys.LeftControl: return VirtualKey.LCONTROL;
                case Keys.RightAlt: return VirtualKey.RMENU;
                case Keys.LeftAlt: return VirtualKey.LMENU;
                case Keys.LeftWindows: return VirtualKey.LWIN;
                case Keys.RightWindows: return VirtualKey.RWIN;
                case Keys.Help: return VirtualKey.HELP;
                case Keys.Print: return VirtualKey.PRINT;
                default: return VirtualKey.UNKNOWN;
            }
        }
        public static WebKeyModifiers MapModifiers()
        {
            int modifiers = 0;

            if (InputManager.IsKeyDown(Keys.LeftControl, false))
                modifiers |= (int)WebKeyModifiers.ControlKey;

            if (InputManager.IsKeyDown(Keys.LeftShift, false))
                modifiers |= (int)WebKeyModifiers.ShiftKey;

            if (InputManager.IsKeyDown(Keys.LeftControl, false))
                modifiers |= (int)WebKeyModifiers.AltKey;

            return (WebKeyModifiers)modifiers;
        }
    }
}

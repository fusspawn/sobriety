using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Client.UI;

namespace Client
{
    public class InputManager
    {
        public static MouseState CurrentMouseState;
        public static MouseState StaleMouseState;
        public static KeyboardState CurrentKeyboardState;
        public static KeyboardState StaleKeyboardState;

        public static float ScrollState;

        public static int GetCurrentScroll() {
            return CurrentMouseState.ScrollWheelValue;
        }

        public static int GetStaleScroll() {
            return StaleMouseState.ScrollWheelValue;
        }

        public static int GetScrollDelta() {
            return GetStaleScroll() - GetCurrentScroll();
        }

        public static bool IsKeyPressed(Keys K, bool UILayer = false) {
            if (UIManager.HasKeyboardFocus
                && !UILayer)
                return false;

            return (CurrentKeyboardState.IsKeyDown(K)
                && StaleKeyboardState.IsKeyUp(K));
        }

        public static bool IsKeyHeld(Keys K, bool UILayer = false) {
            if (UIManager.HasKeyboardFocus
                && !UILayer)
                return false;

            return (CurrentKeyboardState.IsKeyDown(K) 
                && StaleKeyboardState.IsKeyDown(K));
        }

        public static bool IsLeftButtonClick() {
            return (CurrentMouseState.LeftButton == ButtonState.Pressed 
                && StaleMouseState.LeftButton == ButtonState.Released);
        }

        public static bool IsRightButtonClick() {
            return (CurrentMouseState.RightButton == ButtonState.Pressed 
                && StaleMouseState.RightButton == ButtonState.Released);
        }

        public static bool IsKeyDown(Keys K, bool UILayer = false) {
            
            if (UIManager.HasKeyboardFocus
                && !UILayer)
                return false;

            return CurrentKeyboardState.IsKeyDown(K);
        }

        public static void Update() {
            if (StaleMouseState == null)
                StaleMouseState = Mouse.GetState();
            if (StaleKeyboardState == null)
                StaleKeyboardState = Keyboard.GetState();

            CurrentMouseState = Mouse.GetState();
            CurrentKeyboardState = Keyboard.GetState();
        }

        public static void PostUpdate() {
            StaleKeyboardState = CurrentKeyboardState;
            StaleMouseState = CurrentMouseState;
        }


        public static Vector2 MouseWorldLocation()
        {
            float MouseWorldX = (Mouse.GetState().X - GameClient.GDevice.Viewport.Width * 0.5f + (GameClient.Camera.Pos.X) * (float)Math.Pow((GameClient.Camera.Zoom), 1)) /
            (float)Math.Pow(GameClient.Camera.Zoom, 1);
            float MouseWorldY = ((Mouse.GetState().Y - GameClient.GDevice.Viewport.Height * 0.5f + (GameClient.Camera.Pos.Y) * (float)Math.Pow((GameClient.Camera.Zoom), 1))) /
            (float)Math.Pow(GameClient.Camera.Zoom, 1);
            return new Vector2(MouseWorldX, MouseWorldY);
        }
    }
}

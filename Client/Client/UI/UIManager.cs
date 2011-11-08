using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AwesomiumSharp;
using AwesomiumSharpXna;
using System.Reflection;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Client.Native;

namespace Client.UI
{
    public class UIManager
    {
        private static WebView UILayer;
        private static SpriteBatch Batch;
        private static Texture2D RenderBuffer;
        private static Rectangle ScreenRect;

        public static void Init(GraphicsDevice Device) {
            Batch = new SpriteBatch(Device);
            RenderBuffer = new Texture2D(Device,
                Device.Viewport.Width, 
                Device.Viewport.Height);

            ConfigureWebCore();

            ScreenRect = new Rectangle()
            {
                X = 0,
                Y = 0,
                Width = Device.Viewport.Width,
                Height = Device.Viewport.Height
            };


            UILayer = WebCore.CreateWebView(Device.Viewport.Width,
                Device.Viewport.Height, false);
            UILayer.SetTransparent(true);
            UILayer.LoadFile("UIHolder.html");
            UILayer.LoadCompleted += new EventHandler(UILayer_LoadCompleted);
            UILayer.JSConsoleMessageAdded +=new JSConsoleMessageAddedEventHandler(UILayer_JSConsoleMessageAdded);
            UILayer.FlushAlpha = false;
            UILayer.Focus();
            ConfigureAPILayer();

            EventInput.CharEntered += new CharEnteredHandler(EventInput_CharEntered);
            EventInput.KeyDown += new KeyEventHandler(EventInput_KeyDown);
            EventInput.KeyUp += new KeyEventHandler(EventInput_KeyUp);
        }

        private static void ConfigureAPILayer()
        {
            UIApi.UIWindow = UILayer;
            UILayer.CreateObject("Native");
            UILayer.SetObjectCallback("Native", "SendChatMessage", new JSCallback(UIApi.SendChatMessage));
        }

        static void UILayer_LoadCompleted(object sender, EventArgs e)
        {
            Console.WriteLine("UIManager: PageLoaded.");
        }

        static void EventInput_KeyUp(object sender, KeyEventArgs e)
        {
            WebKeyboardEvent Event = new WebKeyboardEvent();
            Event.Type = WebKeyType.KeyUp;
            Event.VirtualKeyCode = KeyMappings.MapKeys(e.KeyCode);
            Event.Modifiers = KeyMappings.MapModifiers();
            UILayer.InjectKeyboardEvent(Event);
        }
        static void EventInput_KeyDown(object sender, KeyEventArgs e)
        {
            WebKeyboardEvent Event = new WebKeyboardEvent();
            Event.Type = WebKeyType.KeyDown;
            Event.VirtualKeyCode = KeyMappings.MapKeys(e.KeyCode);
            Event.Modifiers = KeyMappings.MapModifiers();
            UILayer.InjectKeyboardEvent(Event);
        }
        static void EventInput_CharEntered(object sender, CharacterEventArgs e)
        {
            WebKeyboardEvent Event = new WebKeyboardEvent();
            Event.Type = WebKeyType.Char;
            Event.NativeKeyCode = e.Character;
            Event.Text = new ushort[] { e.Character, 0, 0, 0 };
            Event.Modifiers = KeyMappings.MapModifiers();
            UILayer.InjectKeyboardEvent(Event);
        }
        static void ConfigureWebCore()
        {
            EventInput.Initialize(GameClient.GameWindow);

            // Setup WebConfig Options
            WebCoreConfig Config = new WebCoreConfig();
            Config.EnablePlugins = true;
            Config.EnableDatabases = true;
            Console.WriteLine("WebUI: Configured");

            // Initialize HTML system
            WebCore.Initialize(Config, true);
            WebCore.BaseDirectory = Path.GetDirectoryName(
                    Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName
                ) + "\\UI\\UIStatic\\";   

            Console.WriteLine("WebUI: Initialized");
        }
        public static bool HasKeyboardFocus { 
            get { return UILayer.HasKeyboardFocus; }
        }

        public static void Update(GameTime Time) {
            WebCore.Update();
            UpdateMouseInput();
        }
        static void UpdateMouseInput()
        {
            Vector2 MouseDelta = new Vector2(InputManager.CurrentMouseState.X, InputManager.CurrentMouseState.Y);
            UILayer.JSConsoleMessageAdded += new JSConsoleMessageAddedEventHandler(UILayer_JSConsoleMessageAdded);
            UILayer.InjectMouseMove((int)MouseDelta.X, (int)MouseDelta.Y);

            if (InputManager.IsLeftButtonClick()) 
                UILayer.InjectMouseDown(MouseButton.Left);

            if (InputManager.IsLeftButtonClick())
                UILayer.InjectMouseUp(MouseButton.Left);

            if (InputManager.IsRightButtonClick())
                UILayer.InjectMouseDown(MouseButton.Right);

            if (InputManager.IsRightButtonClick())
                UILayer.InjectMouseUp(MouseButton.Right);

            UILayer.InjectMouseWheel(InputManager.GetScrollDelta());
        }
        static void UILayer_JSConsoleMessageAdded(object sender, JSConsoleMessageEventArgs e)
        {
            Console.WriteLine(String.Format(
                "(JSConsole) Line: {0} Message: {1} Source: {2}",
                e.LineNumber,
                e.Message,
                e.Source
                ));
        }

        public static void Draw() {
            if (UILayer.IsDirty) {
                UILayer.RenderTexture2D(RenderBuffer);
            }

            Batch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            Batch.Draw(RenderBuffer, ScreenRect, Color.White);
            Batch.End();
        }
    }
}

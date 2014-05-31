using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SFML;
using SFML.Window;

namespace Jeden.Engine
{
    /// <summary>
    /// Provides information about player input.
    /// </summary>
    public class InputManager
    {
        Dictionary<Keyboard.Key, bool> KeyboardState { get; set; }
        Dictionary<Keyboard.Key, bool> PreviousKeyboardState { get; set; }
        IEnumerable<Keyboard.Key> keysEnum = Enum.GetValues(typeof(Keyboard.Key)).Cast<Keyboard.Key>();

        Dictionary<Mouse.Button, bool> MouseState { get; set; }
        Dictionary<Mouse.Button, bool> PreviousMouseState { get; set; }
        IEnumerable<Mouse.Button> buttonsEnum = Enum.GetValues(typeof(Mouse.Button)).Cast<Mouse.Button>();

        //public int DeltaScrollWheel { get; set; } //not really needed

        public Vector2i MousePosition { get; set; }
        public Vector2i PreviousMousePosition { get; set; }

        public InputManager(GameEngine engine) 
        {
            KeyboardState = new Dictionary<Keyboard.Key,bool>();
            PreviousKeyboardState = new Dictionary<Keyboard.Key,bool>();

            MouseState = new Dictionary<Mouse.Button,bool>();
            PreviousMouseState = new Dictionary<Mouse.Button,bool>();

            MousePosition = new Vector2i(0,0);
            PreviousMousePosition = new Vector2i(0,0);

            //game.Window.MouseWheelMoved += Window_MouseWheelMoved;
            
            foreach (Keyboard.Key key in keysEnum) 
            {
                KeyboardState.Add(key,false);
                PreviousKeyboardState.Add(key, false);
            }

            foreach (Mouse.Button mouseButton in buttonsEnum) 
            {
                MouseState.Add(mouseButton, false);
                PreviousMouseState.Add(mouseButton, false);
            }
        }

        public void Update(GameEngine engine) 
        {
            //Console.WriteLine(DeltaScrollWheel);
            //DeltaScrollWheel = 0; //we only want the difference for each frame, the total value is irrelevant

            PreviousKeyboardState.Clear();
            PreviousMouseState.Clear();
            PreviousMousePosition = MousePosition;

            MousePosition = Mouse.GetPosition(engine.Window);
            foreach (KeyValuePair<Keyboard.Key,bool> valuePair in KeyboardState) 
            {
                PreviousKeyboardState.Add(valuePair.Key, valuePair.Value);
            }
            foreach (KeyValuePair<Mouse.Button, bool> valuePair in MouseState) 
            {
                PreviousMouseState.Add(valuePair.Key, valuePair.Value);
            }

            KeyboardState.Clear();
            MouseState.Clear();
            foreach (Keyboard.Key key in keysEnum) 
            {
                KeyboardState.Add(key, CheckKey(key));
            }
            foreach (Mouse.Button button in buttonsEnum) 
            {
                MouseState.Add(button, CheckButton(button));
            }
        }

        private bool CheckKey(Keyboard.Key key) 
        {
            return Keyboard.IsKeyPressed(key);
        }
        private bool CheckButton(Mouse.Button button) 
        {
            return Mouse.IsButtonPressed(button);
        }

        public bool IsKeyReleased(Keyboard.Key key)
        {
            return WasKeyDown(key) && IsKeyUp(key);
        }
        public bool IsKeyPressed(Keyboard.Key key)
        {
            return IsKeyDown(key) && WasKeyUp(key);
        }
        public bool IsKeyUp(Keyboard.Key key)
        {
            return !IsKeyDown(key);
        }
        public bool IsKeyDown(Keyboard.Key key)
        {
            return KeyboardState[key];
        }
        public bool WasKeyUp(Keyboard.Key key)
        {
            return !WasKeyDown(key);
        }
        public bool WasKeyDown(Keyboard.Key key)
        {
            return PreviousKeyboardState[key];
        }

        public bool IsButtonReleased(Mouse.Button button)
        {
            return WasButtonDown(button) && IsButtonUp(button);
        }
        public bool IsButtonPressed(Mouse.Button button)
        {
            return IsButtonDown(button) && WasButtonUp(button);
        }
        public bool IsButtonUp(Mouse.Button button)
        {
            return !IsButtonDown(button);
        }
        public bool IsButtonDown(Mouse.Button button)
        {
            return MouseState[button];
        }
        public bool WasButtonUp(Mouse.Button button)
        {
            return !WasButtonDown(button);
        }
        public bool WasButtonDown(Mouse.Button button)
        {
            return PreviousMouseState[button];
        }

        //void Window_MouseWheelMoved(object sender, MouseWheelEventArgs e)
        //{
        //    DeltaScrollWheel += e.Delta;
        //}
        //queues events so they can be used in the Update Cycle of a UserInputComponent
    }
}

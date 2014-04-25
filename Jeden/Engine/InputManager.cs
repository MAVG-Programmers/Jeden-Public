using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML;
using SFML.Window;

namespace Jeden.Engine
{
    /// <summary>
    /// Provides information about player input.
    /// </summary>
    public class InputManager
    {
        /// <summary>
        /// The current state of the keyboard.
        /// </summary>
        Dictionary<Keyboard.Key, bool> KeyboardState { get; set; }
        /// <summary>
        /// The previous state of the keyboard.
        /// </summary>
        Dictionary<Keyboard.Key, bool> PreviousKeyboardState { get; set; }

        /// <summary>
        /// The keys of the keyboard.
        /// </summary>
        IEnumerable<Keyboard.Key> keysEnum = Enum.GetValues(typeof(Keyboard.Key)).Cast<Keyboard.Key>();

        /// <summary>
        /// The current state of the mouse.
        /// </summary>
        Dictionary<Mouse.Button, bool> MouseState { get; set; }

        /// <summary>
        /// The previous state of the mouse.
        /// </summary>
        Dictionary<Mouse.Button, bool> PreviousMouseState { get; set; }

        /// <summary>
        /// The buttons of the mouse.
        /// </summary>
        IEnumerable<Mouse.Button> buttonsEnum = Enum.GetValues(typeof(Mouse.Button)).Cast<Mouse.Button>();

        //public int DeltaScrollWheel { get; set; } //not really needed

        /// <summary>
        /// The current position of the mouse.
        /// </summary>
        public Vector2i MousePosition { get; set; }

        /// <summary>
        /// The previous position of the mouse.
        /// </summary>
        public Vector2i PreviousMousePosition { get; set; }

        /// <summary>
        /// A new instance of InputManager
        /// </summary>
        /// <param name="engine">The GameEngine that manages this InputManager</param>
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

        /// <summary>
        /// Updates the InputManager
        /// </summary>
        /// <param name="engine">The GameEngine that manages this InputManager</param>
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

        /// <summary>
        /// Checks whether a key is down.
        /// </summary>
        /// <param name="key">The checked key.</param>
        /// <returns>True if key is pressed; false otherwise.</returns>
        private bool CheckKey(Keyboard.Key key) 
        {
            return Keyboard.IsKeyPressed(key);
        }

        /// <summary>
        /// Checks whether the specified mouse button is clicked
        /// </summary>
        /// <param name="button">The checked mouse button.</param>
        /// <returns>True if button is pressed; false otherwise.</returns>
        private bool CheckButton(Mouse.Button button) 
        {
            return Mouse.IsButtonPressed(button);
        }

        /// <summary>
        /// Checks whether the specified key is released.
        /// </summary>
        /// <param name="key">The key that is checked.</param>
        /// <returns>True if key is released; false otherwise.</returns>
        public bool IsKeyReleased(Keyboard.Key key)
        {
            return WasKeyDown(key) && IsKeyUp(key);
        }

        /// <summary>
        /// Checks whether the specified key is pressed.
        /// </summary>
        /// <param name="key">The key that is checked.</param>
        /// <returns>True if the key is pressed; false otherwise.</returns>
        public bool IsKeyPressed(Keyboard.Key key)
        {
            return IsKeyDown(key) && WasKeyUp(key);
        }
        /// <summary>
        /// Checks whether the specified key is up.
        /// </summary>
        /// <param name="key">The key that is checked.</param>
        /// <returns>True if the key is up; false otherwise.</returns>
        public bool IsKeyUp(Keyboard.Key key)
        {
            return !IsKeyDown(key);
        }

        /// <summary>
        /// Checks whether the specified key is down.
        /// </summary>
        /// <param name="key">The key that is checked.</param>
        /// <returns>True if key is down; false otherwise.</returns>
        public bool IsKeyDown(Keyboard.Key key)
        {
            return KeyboardState[key];
        }

        /// <summary>
        /// Checks whether the specified key was up in the last frame.
        /// </summary>
        /// <param name="key">The key that is checked.</param>
        /// <returns>True if key was up; false otherwise.</returns>
        public bool WasKeyUp(Keyboard.Key key)
        {
            return !WasKeyDown(key);
        }

        /// <summary>
        /// Checks whether the specified key was down in the last frame.
        /// </summary>
        /// <param name="key">The key that is checked.</param>
        /// <returns>True if key was down; false otherwise.</returns>
        public bool WasKeyDown(Keyboard.Key key)
        {
            return PreviousKeyboardState[key];
        }

        /// <summary>
        /// Checks if the specified button is released.
        /// </summary>
        /// <param name="button">The button that is checked.</param>
        /// <returns>True if button is released; false otherwise.</returns>
        public bool IsButtonReleased(Mouse.Button button)
        {
            return WasButtonDown(button) && IsButtonUp(button);
        }
        /// <summary>
        /// Checks if the specified button is pressed.
        /// </summary>
        /// <param name="button">The button that is checked.</param>
        /// <returns>True if button is pressed; false otherwise.</returns>
        public bool IsButtonPressed(Mouse.Button button)
        {
            return IsButtonDown(button) && WasButtonUp(button);
        }

        /// <summary>
        /// Checks whether the specified button is up.
        /// </summary>
        /// <param name="button">The button that is checked.</param>
        /// <returns>True if button is up; false otherwise.</returns>
        public bool IsButtonUp(Mouse.Button button)
        {
            return !IsButtonDown(button);
        }

        /// <summary>
        /// Checks whether the specified button is down.
        /// </summary>
        /// <param name="button">The button that is checked.</param>
        /// <returns>True if button is down; false otherwise.</returns>
        public bool IsButtonDown(Mouse.Button button)
        {
            return MouseState[button];
        }

        /// <summary>
        /// Checks whether the specified button was up in the last frame.
        /// </summary>
        /// <param name="button">The button that is checked.</param>
        /// <returns>True if button was up in the last frame; false otherwise.</returns>
        public bool WasButtonUp(Mouse.Button button)
        {
            return !WasButtonDown(button);
        }

        /// <summary>
        /// Checks whether the specified button was down in the last frame.
        /// </summary>
        /// <param name="button">The button that is checked.</param>
        /// <returns>True if button was down in the last frame; false otherwise.</returns>
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

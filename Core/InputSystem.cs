using System.Collections.Generic;
using Gwen.Input;
using OpenTK;
using OpenTK.Input;

namespace Substructio.Core
{
	public static class InputSystem
	{
		public static List<Key> CurrentKeys = new List<Key>();
		public static List<Key> NewKeys = new List<Key>();
		public static List<Key> ReleasedKeys = new List<Key>(); 
		public static List<char> PressedChars = new List<char>();

		public static List<MouseButton> ReleasedButtons = new List<MouseButton>();
		public static List<MouseButton> CurrentButtons = new List<MouseButton>();
		public static List<MouseButton> PressedButtons = new List<MouseButton>();
		public static List<MouseButton> UnHandledButtons = new List<MouseButton>();

		public static float MouseWheelDelta { get; private set; }

		public static Vector2 RawMouseDelta { get; private set; }
		public static Vector2 RawMouseXY { get; private set; }
		public static Vector2 RawMousePreviousXY { get; private set; }

		public static Vector2 MouseDelta { get; private set; }
		public static Vector2 MousePreviousXY { get; private set; } 
		public static Vector2 MouseXY { get; private set; }

		public static bool HasMouseMoved { get; private set; }

		public static bool Focused;

		private static List<OpenTKAlternative> _guiInputDevices = new List<OpenTKAlternative>(); 

		public static void KeyPressed(KeyPressEventArgs e)
		{
			if (Focused)
				PressedChars.Add(e.KeyChar);
		}

		public static void KeyDown(KeyboardKeyEventArgs e)
		{
			if (Focused) {

				if (!NewKeys.Contains(e.Key) && !CurrentKeys.Contains(e.Key)) {
					NewKeys.Add(e.Key);
				} 
				if (!CurrentKeys.Contains(e.Key)) {
					CurrentKeys.Add(e.Key);
				}
			}
		}

		public static void KeyUp(KeyboardKeyEventArgs e)
		{
			if (Focused) {
				if (CurrentKeys.Contains(e.Key)) {
					CurrentKeys.Remove(e.Key);
				}
				if (!ReleasedKeys.Contains(e.Key))
					ReleasedKeys.Add(e.Key);
			}
		}

		public static void MouseDown(MouseButtonEventArgs e)
		{
			if (Focused) {
				if (!CurrentButtons.Contains(e.Button)) {
					CurrentButtons.Add(e.Button);
				}
				if (!UnHandledButtons.Contains(e.Button)) {
					UnHandledButtons.Add(e.Button);
				}
				if (!PressedButtons.Contains(e.Button)) {
					PressedButtons.Add(e.Button);
				} 
			}
		}

		public static void MouseUp(MouseButtonEventArgs e)
		{
			if (Focused) {
				if (CurrentButtons.Contains(e.Button)) {
					CurrentButtons.Remove(e.Button);
				}
				if (!ReleasedButtons.Contains(e.Button))
				{
					ReleasedButtons.Add(e.Button);
				}
			}
		}

		public static void AddGUIInput(OpenTKAlternative input)
		{
			if (!_guiInputDevices.Contains(input))
				_guiInputDevices.Add(input);
		}

		public static void RemoveGUIInput(OpenTKAlternative input)
		{
			if (_guiInputDevices.Contains(input))
				_guiInputDevices.Remove(input);
		}

		public static bool IsKeyDown(Key k)
		{
			return CurrentKeys.Contains(k);
		}

		public static bool IsMouseButtonClicked(MouseButton button)
		{
			return Mouse.GetState().IsButtonDown(button);
		}

		public static void Update(bool focused)
		{
			Focused = focused;

			UpdateGUIInputs();

			//handle raw mouse state
			var mstate = Mouse.GetState();
			RawMousePreviousXY = RawMouseXY;
			RawMouseXY = new Vector2(mstate.X, mstate.Y);
			RawMouseDelta = Vector2.Subtract(RawMouseXY, RawMousePreviousXY);

			//reset mouse variables 
			MouseWheelDelta = 0;
			HasMouseMoved = false;

			PressedChars.Clear();
			PressedButtons.Clear();
			ReleasedButtons.Clear();
			UnHandledButtons.Clear();
			NewKeys.Clear();
			ReleasedKeys.Clear();

		}

		private static void UpdateGUIInputs()
		{
			foreach (OpenTKAlternative gInput in _guiInputDevices)
			{
				foreach (var pressedButton in PressedButtons)
				{
					gInput.ProcessMouseButton(pressedButton, true);
				}
				foreach (var releasedButton in ReleasedButtons)
				{
					gInput.ProcessMouseButton(releasedButton, false);
				}
				gInput.ProcessMouseWheel(-(int)MouseWheelDelta*10);
				if (HasMouseMoved)
					gInput.ProcessMouseMove((int)MouseXY.X, (int)MouseXY.Y);
				foreach (var newKey in NewKeys)
				{
					gInput.ProcessKeyDown(newKey);
				}
				foreach (var releasedKey in ReleasedKeys)
				{
					gInput.ProcessKeyUp(releasedKey);
				}
				foreach (var pressedChar in PressedChars)
				{
					gInput.KeyPress(pressedChar);
				}
			}
		}

		public static void MouseWheelChanged(MouseWheelEventArgs e)
		{
			MouseWheelDelta += -e.DeltaPrecise;
		}

		public static void MouseMoved(MouseMoveEventArgs e)
		{
			MousePreviousXY = MouseXY;
			MouseXY = new Vector2(e.X, e.Y);
			MouseDelta = Vector2.Subtract(MouseXY, MousePreviousXY);
			HasMouseMoved = true;
		}
	}
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GS.Player
{
    public class InputListener : MonoBehaviour {

		#region Attributes

		private static Vector3 oldMousePosition = Vector3.zero;

		#endregion // Attributes

		#region Interface events

		private static bool inMenu;
		public static event System.Action menuToggle;
		public static event System.Action menuApply;
		public static event System.Action menuCancel;

		public static event System.Action<float> menuScroll;
		public static event System.Action<float> menuHorizontal;
		public static event System.Action<float> menuVertical;
		public static event System.Action<float> menuSwitchPanel;
		
		#endregion // Interface events

		#region Action events

		public static event System.Action fire1;
		public static event System.Action fire2;
		public static event System.Action fire3;

		public static event System.Action<float> scroll;
		public static event System.Action<float> moveHorizontal;
		public static event System.Action<float> moveVertical;
		public static event System.Action<float> pivotHorizontal;
		public static event System.Action<float> pivotVertical;

		#endregion // Action events

		private void LaunchBoolEvent(bool test, System.Action action) {
			if(test && action != null) { action.Invoke(); }
		}
        private void LaunchFloatEvent(float value, System.Action<float> action) {
			if(value != 0f && action != null) { action.Invoke(value); }
		}

		#region MouseAndKeyboard

			#region MouseMethods

			void CheckCursorState() {
				// Check cursor mode based on window mode and share the result with things that require it.
				switch(Screen.fullScreenMode) {
					case FullScreenMode.ExclusiveFullScreen:
						//Fullscreen mode.
						Cursor.lockState = (inMenu ? CursorLockMode.Confined : CursorLockMode.Locked);
						break;
					case FullScreenMode.FullScreenWindow:
						//Borderless window mode.
						Cursor.lockState = (inMenu ? CursorLockMode.None : CursorLockMode.Locked);
						break;
					default:
						//Maximized window, windowed and other modes.
						Cursor.lockState = (inMenu ? CursorLockMode.None : CursorLockMode.Locked);
						break;
				}
			}

			#endregion // MouseMethods

			#region KeyMethods

			void CheckMenuKeys(){
				// Menu toggle checked in CheckMouseAndKeyboard.
				LaunchBoolEvent(Input.GetKeyDown(KeyCode.Y), menuApply);
				LaunchBoolEvent(Input.GetKeyDown(KeyCode.N), menuCancel);

				LaunchFloatEvent(Input.GetAxis("Mouse ScrollWheel"), menuScroll);
				LaunchFloatEvent(Input.GetAxis("Horizontal"), menuHorizontal);
				LaunchFloatEvent(Input.GetAxis("Vertical"), menuVertical);
				LaunchFloatEvent((Input.GetKeyDown(KeyCode.Tab) ? 1f : 0f), menuSwitchPanel);
			}

			void CheckGameKeys() {
				LaunchBoolEvent(Input.GetKey(KeyCode.Mouse0), fire1);
				LaunchBoolEvent(Input.GetKey(KeyCode.Mouse1), fire2);
				LaunchBoolEvent(Input.GetKey(KeyCode.Mouse2), fire3);
				LaunchFloatEvent(Input.GetAxis("Horizontal"), moveHorizontal);
				LaunchFloatEvent(Input.GetAxis("Vertical"), moveVertical);
				LaunchFloatEvent(Input.GetAxis("Mouse X"), pivotHorizontal);
				LaunchFloatEvent(Input.GetAxis("Mouse Y"), pivotVertical);
				LaunchFloatEvent(Input.GetAxis("Mouse ScrollWheel"), scroll);
			}

			#endregion // KeyMethods

		void CheckMouseAndKeyboard() {
			if(Input.GetKeyDown(KeyCode.Escape)) {
				inMenu = !inMenu;
				//Cursor.visible = inMenu;
				CheckCursorState();
				if(menuToggle != null) { menuToggle.Invoke(); }
			}

			if(inMenu) { CheckMenuKeys(); }
			else { CheckGameKeys(); }
		}

		#endregion // MouseAndKeyboard

		#region Controller

		void CheckWindowsControls() {

			if(Input.GetAxis("Start") != 0) {
				inMenu = !inMenu;
				//Cursor.visible = inMenu;
				if(menuToggle != null) { menuToggle.Invoke(); }
			} 

			// TODO
			#region WindowsMenuControls

			LaunchBoolEvent(Input.GetAxis("Submit") != 0, menuApply);
			LaunchBoolEvent(Input.GetAxis("Cancel") != 0, menuCancel);

			LaunchFloatEvent(Input.GetAxis("Horizontal"), menuHorizontal);
			LaunchFloatEvent(Input.GetAxis("Vertical"), menuVertical);
			LaunchFloatEvent(Input.GetAxis("VerticalPivot"), menuScroll);
			LaunchFloatEvent(Input.GetAxis("Bumpers"), menuSwitchPanel);

			#endregion // WindowsMenuControls

			// TODO
			#region WindowsGameControls
			
			LaunchBoolEvent(Input.GetAxis("Fire1") != 0, fire1);
			LaunchBoolEvent(Input.GetAxis("Fire2") != 0, fire2);
			LaunchBoolEvent(Input.GetAxis("Fire3") != 0, fire3);
			LaunchFloatEvent(Input.GetAxis("Horizontal"), moveHorizontal);
			LaunchFloatEvent(Input.GetAxis("Vertical"), moveHorizontal);
			LaunchFloatEvent(Input.GetAxis("HorizontalPivot"), pivotHorizontal);
			LaunchFloatEvent(Input.GetAxis("VerticalPivot"), pivotVertical);
			LaunchFloatEvent(Input.GetAxis("Bumpers"), scroll);

			#endregion // WindowsGameControls
		}

		void CheckLinuxControls() {

			// TODO
			#region LinuxMenuControls

			LaunchBoolEvent(Input.GetKeyDown(KeyCode.Y), menuApply);
			LaunchBoolEvent(Input.GetKeyDown(KeyCode.N), menuCancel);

			LaunchFloatEvent(Input.GetAxis("Mouse ScrollWheel"), menuScroll);
			LaunchFloatEvent(Input.GetAxis("Horizontal"), menuHorizontal);
			LaunchFloatEvent(Input.GetAxis("Vertical"), menuVertical);
			LaunchFloatEvent((Input.GetKeyDown(KeyCode.Tab) ? 1f : 0f), menuSwitchPanel);

			#endregion // LinuxMenuControls

			// TODO
			#region LinuxGameControls
			
			LaunchBoolEvent(Input.GetKey(KeyCode.Mouse0), fire1);
			LaunchBoolEvent(Input.GetKey(KeyCode.Mouse1), fire2);
			LaunchBoolEvent(Input.GetKey(KeyCode.Mouse2), fire3);
			LaunchFloatEvent(Input.GetAxis("Horizontal"), moveHorizontal);
			LaunchFloatEvent(Input.GetAxis("Vertical"), moveHorizontal);
			LaunchFloatEvent(Input.GetAxis("HorizontalPivotWindows"), pivotHorizontal);
			LaunchFloatEvent(Input.GetAxis("VerticalPivotWindows"), pivotVertical);
			LaunchFloatEvent(Input.GetAxis("BumpersWindows"), scroll);

			#endregion // LinuxGameControls
		}

		void CheckMacControls() {
			
			// TODO
			#region MacMenuControls

			LaunchBoolEvent(Input.GetKeyDown(KeyCode.Y), menuApply);
			LaunchBoolEvent(Input.GetKeyDown(KeyCode.N), menuCancel);

			LaunchFloatEvent(Input.GetAxis("Mouse ScrollWheel"), menuScroll);
			LaunchFloatEvent(Input.GetAxis("Horizontal"), menuHorizontal);
			LaunchFloatEvent(Input.GetAxis("Vertical"), menuVertical);
			LaunchFloatEvent((Input.GetKeyDown(KeyCode.Tab) ? 1f : 0f), menuSwitchPanel);

			#endregion // MacMenuControls

			// TODO
			#region MacGameControls
			
			LaunchBoolEvent(Input.GetKey(KeyCode.Mouse0), fire1);
			LaunchBoolEvent(Input.GetKey(KeyCode.Mouse1), fire2);
			LaunchBoolEvent(Input.GetKey(KeyCode.Mouse2), fire3);
			LaunchFloatEvent(Input.GetAxis("Horizontal"), moveHorizontal);
			LaunchFloatEvent(Input.GetAxis("Vertical"), moveHorizontal);
			LaunchFloatEvent(Input.GetAxis("HorizontalPivotWindows"), pivotHorizontal);
			LaunchFloatEvent(Input.GetAxis("VerticalPivotWindows"), pivotVertical);
			LaunchFloatEvent(Input.GetAxis("BumpersWindows"), scroll);

			#endregion // MacGameControls
		}

		void CheckController() {
			switch(Application.platform) {
				case RuntimePlatform.WindowsPlayer:
					CheckWindowsControls();
					Debug.LogWarning("CheckController selected windows formula!");
					break;
				// case RuntimePlatform.LinuxPlayer:
				// 	CheckLinuxControls();
				// 	break;
				// case RuntimePlatform.OSXPlayer:
				// 	CheckMacControls();
				// 	break;
				default:
					// If nothing else works, just got to try!
					CheckWindowsControls();
					Debug.LogWarning("CheckController selected default setting!");
					break;
			}
		}

		#endregion // Controller

		void Awake() {
			switch(GS.Data.Settings.Instance.inputMode) {
				case GS.Data.Values.InputMode.MouseAndKeyboard:
					CheckCursorState();
					break;
				case GS.Data.Values.InputMode.Controller:
					//Cursor.lockState = CursorLockMode.Confined;
					Cursor.visible = false;
					break;
				default:
					Debug.LogError("No input mode selected for the game!");
					break;
			}
		}

		void Update()
        {
            CheckMouseAndKeyboard();
            /*switch (PlayerData.Settings.inputMode) {
				case PlayerData.InputMode.MouseAndKeyboard:
					CheckMouseAndKeyboard();
					break;
				case PlayerData.InputMode.Controller:
					CheckController();
					break;
				default:
					Debug.LogError("No input mode selected for the game!");
					break;
			}*/
		}

	}

}

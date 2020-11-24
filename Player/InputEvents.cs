namespace GS.Player
{
    public static class InputEvents
    {
        #region Interface events

        public static bool inMenu;
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

        #region Event triggers

        public static void TriggerBoolEvent(bool test, System.Action action)
        {
            if ( test && action != null ) { action.Invoke(); }
        }
        public static void TriggerFloatEvent(float value, System.Action<float> action)
        {
            if ( value != 0f && action != null ) { action.Invoke(value); }
        }

        #endregion // Event triggers

    }
}
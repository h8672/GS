using UnityEngine;

namespace GS.Basics
{
    public abstract class MenuMovement : MonoBehaviour
    {
        public abstract void Apply();
        public abstract void Cancel();

        public abstract void MoveUp();
        public abstract void MoveDown();
        public abstract void MoveLeft();
        public abstract void MoveRight();

        public abstract void TabLeft();
        public abstract void TabRight();

        public abstract void ScrollUp();
        public abstract void ScrollDown();
    }
}
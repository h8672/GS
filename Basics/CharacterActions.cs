using UnityEngine;

namespace GS.Basics
{
    public abstract class CharacterActions : MonoBehaviour
    {
        public abstract void MenuToggle();

        public abstract void Action1();
        public abstract void Action2();
        public abstract void Action3();

        public abstract void Use();
        public abstract void Pick();
        public abstract void Mount();
    }
}
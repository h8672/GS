using UnityEngine;

namespace GS.Executes
{
    /// <summary>
    /// Executable. Base class for an effect.
    /// </summary>
    public abstract class Executable : MonoBehaviour, IExecute
    {
        public abstract void Execute();
    }
}
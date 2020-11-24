using System.Collections.Generic;
using UnityEngine;

namespace GS.Executes
{
    /// <summary>
    /// Executor. Base class for triggering effects.
    /// </summary>
    public abstract class Executor : MonoBehaviour
    {
        // Example value array
    	[SerializeField] protected Executable[] executables;

        protected abstract void Execute();
        /*/ Example Execute
        protected override void Execute()
        {
            foreach(Executable exe in executables)
            {
                exe.Execute();
            }
        }
        // */

#if UNITY_EDITOR
        public bool ContainsExecutable(Executable executable)
        {
            List<Executable> _executables = new List<Executable>();
            _executables.AddRange(executables);
            return _executables.Contains(executable);
        }
#endif // UNITY_EDITOR
    }
}
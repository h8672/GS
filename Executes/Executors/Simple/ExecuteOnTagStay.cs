using UnityEngine;

namespace GS.Executes.Executors
{
    /// <summary>
    /// Executor. Base class for triggering effects for testing purposes.
    /// </summary>
    [AddComponentMenu("GS/Executor/ExecuteOnTagStay")]
    [RequireComponent(typeof(BoxCollider))]
    public class ExecuteOnTagStay : Executor
    {
        // Example value array
        [SerializeField] private string tagToFollow = "Player";

        [Tooltip("Triggered by trigger collider?")]
        [SerializeField] private bool isTriggerCollider = false;

        [Tooltip("Execute every frame update?")]
        [SerializeField] private bool onceInUpdate = true;

        [Tooltip("Execute after time is passed?")]
        [SerializeField] private bool timedUpdate = false;
        [SerializeField] internal float delayTime = 5f;
        internal bool tagIn = false;
        internal float updated = float.MinValue;

        //[SerializeField] internal Executable[] executables;

        // Example Execute
        protected override void Execute()
        {
            // When no ignore, true, when ignoring, 0 or 1.
            foreach (Executable exe in executables) {
                exe.Execute();
            }
        }

        private void TestCollision()
        {
            if (onceInUpdate && !tagIn ) {
                Execute();
                tagIn = true;
            }
            if(!onceInUpdate) {
                Execute();
            }
            if(timedUpdate)
            {
                if(Time.time > (updated + delayTime))
                {
                    updated = Time.time;
                    Execute();
                }
            }
        }

        /// <summary>
        /// Late update to reset tag after updates.
        /// </summary>
        void LateUpdate()
        {
            if (onceInUpdate && tagIn) { tagIn = false; }
        }

        /// <summary>
        /// Collisions the stay.
        /// Earlier procced once per update, but now it updates for each collider in update.
        /// </summary>
        /// <param name="col">Col.</param>
        void CollisionStay(Collider col)
        {
            if ( !isTriggerCollider && col.tag.Equals(tagToFollow) ) { TestCollision(); }
        }

        /// <summary>
        /// Triggers the stay.
        /// Earlier procced once per update, but now it updates for each collider in update.
        /// </summary>
        /// <param name="col">Col.</param>
        void TriggerStay(Collider col)
        {
            if ( isTriggerCollider && col.tag.Equals(tagToFollow) ) { TestCollision(); }
        }
    }
}
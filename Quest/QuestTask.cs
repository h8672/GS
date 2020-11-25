namespace GS.Quest
{
    /// <summary>
    /// Quest task. You have to design your own version of it.
    /// </summary>
    [System.Serializable]
    public abstract class QuestTask
    {
        public abstract void ActivateTask();
        public abstract void DeactivateTask();

        public abstract bool IsTaskCompleted();
        public abstract string GetTaskData();
        public abstract string[] GetTaskEvents();
    }
}
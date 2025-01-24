using System.Collections.Generic;
using UTasks.Singleton;

namespace UTasks
{
    public class UTaskModule : SingletonModule<UTaskModule>
    {
        private const int CAPACITY = 128;

        internal List<UTask> tasks = new(CAPACITY);
        internal Queue<UTask> taskPool = new(CAPACITY);

        private void Awake()
        {
            if (IsInstanceSetupBefore()) return;
            for (var i = 0; i < CAPACITY; i++)
            {
                var t = new UTask(i);
                taskPool.Enqueue(t);
            }
        }

        private void Update()
        {
            for (var i = tasks.Count - 1; i >= 0; i--)
            {
                if (tasks[i] == null || tasks[i].updater == null || tasks[i].isFinished)
                {
                    tasks.RemoveAt(i);
                    continue;
                }
                else if (tasks[i].isPaused)
                {
                    continue;
                }


                tasks[i].updater.Update(tasks[i]);
            }
        }
    }
}
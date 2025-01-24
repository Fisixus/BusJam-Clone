using System;

namespace UTasks
{
    public class UTask
    {
        private static UTaskModule TInstance => UTaskModule.Instance;

        internal int ID { get; private set; }
        internal ITaskUpdater updater { get; private set; }
        internal bool isFinished { get; private set; }

        internal bool isPaused { get; private set; }
        internal Action onDo { get; private set; }

        internal UTask(int newID)
        {
            ID = newID;
        }

        internal void Reset()
        {
            ID = -1;
            updater = null;
            isFinished = false;
            isPaused = false;
            onDo = null;
        }

        public static UTask While(Func<bool> condition)
        {
            var newT = TInstance.taskPool.Dequeue();
            newT.updater = new WhileUpdater(condition);
            OnStart(newT);
            return newT;
        }

        public static UTask For(float duringTime)
        {
            var newT = TInstance.taskPool.Dequeue();
            newT.updater = new ForUpdater(duringTime);
            OnStart(newT);
            return newT;
        }

        public static UTask Repeat(int repeatCount, float repeatGap)
        {
            var newT = TInstance.taskPool.Dequeue();
            newT.updater = new RepeatUpdater(repeatCount, repeatGap);
            OnStart(newT);
            return newT;
        }

        public static UTask If(Func<bool> condition)
        {
            var newT = TInstance.taskPool.Dequeue();
            newT.updater = new IfUpdater(condition);
            OnStart(newT);
            return newT;
        }


        public static UTask Wait(float waitingTime)
        {
            var newT = TInstance.taskPool.Dequeue();
            newT.updater = new WaitingTimeUpdater(waitingTime);
            OnStart(newT);
            return newT;
        }

        public static UTask WaitFrames(int waitingFrame)
        {
            var newT = TInstance.taskPool.Dequeue();
            newT.updater = new WaitingFrameUpdater(waitingFrame);
            OnStart(newT);
            return newT;
        }


        public UTask Do(Action a)
        {
            onDo = a;
            return this;
        }

        public UTask Pause()
        {
            isPaused = true;
            return this;
        }

        public UTask Play()
        {
            isPaused = false;
            return this;
        }

        public void Kill()
        {
            Reset();
        }

        internal static void OnStart(UTask uT)
        {
            TInstance.tasks.Add(uT);
        }

        internal void OnComplete()
        {
            isFinished = true;
            Reset();
            TInstance.taskPool.Enqueue(this);
        }

        internal void OnDo()
        {
            onDo?.Invoke();
        }
    }
}
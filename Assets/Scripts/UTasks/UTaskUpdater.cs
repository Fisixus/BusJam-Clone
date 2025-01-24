using System;
using UnityEngine;

namespace UTasks
{
    public interface ITaskUpdater
    {
        public void Update(UTask uT);
    }

    public class WaitingTimeUpdater : ITaskUpdater
    {
        private float waitingTime;

        public WaitingTimeUpdater()
        {
        }

        public WaitingTimeUpdater(float waitingTime)
        {
            this.waitingTime = waitingTime;
        }

        public void Update(UTask uT)
        {
            waitingTime -= Time.deltaTime;
            if (waitingTime <= 0)
            {
                uT.OnDo();
                uT.OnComplete();
            }
        }
    }

    public class WaitingFrameUpdater : ITaskUpdater
    {
        private int waitingFrame;

        public WaitingFrameUpdater()
        {
        }

        public WaitingFrameUpdater(int waitingFrame)
        {
            this.waitingFrame = waitingFrame;
        }

        public void Update(UTask uT)
        {
            waitingFrame--;
            if (waitingFrame <= 0)
            {
                uT.OnDo();
                uT.OnComplete();
            }
        }
    }

    public class IfUpdater : ITaskUpdater
    {
        private Func<bool> condition;

        public IfUpdater()
        {
        }

        public IfUpdater(Func<bool> condition)
        {
            this.condition = condition;
        }

        public void Update(UTask uT)
        {
            if (condition.Invoke())
            {
                uT.OnDo();
                uT.OnComplete();
            }
        }
    }

    public class ForUpdater : ITaskUpdater
    {
        private float duringTime;

        public ForUpdater()
        {
        }

        public ForUpdater(float duringTime)
        {
            this.duringTime = duringTime;
        }

        public void Update(UTask uT)
        {
            duringTime -= Time.deltaTime;
            uT.OnDo();
            if (duringTime <= 0) uT.OnComplete();
        }
    }

    public class WhileUpdater : ITaskUpdater
    {
        private Func<bool> condition;

        public WhileUpdater()
        {
        }

        public WhileUpdater(Func<bool> condition)
        {
            this.condition = condition;
        }

        public void Update(UTask uT)
        {
            if (condition.Invoke()) uT.OnDo();
        }
    }

    public class RepeatUpdater : ITaskUpdater
    {
        private int repeatCount;
        private float repeatGap;

        private float repeatDuration;

        private bool isInfinite;

        public RepeatUpdater()
        {
        }

        public RepeatUpdater(int repeatCount, float repeatGap)
        {
            this.repeatCount = repeatCount;
            this.repeatGap = repeatGap;

            repeatDuration = 0f;

            if (repeatCount == -1) isInfinite = true;
        }

        private void RepeatCount(UTask uT)
        {
            repeatDuration -= Time.deltaTime;
            if (repeatDuration <= 0)
            {
                uT.OnDo();
                repeatCount--;
                repeatDuration = repeatGap;
            }
        }

        public void Update(UTask uT)
        {
            if (isInfinite)
            {
                RepeatCount(uT);
                return;
            }

            if (repeatCount < 0)
            {
                RepeatCount(uT);
            }

            else
            {
                if (repeatCount == 0)
                {
                    uT.OnComplete();
                    return;
                }

                RepeatCount(uT);
            }
        }
    }
}
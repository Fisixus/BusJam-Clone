using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.LevelSerialization
{
    public class LevelInfo
    {
        public int LevelNumber { get; private set; }
        public ColorType[,] Dummies { get; private set; }
        public ColorType[] BusOrder { get; private set; }
        public Vector2Int GridSize { get; private set; }
        public int Timer { get; private set; }

        public LevelInfo(int levelNumber, ColorType[,] dummies, ColorType[] busOrder, int timer)
        {
            LevelNumber = levelNumber;
            Dummies = dummies;
            BusOrder = busOrder;
            GridSize = new Vector2Int(Dummies.GetLength(0), Dummies.GetLength(1));
            Timer = timer;
        }

        public override string ToString()
        {
            return
                $"LevelNumber:{LevelNumber}, GridSize:{GridSize}, Timer:{Timer}";
        }
    }
}
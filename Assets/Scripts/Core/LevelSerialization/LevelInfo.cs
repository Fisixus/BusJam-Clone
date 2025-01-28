using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.LevelSerialization
{
    public class LevelInfo
    {
        public int LevelNumber { get; private set; }
        public ColorType[,] Dummies { get; private set; }
        public ColorType[] Buses { get; private set; }
        public Vector2Int DummyGridSize { get; private set; }
        public int BusCount { get; private set; }
        public int Timer { get; private set; }

        public LevelInfo(int levelNumber, ColorType[,] dummies, ColorType[] buses, int timer)
        {
            LevelNumber = levelNumber;
            Dummies = dummies;
            Buses = buses;
            DummyGridSize = new Vector2Int(Dummies.GetLength(0), Dummies.GetLength(1));
            BusCount = Buses.Length;
            Timer = timer;
        }

        public override string ToString()
        {
            return
                $"LevelNumber:{LevelNumber}, GridSize:{DummyGridSize}, BusCount:{BusCount}, Timer:{Timer}";
        }
    }
}
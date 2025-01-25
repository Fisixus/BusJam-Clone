using System.Collections.Generic;
using Core.Factories.Pools;
using UnityEngine;

namespace Core.Factories.Interface
{
    public interface IFactory<T>
    {
        public T CreateObj();

        public void DestroyObjs(List<T> emptyItems);

        public void DestroyObj(T emptyItem);

        public T ObjPrefab { get; }

        public Transform ParentTr { get; }

        public IPool<T> Pool { get; }
    }
}
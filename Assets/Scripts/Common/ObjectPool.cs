using System;
using System.Collections.Generic;
using UnityEngine;

namespace SaberCombatMeta.Common
{
    public class ObjectPool<T> where T : class
    {
        private readonly Stack<T> _availableObjects;
        private readonly HashSet<T> _allObjects;
        private readonly Func<ObjectPool<T>, T> _createFunc;
        private readonly Action<T> _resetObject;
        private readonly Action<T> _destroyObject;

        public int AvailableCount => _availableObjects.Count;
        public int TotalCount => _allObjects.Count;

        public ObjectPool(
            Func<ObjectPool<T>, T> createObject,
            Action<T> resetObject = null,
            Action<T> destroyObject = null,
            int initialSize = 10)
        {
            _createFunc = createObject ?? throw new ArgumentNullException(nameof(createObject));
            _resetObject = resetObject;
            _destroyObject = destroyObject;

            _availableObjects = new Stack<T>(initialSize);
            _allObjects = new HashSet<T>();

            for (var i = 0; i < initialSize; i++)
            {
                CreateNewObject();
            }
        }

        public T Get()
        {
            var obj = _availableObjects.Count > 0 ? _availableObjects.Pop() : CreateNewObject();

            if (obj is MonoBehaviour mb)
            {
                mb.gameObject.SetActive(true);
            }

            return obj;
        }

        public void Return(T obj)
        {
            if (obj == null)
            {
                Debug.LogWarning("Tried to return a null object to the pool");
                return;
            }

            if (!_allObjects.Contains(obj))
            {
                Debug.LogWarning($"Tried to return an object that doesn't belong to this pool");
                _destroyObject?.Invoke(obj);
                return;
            }

            _resetObject?.Invoke(obj);

            if (obj is MonoBehaviour mb)
            {
                mb.gameObject.SetActive(false);
            }

            _availableObjects.Push(obj);
        }

        public void Clear()
        {
            foreach (var obj in _allObjects)
            {
                _destroyObject?.Invoke(obj);
            }

            _availableObjects.Clear();
            _allObjects.Clear();
        }

        private T CreateNewObject()
        {
            var newObj = _createFunc(this);
            if (newObj is MonoBehaviour mb)
            {
                mb.gameObject.SetActive(false);
            }

            _allObjects.Add(newObj);
            _availableObjects.Push(newObj);

            return newObj;
        }
    }
}
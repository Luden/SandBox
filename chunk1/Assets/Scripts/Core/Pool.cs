using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.Core
{
    public interface IKeyProvider<K> where K : struct
    {
        K GetKey();
    }

    public class Pool<K, T> where K : struct where T : IKeyProvider<K>, new()
    {
        private Dictionary<K, List<T>> _pool = new Dictionary<K, List<T>>();

        public virtual T GetOrCreate(K key)
        {
            var pool = GetOrCreatePool(key);
            if (pool.Count != 0)
            {
                var command = pool.First();
                pool.RemoveAt(0);
                return command;
            }

            return Create(key);
        }

        protected virtual T Create(K key)
        {
            return new T();
        }

        private List<T> GetOrCreatePool(K key)
        {
            List<T> pool;
            if (!_pool.TryGetValue(key, out pool))
            {
                pool = new List<T>();
                _pool[key] = pool;
            }
            return pool;
        }

        public void Release(T command)
        {
            var pool = GetOrCreatePool(command.GetKey());
            pool.Add(command);
        }
    }
}
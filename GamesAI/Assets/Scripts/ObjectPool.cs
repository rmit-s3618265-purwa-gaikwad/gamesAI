using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using GObject = UnityEngine.Object;

namespace GamesAI
{
    public class ObjectPool<T> : IEnumerable<T> where T : MonoBehaviour
    {
        private List<T> pool;
        private T prefab;

        public ObjectPool(List<T> source, T prefab)
        {
            pool = source;
            this.prefab = prefab;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return pool.Where(obj => obj.gameObject.activeInHierarchy).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public T Instantiate()
        {
            T instance = pool.Find(obj => !obj.gameObject.activeInHierarchy);
            if (instance == null)
            {
                instance = GObject.Instantiate(prefab);
                pool.Add(instance);
            }
            instance.gameObject.SetActive(true);
            return instance;
        }

        public void Destroy(T obj)
        {
            obj.gameObject.SetActive(false);
        }
    }
}

using System.Collections.Generic;
using UnityEngine;

namespace MTFrame.MTPool
{
    public abstract class BasePool
    {
        public List<GameObject> UsePool;
        protected List<GameObject> IdlePool;

        public virtual void Init()
        {
            UsePool = new List<GameObject>();
            IdlePool = new List<GameObject>();
        }

        /// <summary>
        /// 放回对象池
        /// </summary>
        public virtual void AddPool(GameObject go)
        {
            IdlePool.Add(go);
            go.SetActive(false);
            UsePool.Remove(go);
        }

        /// <summary>
        /// 从对象池获取
        /// </summary>
        /// <returns></returns>
        public virtual GameObject GetPool()
        {
            if(IdlePool.Count > 0)
            {
                GameObject go = IdlePool[0];
                go.SetActive(true);
                IdlePool.RemoveAt(0);
                UsePool.Add(go);
                return go;
            }
            else
            {
                return null;
            }
        }

        public virtual void Clear()
        {
            UsePool.Clear();
            IdlePool.Clear();
        }

    }
}

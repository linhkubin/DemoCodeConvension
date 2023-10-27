using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class HBPool 
{
    class Pool
    {
        //parent contain all pool member
        Transform m_sRoot = null;
        //object is can collect back to pool
        //list object in pool
        Queue<GameUnit> m_inactive;
        //collect obj active ingame
        List<GameUnit> m_active;
        //The prefab that we are pooling
        GameUnit m_prefab;

        public List<GameUnit> Active => m_active;
        public int Count => m_inactive.Count + m_active.Count;
        public Transform Root => m_sRoot;

        // Constructor
        public Pool(GameUnit prefab, int initialQty, Transform parent)
        {
            m_sRoot = parent;
            this.m_prefab = prefab;
            m_inactive = new Queue<GameUnit>(initialQty);
            m_active = new List<GameUnit>();
        }

        // Spawn an object from our pool with position and rotation
        public GameUnit Spawn(Vector3 pos, Quaternion rot)
        {
            GameUnit obj;

            if (m_inactive.Count <= 0)
            {
                obj = (GameUnit)GameObject.Instantiate(m_prefab, m_sRoot);
            }
            else
            {
                // Grab the last object in the inactive array
                obj = m_inactive.Dequeue();
            }

            m_active.Add(obj);

            obj.TF.SetPositionAndRotation(pos, rot);
            obj.gameObject.SetActive(true);

            return obj;
        }

        // Return an object to the inactive pool.
        public void Despawn(GameUnit obj)
        {
            if (obj != null && obj.gameObject.activeSelf)
            {
                obj.gameObject.SetActive(false);
                m_inactive.Enqueue(obj);
            }

            m_active.Remove(obj);
        }

        //collect all unit comeback to pool
        public void Collect()
        {
            while (m_active.Count > 0)
            {
                Despawn(m_active[0]);
            }
        }
    }

    static Dictionary<PoolType, Pool> poolInstance = new Dictionary<PoolType, Pool>();

    public static void Preload(GameUnit prefab, int amount, Transform parent = null)
    {
        if (prefab != null && !poolInstance.ContainsKey(prefab.poolType))
        {
            poolInstance.Add(prefab.poolType, new Pool(prefab, amount, parent));
        }
    }

    public static T Spawn<T>(PoolType poolType, Vector3 pos, Quaternion rot) where T : GameUnit
    {
         return poolInstance[poolType].Spawn(pos, rot) as T;
    }

    public static void Despawn(GameUnit gameUnit)
    {
        poolInstance[gameUnit.poolType].Despawn(gameUnit);
    }

    public static void CollectAll()
    {
        foreach (var pool in poolInstance)
        {
            pool.Value.Collect();
        }
    }
}

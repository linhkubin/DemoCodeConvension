
using UnityEngine;
using System.Collections.Generic;


public static class ParticlePool
{
    const int DEFAULT_POOL_SIZE = 3;

    private static Transform root;

    public static Transform Root
    {
        get
        {
            if (root == null)
            {
                PoolControler controler = GameObject.FindObjectOfType<PoolControler>();
                root = controler != null ? controler.transform : new GameObject("ParticlePool").transform;
            }

            return root;
        }
    }

    /// <summary>
    /// The Pool class represents the pool for a particular prefab.
    /// </summary>
    class Pool
    {
        Transform m_sRoot = null;

        //list prefab ready
        List<ParticleSystem> inactive;

        // The prefab that we are pooling
        ParticleSystem prefab;

        int index;

        // Constructor
        public Pool(ParticleSystem prefab, int initialQty, Transform parent)
        {
#if UNITY_EDITOR
            if (prefab.main.loop)
            {
                var main = prefab.main;
                main.loop = false;

                //save prefab
                UnityEditor.Undo.RegisterCompleteObjectUndo(prefab, "Fix To Not Loop");
                Debug.Log(prefab.name + " ~ Fix To Not Loop");
            }

            if (prefab.main.playOnAwake)
            {
                var main = prefab.main;
                main.playOnAwake = false;

                //save prefab
                UnityEditor.Undo.RegisterCompleteObjectUndo(prefab, "Fix To Not PlayAwake");
                Debug.Log(prefab.name + " ~ Fix To Not PlayAwake");
            }

            if (prefab.main.stopAction != ParticleSystemStopAction.None)
            {
                var main = prefab.main;
                main.stopAction = ParticleSystemStopAction.None;

                //save prefab
                UnityEditor.Undo.RegisterCompleteObjectUndo(prefab, "Fix To Stop Action None");
                Debug.Log(prefab.name + " ~ Fix To  Stop Action None");
            }   
            
            if (prefab.main.duration > 1)
            {
                var main = prefab.main;
                main.duration = 1;

                //save prefab
                UnityEditor.Undo.RegisterCompleteObjectUndo(prefab, "Fix To Duration By 1");
                Debug.Log(prefab.name + " ~ Fix To Duration By 1");
            }
#endif

            m_sRoot = parent;
            this.prefab = prefab;
            inactive = new List<ParticleSystem>(initialQty);

            for (int i = 0; i < initialQty; i++)
            {
                ParticleSystem particle = (ParticleSystem)GameObject.Instantiate(prefab, m_sRoot);
                particle.Stop();
                inactive.Add(particle);
            }
        }

        public int Count {
            get { return inactive.Count;}
        }

        // Spawn an object from our pool
        public void Play(Vector3 pos, Quaternion rot)
        {
            index = index + 1 < inactive.Count ? index + 1 : 0;

            ParticleSystem obj = inactive[index];

            if (obj.isPlaying)
            {
                obj = (ParticleSystem)GameObject.Instantiate(prefab, m_sRoot);
                obj.Stop();
                inactive.Insert(index, obj);
            }

            obj.transform.SetPositionAndRotation( pos, rot);
            obj.Play();
        }

        public void Release() {
            while(inactive.Count > 0) {
                GameObject.DestroyImmediate(inactive[0]);
                inactive.RemoveAt(0);
            }
            inactive.Clear();
        }
    }

    //--------------------------------------------------------------------------------------------------

    static Dictionary<ParticleType, ParticleSystem> shortcuts = new Dictionary<ParticleType, ParticleSystem>();
    // All of our pools
    static Dictionary<int, Pool> pools = new Dictionary<int, Pool>();

    /// <summary>
    /// Init our dictionary.
    /// </summary>
    static void Init(ParticleSystem prefab = null, int qty = DEFAULT_POOL_SIZE, Transform parent = null)
    {
        if (prefab != null && !pools.ContainsKey(prefab.GetInstanceID()))
        {
            pools[prefab.GetInstanceID()] = new Pool(prefab, qty, parent);
        }
    }

    static public void Preload(ParticleSystem prefab, int qty = 1, Transform parent = null)
    {
        Init(prefab, qty, parent);
    }

    static public void Play(ParticleSystem prefab, Vector3 pos, Quaternion rot)
    {
#if UNITY_EDITOR
        if (prefab == null)
        {
            Debug.LogError(prefab.name + " is null!");
            return;
        }
#endif

        if (!pools.ContainsKey(prefab.GetInstanceID()))
        {
            Transform newRoot = new GameObject("VFX_" + prefab.name).transform;
            newRoot.SetParent(Root);
            pools[prefab.GetInstanceID()] = new Pool(prefab, 10, newRoot);
        }

        pools[prefab.GetInstanceID()].Play(pos, rot);
    }

    static public void Play(ParticleType particleType, Vector3 pos, Quaternion rot)
    {
#if UNITY_EDITOR
        if (!shortcuts.ContainsKey(particleType))
        {
            Debug.LogError(particleType + " is nees install at pool container!!!");
        }
#endif

        Play(shortcuts[particleType], pos, rot);
    }

    static public void Release(ParticleSystem prefab)
    {
        if (pools.ContainsKey(prefab.GetInstanceID()))
        {
            pools[prefab.GetInstanceID()].Release();
            pools.Remove(prefab.GetInstanceID());
        }
        else
        {
            GameObject.DestroyImmediate(prefab);
        }
    }

    static public void Shortcut(ParticleType particleType, ParticleSystem particleSystem)
    {
        shortcuts.Add(particleType, particleSystem);
    }
}


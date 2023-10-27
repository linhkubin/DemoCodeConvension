using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUnit : MonoBehaviour
{
    private Transform tf;
    public Transform TF
    {
        get
        {
            //tf = tf ?? gameObject.transform;
            if (tf == null)
            {
                tf = transform;
            }
            return tf;
        }
    }

    public PoolType poolType;

    public void OnDespawn(float delay)
    {
        Invoke(nameof(OnDespawn), delay);
    }

    private void OnDespawn()
    {
        HBPool.Despawn(this);
    }

}
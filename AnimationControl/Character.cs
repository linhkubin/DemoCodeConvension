using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CompareTagName
{
    public class Character : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(Constants.TAG_BRICK))
            {
                //DO SOMETHINGS...
            }
        }
    }
}

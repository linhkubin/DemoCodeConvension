using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

namespace NavMesh
{
    public class Character : MonoBehaviour
    {
        //cache transform de toi uu hieu nang
        [SerializeField] protected Transform tf;
        //keo navmesh agent vao
        [SerializeField] private NavMeshAgent agent;
        //luu diem muc tieu se di den
        private Vector3 destination;

        //property tra ve ket qua xem la da toi diem muc tieu hay chua
        public bool IsDestionation => Vector3.Distance(tf.position, destination + (tf.position.y - destination.y) * Vector3.up) < 0.1f;

        //set diem den
        public void SetDestination(Vector3 destination)
        {
            this.destination = destination;
            agent.SetDestination(destination);
        }
    }

}

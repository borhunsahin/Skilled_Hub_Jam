using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class NawMeshAI : MonoBehaviour
{
    public Transform playerTrs;
    private NavMeshAgent agent;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }
    private void Update()
    {
        if (agent != null)
        {
            agent.destination = playerTrs.position;
        }
    }
}

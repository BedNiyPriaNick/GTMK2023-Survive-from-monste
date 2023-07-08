using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAI : MonoBehaviour
{
    [SerializeField] private UnityEngine.AI.NavMeshAgent ai;
    [SerializeField] private List<Transform> destinations;
    [SerializeField] private Animator aiAnim;
    [SerializeField] private float walkSpeed, chaseSpeed, idleTime, destinationAmount;
    [SerializeField] private bool walking, chasing;
    [SerializeField] private Transform player;

    private Transform currentDest;
    private Vector3 dest;
    private int randNum;

}

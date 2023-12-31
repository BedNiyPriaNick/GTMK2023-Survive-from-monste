using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
public class MonsterAI : MonoBehaviour
{
    private NavMeshAgent ai;
    [SerializeField] private List<Transform> destinations;
    private Animator aiAnim;
    [SerializeField] private float walkSpeed, chaseSpeed, minIdleTime, maxIdleTime, sightDistance, catchDistance, minChaseTime, maxChaseTime, jumpscareTime;
    private float idleTime, chaseTime;
    private bool walking, chasing;
    [SerializeField] private Transform player;
    private Transform currentDest;
    private Vector3 dest;
    private int randNum;
    [SerializeField] private int destinationAmount;
    private Vector3 rayCastOffset;
    [SerializeField] private PlayerMove playerScript;

    void Start()
    {
        aiAnim = GetComponent<Animator>();
        ai = GetComponent<NavMeshAgent>();

        ai.updateRotation = false;
        ai.updateUpAxis = false;

        walking = false;
        chasing = true;
        randNum = Random.Range(0, destinationAmount);
        currentDest = destinations[randNum];
    }
    void Update()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        RaycastHit hit;

        if (Physics.Raycast(transform.position + rayCastOffset, direction, out hit, sightDistance))
        {
            if (hit.collider.gameObject.tag == "Player")
            {
                walking = false;
                StopCoroutine("stayIdle");
                StopCoroutine("chaseRoutine");
                StartCoroutine("chaseRoutine");
                chasing = true;
            }
        }

        if (playerScript.Hiding)
        {
            StopCoroutine("chaseRoutine");
            chasing = false;
            walking = true;
        }

        if (chasing == true)
        {
            dest = player.position;
            ai.destination = dest;
            ai.speed = chaseSpeed;
            aiAnim.ResetTrigger("walk");
            aiAnim.ResetTrigger("idle");
            aiAnim.SetTrigger("sprint");
        }
        if (walking == true)
        {
            dest = currentDest.position;
            ai.destination = dest;
            ai.speed = walkSpeed;
            aiAnim.ResetTrigger("sprint");
            aiAnim.ResetTrigger("idle");
            aiAnim.SetTrigger("walk");
            if (ai.remainingDistance <= ai.stoppingDistance)
            {
                aiAnim.ResetTrigger("sprint");
                aiAnim.ResetTrigger("walk");
                aiAnim.SetTrigger("idle");
                ai.speed = 0;
                StopCoroutine("stayIdle");
                StartCoroutine("stayIdle");
                walking = false;
            }
        }
    }

    IEnumerator stayIdle()
    {
        idleTime = Random.Range(minIdleTime, maxIdleTime);
        yield return new WaitForSeconds(idleTime);
        walking = true;
        randNum = Random.Range(0, destinationAmount);
        currentDest = destinations[randNum];
    }
    IEnumerator chaseRoutine()
    {
        chaseTime = Random.Range(minChaseTime, maxChaseTime);
        yield return new WaitForSeconds(chaseTime);
        walking = true;
        chasing = false;
        randNum = Random.Range(0, destinationAmount);
        currentDest = destinations[randNum];
    }
    IEnumerator deathRoutine()
    {
        yield return new WaitForSeconds(jumpscareTime);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            aiAnim.ResetTrigger("walk");
            aiAnim.ResetTrigger("idle");
            aiAnim.ResetTrigger("sprint");
            StartCoroutine(deathRoutine());
            chasing = false;
        }
    }
}

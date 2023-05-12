using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform[] patrolPoints;
    public Transform transformDest;
    private NavMeshAgent agent;
    private Animator animator;

    [Header("Variables")]
    [SerializeField] private float patrolRadius = 1f;
    private int currentPatrolPointIndex;
    [SerializeField] private bool isPatrolling;

    [SerializeField] public enum EnemyState
    {
        Idle,
        Patrol
    }

    private EnemyState currentState;


    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        // Set initial state to idle
        currentState = EnemyState.Idle;
        agent.destination = transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        UpdatePatrolState();
        //if (isPatrolling)
        //{
        //    agent.destination = GetRandomPos();
        //}

    }

    private void UpdatePatrolState()
    {
        switch (currentState)
        {
            case EnemyState.Idle:
                agent.destination = transform.position; 
                animator.SetBool("IsWalking", false); 
                break;

            case EnemyState.Patrol:
                if (isPatrolling)
                {

                    Debug.Log("Should start corountine");
                    //agent.destination = transformDest.position;
                    StartCoroutine(Patrol());
                }
                animator.SetBool("IsWalking", true); 
                break;
        }
    }
   
    void StartPatrolling()
    {
        isPatrolling = true;
        currentState = EnemyState.Patrol;
    }

   
    void StopPatrolling()
    {
        isPatrolling = false;
        currentState = EnemyState.Idle;
    }



     IEnumerator Patrol()
    {
        currentPatrolPointIndex = (currentPatrolPointIndex + 1) % patrolPoints.Length;
        Vector3 target = GetRandomPos(); //GetRandomPointInRadius(patrolPoints[currentPatrolPointIndex].position, patrolRadius);
        agent.SetDestination(target);
        Debug.Log(agent.SetDestination(target));
        while (agent.remainingDistance > agent.stoppingDistance)
        {
            yield return null;
        }
        yield return null;
    }

    IEnumerator Patrolll()
    {
        // Move to next patrol point
        currentPatrolPointIndex = (currentPatrolPointIndex + 1) % patrolPoints.Length;
        Vector3 target = GetRandomPointInRadius(patrolPoints[currentPatrolPointIndex].position, patrolRadius);
        agent.SetDestination(target);

        // Wait until destination is reached
        while (agent.remainingDistance > agent.stoppingDistance)
        {
            yield return null;
        }

        yield return null;
    }

    private void GoToDestination()
    {
        agent.destination = transformDest.position;
    }
    
    Vector3 GetRandomPointInRadius(Vector3 center, float radius)
    {
        Vector3 randomPos = Random.insideUnitSphere * radius;
        randomPos += center;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomPos, out hit, radius, NavMesh.AllAreas);
        return hit.position;
    }

    private Vector3 GetRandomPos()
    {
        int randIndex = Random.Range(0, patrolPoints.Length -1);
        return (patrolPoints[randIndex].position);
    }

    #region -Button Onclick-
    public void StartPatrol()
    {
        StartPatrolling();
    }

    public void StopPatrol()
    {
        StopPatrolling();
    }
    #endregion
}

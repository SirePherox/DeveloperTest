using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform[] patrolPoints;
    private NavMeshAgent agent;
    private Animator animator;

    [Header("Variables")]
    [SerializeField] private float patrolRadius = 5f;
    private int currentPatrolPointIndex;
    private bool isPatrolling;

    private enum EnemyState
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

        // Start patrolling
        StartPatrolling();
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePatrolState();
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
                if (!isPatrolling)
                {
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
        Vector3 target = GetRandomPointInRadius(patrolPoints[currentPatrolPointIndex].position, patrolRadius);
        agent.SetDestination(target);

        while (agent.remainingDistance > agent.stoppingDistance)
        {
            yield return null;
        }

        if (PlayerIsDetected())
        {
            StopPatrolling();
        }

        yield return null;
    }

    
    Vector3 GetRandomPointInRadius(Vector3 center, float radius)
    {
        Vector3 randomPos = Random.insideUnitSphere * radius;
        randomPos += center;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomPos, out hit, radius, NavMesh.AllAreas);
        return hit.position;
    }

  
    bool PlayerIsDetected()
    {
        return false;
    }
}

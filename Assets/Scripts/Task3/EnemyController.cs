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
    [SerializeField] private List<Vector3> patrolPos ;
    [SerializeField] private List<Vector3> availablePatrolPos = new List<Vector3>();
    
    private bool isMovingToPatrolPoint;
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

        GetAllPatrolPositions();
    }

    // Update is called once per frame
    void Update()
    {
        //call functions
        UpdatePatrolState();

        if (!isMovingToPatrolPoint && currentState == EnemyState.Patrol)
        {
            StartCoroutine(Patrol());
        }
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
        isMovingToPatrolPoint = true; // set flag to true
        currentPatrolPointIndex = (currentPatrolPointIndex + 1) % patrolPoints.Length;
        Vector3 target = GetRandomPos(); 
        agent.SetDestination(target);
        Debug.Log(agent.SetDestination(target));
        while (agent.remainingDistance > agent.stoppingDistance)
        {
            isMovingToPatrolPoint = true; // set flag to false
            yield return null;
        }
        
        if(agent.remainingDistance <= agent.stoppingDistance)
        {
            StopPatrolling();
            isMovingToPatrolPoint = false;
        }

        yield return null;
    }


    private Vector3 GetRandomPos()
    {
        if(availablePatrolPos.Count == 0)
        {
            availablePatrolPos = new List<Vector3>(patrolPos);
        }
        Vector3 randPos;
        int randIndex = Random.Range(0, availablePatrolPos.Count -1);
        randPos = (availablePatrolPos[randIndex]);
        availablePatrolPos.RemoveAt(randIndex);
        return randPos;
    }

    private void GetAllPatrolPositions()
    {
        for (int i = 0; i < patrolPoints.Length; i++)
        {
            patrolPos[i] = patrolPoints[i].position;
        }
        availablePatrolPos = new List<Vector3>(patrolPos);

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

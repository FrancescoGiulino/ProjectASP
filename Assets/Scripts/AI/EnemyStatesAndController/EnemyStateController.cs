using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(EnemyTargetDetectionController))]
[RequireComponent(typeof(NavMeshAgent))]
public class EnemyStateController : MonoBehaviour
{
    [Header("Enemy Resources")]
    [SerializeField] private EnemyResources resources;
    public EnemyResources Resources => resources;

    [Header("Target")]
    [SerializeField] private Transform target;
    //[SerializeField] private float runningSpeed = 3, walkingSpeed = 1;
    [SerializeField] private EnemyTargetDetectionController targetDetectionController;

    [Header("Patrol Settings")]
    [SerializeField] private Vector3[] patrolPoints;
    private int currentPatrolIndex = 0;

    [Header("Look Settings")]
    [SerializeField] private float lookRotationTime = 1.5f;
    [SerializeField] private float rotationSpeed = 120f;
    [SerializeField] private float maxLookTime = 4f;

    [Header("Shooter")]
    [SerializeField] private Shooter shooter;

    [Header("EnemyFXController")]
    [SerializeField] private EnemyFXController enemyFXController;

    private NavMeshAgent navMeshAgent;
    private IEnemyState currentState;

    public bool IsWalking { get; private set; }
    public bool IsRunning { get; private set; }
    public Shooter Shooter { get { return shooter; } }

    // Variabili accessibili dagli stati
    public Transform Target => target;
    public NavMeshAgent Agent => navMeshAgent;
    public EnemyTargetDetectionController Detection => targetDetectionController;
    public float LookRotationTime => lookRotationTime;
    public float RotationSpeed => rotationSpeed;
    public float MaxLookTime => maxLookTime;
    public Vector3 CheckPosition { get; private set; }

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        ChangeState(new PatrolState()); // Stato iniziale
    }

    private void Update()
    {
        currentState?.Update(this);
        UpdateMovementFlags();
    }

    public void ChangeState(IEnemyState newState)
    {
        currentState?.Exit(this);
        currentState = newState;
        currentState?.Enter(this);
        enemyFXController.HandleFX(GetCurrentState());
    }

    // --- HELPERS ---
    public void GoToNextPatrolPoint()
    {
        if (patrolPoints.Length == 0)
        {
            ChangeState(new LookState());
            return;
        }

        navMeshAgent.speed = resources.walkSpeed;
        navMeshAgent.destination = patrolPoints[currentPatrolIndex];
        currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
    }

    public void GoToClosestPatrolPoint()
    {
        if (patrolPoints.Length == 0) return;

        int closestIndex = 0;
        float closestDistance = Mathf.Infinity;

        for (int i = 0; i < patrolPoints.Length; i++)
        {
            float dist = Vector3.Distance(transform.position, patrolPoints[i]);
            if (dist < closestDistance)
            {
                closestDistance = dist;
                closestIndex = i;
            }
        }

        currentPatrolIndex = closestIndex;
        navMeshAgent.speed = resources.walkSpeed;
        navMeshAgent.destination = patrolPoints[currentPatrolIndex];
    }

    public void GoCheckPosition(Vector3 pos)
    {
        CheckPosition = pos;
        ChangeState(new CheckState());
    }

    private void UpdateMovementFlags()
    {
        // Velocità attuale sul piano (ignora eventuali salti)
        float currentSpeed = navMeshAgent.velocity.magnitude;

        // Consideriamo in movimento solo sopra una certa soglia (es. 0.1)
        if (currentSpeed > 0.1f)
        {
            if (Mathf.Approximately(navMeshAgent.speed, resources.walkSpeed))
            {
                IsWalking = true;
                IsRunning = false;
            }
            else if (Mathf.Approximately(navMeshAgent.speed, resources.runSpeed))
            {
                IsWalking = false;
                IsRunning = true;
            }
            else
            {
                // Caso in cui la speed è settata a valori intermedi
                IsWalking = currentSpeed < resources.runSpeed * 0.75f;
                IsRunning = !IsWalking;
            }
        }
        else
        {
            IsWalking = false;
            IsRunning = false;
        }
    }

    // Getters
    public string GetCurrentState() => currentState?.ToString();
}

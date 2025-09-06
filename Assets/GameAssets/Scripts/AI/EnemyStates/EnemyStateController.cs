using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(EnemyTargetDetectionController))]
[RequireComponent(typeof(NavMeshAgent))]
public class EnemyStateController : MonoBehaviour
{
    [Header("Enemy Resources")]
    [SerializeField] private EnemyProfile profile;
    [SerializeField] private EnemyResources resources;
    public EnemyResources Resources => resources;

    [Header("Reasoning Style")]
    [SerializeField] private ReasoningStyles reasoningStyle;
    public ReasoningStyles ReasoningStyle { get { return reasoningStyle; } }
    public enum ReasoningStyles { EcoSentinel, OverrideStalker, CloseRangeEnforcer };
    [HideInInspector] public string ReasoningStyleType="CloseRangeEnforcer";

    [Header("Target")]
    [SerializeField] private Transform target;
    [SerializeField] private EnemyTargetDetectionController targetDetectionController;

    [Header("EnemyStates")]
    [SerializeField] private EnemyStateSwitcher enemyStateSwitcher;

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

    [Header("Health")]
    [SerializeField] private HealthController healthController;
    public HealthController HealthController => healthController;
    public int healthValue;

    [Header("Sound")]
    [SerializeField] public SoundEventComponent SoundComponent { get; private set; }

    [HideInInspector] public int X, Y, Z; // Posizione approssimata

    private NavMeshAgent navMeshAgent;
    private EnemyState currentState;
    public EnemyState CurrentState => currentState;
    public EnemyState PrevState { get; set; }
    [HideInInspector] public string currentStateName = "PatrolState"; // serve a ThinkEngine
    [HideInInspector] public int EnemyId;

    public bool CanHearSounds { get; private set; } = true;
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
    public Vector3 CheckPosition { get; set; }
    public Vector3 BatteryPosition { get; set; }

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();

        if (!SoundComponent) SoundComponent = GetComponent<SoundEventComponent>();
        if (!SoundComponent) Debug.LogError($"[ENEMY {name}] SoundEventComponent not found!");

        if (profile != null)
            resources = profile.GenerateResources();
        else
            Debug.LogError($"No EnemyProfile assigned to {name}!");

        EnemyId = gameObject.GetInstanceID();
        ReasoningStyleType = reasoningStyle.ToString();
    }

    private void Start()
    {
        ChangeState(new PatrolState()); // Stato iniziale
    }

    private void Update()
    {
        enemyStateSwitcher.CalculateCorrectState();
        currentState?.Update(this);
        UpdateMovementFlags();

        X = Mathf.RoundToInt(transform.position.x);
        Y = Mathf.RoundToInt(transform.position.y);
        Z = Mathf.RoundToInt(transform.position.z);

        healthValue=Mathf.RoundToInt(healthController.CurrentHealth);
    }

    public void ChangeState(EnemyState newState)
    {
        // Se il nuovo stato è dello stesso tipo di quello corrente, non fare nulla
        if (currentState != null && currentState.GetType() == newState.GetType())
            return;

        // Salva lo stato precedente
        PrevState = currentState;

        // Cambia stato
        currentState?.Exit(this);
        currentState = newState;
        currentState?.Enter(this);

        enemyFXController.HandleFX(GetCurrentState());
        currentStateName = currentState?.ToString();
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

    public bool HasLowBattery() => HealthController.CurrentHealth <= Resources.minBatteryBeforeRecharge;
    public bool HasNoBattery() => HealthController.CurrentHealth <= 0;
    public void GoToNearestBatteryCharger()
    {
        if (WorldInformationManager.Instance == null)
        {
            Debug.LogError("WorldInformationManager.Instance è NULL: manca il manager in scena.");
            return;
        }

        // Ottieni il punto più vicino sulla NavMesh
        GameObject nearestCharger = WorldInformationManager.Instance.GetNearestBatteryCharger(transform.position);
        Vector3? nearestNavPoint = nearestCharger != null ? (Vector3?)nearestCharger.transform.position : null;
        if (nearestNavPoint == null)
        {
            Debug.LogWarning("Nessun battery charger raggiungibile trovato!");
            return;
        }

        // Imposta la destinazione proiettata sulla NavMesh
        BatteryPosition = nearestNavPoint.Value;
        //Debug.LogError($"[{name}] Destinazione charger impostata a {BatteryPosition}");

        // Siccome la batteria è scarica, tutti i task assegnati falliranno.
        MessageBus.Instance.ChangeTaskStateByEnemyName(name,"Failed");

        // Cambia stato
        ChangeState(new GoToRechargeState());
    }

    // Getters
    public string GetCurrentState() => currentState?.ToString();
}

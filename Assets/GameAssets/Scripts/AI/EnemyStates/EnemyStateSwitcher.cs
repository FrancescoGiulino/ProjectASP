using UnityEngine;

public class EnemyStateSwitcher : MonoBehaviour
{
    [SerializeField] private EnemyStateController enemy;

    private void Awake()
    {
        if (!enemy) enemy = GetComponent<EnemyStateController>();
        //if (!enemy) Debug.LogWarning("EnemyStateController not found.");
        if (!enemy) Debug.LogError("EnemyStateController not found.");
    }

    public void CheckChaseState()
    {
        // controlla se il nemico vede il player --> se sì, passa allo stato di chase.
        if (enemy.Detection.CheckForChaseTrigger() && !enemy.HasLowBattery())
        {
            // controllo extra: target non morto
            if (enemy.Target != null && enemy.Target.TryGetComponent(out HealthController playerHealth) && !playerHealth.IsDead)
            {
                enemy.ChangeState(new ChaseState());
                return;
            }
        }

    }
    public void CheckGoToNearestBatteryChargerState()
    {
        if (enemy.HasLowBattery())
            enemy.GoToNearestBatteryCharger();
    }
    public void CheckLookState()
    {
        if (!enemy.Detection.IsTargetInChaseRange())
            enemy.ChangeState(new LookState());
    }

    public MessageData GetActiveTask()
    {
        foreach (var message in MessageBus.Instance.AiMessages)
        {
            if (message.AssignedTo == enemy.name)
                return message;
        }

        return null;
    }

    public void CalculateCorrectState()
    {
        // se il nemico ha poca batteria, va a ricaricarla
        if (enemy.HasLowBattery())
        {
            enemy.GoToNearestBatteryCharger();
            return;
        }

        // Se il nemico è in ChaseState ma il player non è più in range, passa a LookState
        if (enemy.GetCurrentState() == "ChaseState")
        {
            if (!enemy.Detection.IsTargetInChaseRange())
            {
                enemy.ChangeState(new LookState());
                return; // esci subito, non forzare ChaseState
            }
        }

        MessageData activeTask = GetActiveTask();

        // --- Gestione Task Attivi ---
        if (activeTask != null)
        {
            Vector3 taskPos = new Vector3(activeTask.X, activeTask.Y, activeTask.Z);

            // Se sono in Patrol e ricevo un task --> vado a controllare
            if (enemy.GetCurrentState() == "PatrolState")
            {
                enemy.CheckPosition = taskPos;
                enemy.ChangeState(new CheckState());
            }
            // Se sono già in CheckState --> aggiorno costantemente la posizione
            else if (enemy.GetCurrentState() == "CheckState")
            {
                enemy.CheckPosition = taskPos;
            }
        }
        // --- Nessun task ---
        else if (activeTask == null 
                && enemy.GetCurrentState() != "ChaseState" 
                && enemy.GetCurrentState() != "GoToRechargeState"
                && enemy.GetCurrentState() != "CheckState") // <-- aggiunto filtro
        {
            if (enemy.PrevState != null)
            {
                if (enemy.PrevState.ToString() != "ChaseState" && enemy.PrevState.ToString() != "CheckState")
                    enemy.ChangeState(new PatrolState());
                else
                    enemy.ChangeState(new LookState());
            }
        }

        // ----------------------------------------

        // controlla se il nemico vede il player --> se sì, passa allo stato di chase.
        if (enemy.Detection.CheckForChaseTrigger() && !enemy.HasLowBattery())
        {
            enemy.ChangeState(new ChaseState());
            return;
        }
    }
}

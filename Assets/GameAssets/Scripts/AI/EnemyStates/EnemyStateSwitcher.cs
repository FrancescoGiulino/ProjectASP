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
            //enemy.PrevState = enemy.CurrentState;
            enemy.GoToNearestBatteryCharger();
            return;
        }

        MessageData activeTask = GetActiveTask();

        // controlla se c'è un task attivo (oppure se è in chase state e non ha nient'altro da fare).
        if (activeTask != null || ((enemy.GetCurrentState() == "ChaseState" || enemy.GetCurrentState()=="LookState" || enemy.GetCurrentState()=="CheckState") && GetActiveTask() == null))
        {
            // se c'è un task attivo ed il nemico è in stato di patrol, allora, vai a controllare nella posizione del task.
            if (enemy.GetCurrentState() == "PatrolState")
            {
                enemy.CheckPosition = new Vector3(activeTask.X, activeTask.Y, activeTask.Z);
                enemy.ChangeState(new CheckState());
            }
        }
        // altrimenti, se non c'è un task attivo torna allo stato di patrol.
        else if (activeTask == null && enemy.GetCurrentState() != "ChaseState" && enemy.GetCurrentState() != "GoToRechargeState")
        {
            if (enemy.PrevState != null)
                if (enemy.PrevState.ToString() != "ChaseState" || enemy.PrevState.ToString() != "CheckState")
                    enemy.ChangeState(new PatrolState());
                else
                    enemy.ChangeState(new LookState());
        }
        else if (enemy.PrevState.ToString() == "GoToRechargeState" && enemy.HealthController.CurrentHealth >= 95)
        {
            enemy.ChangeState(new PatrolState());
        }

        // ----------------------------------------

        // controlla se il nemico vede il player --> se sì, passa allo stato di chase.
        if (enemy.Detection.CheckForChaseTrigger() && !enemy.HasLowBattery())
        {
            enemy.ChangeState(new ChaseState());
            return;
        }

        // se lo stato precedente è chase o check, allora il successivo sarà look
        //if (enemy.PrevState.ToString() == "ChaseState" || enemy.PrevState.ToString() == "CheckState")

    }
}

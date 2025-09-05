using System.Collections.Generic;
using UnityEngine;
using ThinkEngine.Planning;

public class SetState : Action
{
    public string EnemyName { get; set; }
    public string NewState { get; set; }
    public List<string> ValidStates = new List<string>();

    private void InitializeValidStatesList()
    {
        if (ValidStates.Count == 0) // inizializza solo una volta
        {
            ValidStates.Add("ChaseState");
            ValidStates.Add("CheckState");
            ValidStates.Add("PatrolState");
            ValidStates.Add("LookState");
            ValidStates.Add("GoToRechargeState");
        }
    }

    private EnemyStateController FindEnemyController(string enemyName)
    {
        if (WorldInformationManager.Instance == null || WorldInformationManager.Instance.LumenSentinels == null)
        {
            Debug.LogError("[SetState] WorldInformationManager non inizializzato o lista nemici mancante.");
            return null;
        }

        foreach (var enemy in WorldInformationManager.Instance.LumenSentinels)
        {
            // se è lista di GameObject
            if (enemy != null && enemy.name == enemyName)
                return enemy.GetComponent<EnemyStateController>();
        }

        return null;
    }

    public override State Prerequisite()
    {
        InitializeValidStatesList();

        // Siccome lato ASP salvo solo il nome senza "State", la aggiungo qui
        if (!NewState.EndsWith("State"))
            NewState += "State";

        // Controllo che lo stato richiesto sia valido
        if (!ValidStates.Contains(NewState))
            return State.ABORT;

        // Controllo che il nemico esista
        var controller = FindEnemyController(EnemyName);
        if (controller == null)
        {
            Debug.LogError($"[SetState] Nessun nemico trovato con nome {EnemyName}");
            return State.ABORT;
        }

        return State.READY;
    }

    public override void Do()
    {
        var controller = FindEnemyController(EnemyName);
        if (controller == null) return; // doppio check di sicurezza

        // Crea lo stato richiesto dinamicamente
        var stateType = System.Type.GetType(NewState);
        if (stateType == null)
        {
            Debug.LogWarning($"[SetState] Stato {NewState} non trovato come tipo valido");
            return;
        }

        EnemyState newStateInstance = (EnemyState)System.Activator.CreateInstance(stateType);
        controller.ChangeState(newStateInstance);

        Debug.Log($"[SetState] {EnemyName} è passato allo stato {NewState}");
    }

    public override State Done()
    {
        return State.READY;
    }
}

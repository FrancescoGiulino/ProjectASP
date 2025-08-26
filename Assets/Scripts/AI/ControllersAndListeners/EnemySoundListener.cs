using UnityEngine;
using System;

public class EnemySoundListener : MonoBehaviour
{
    [Header("Sensibilità del nemico")]
    [SerializeField] private float reactionDelay = 0.2f;

    private EnemyStateController enemy;

    public event Action<Vector3> OnSuspiciousSoundHeard;

    private void Awake()
    {
        enemy = GetComponent<EnemyStateController>();
    }

    public void OnSoundHeard(Vector3 soundPosition)
    {
        //if (!enemy.HasLowBattery())
        //{
            lastHeardSound = soundPosition;
            Invoke(nameof(ReactToSound), reactionDelay);
        //}
    }

    private Vector3 lastHeardSound;

    private void ReactToSound()
    {
        if (enemy == null) return;

        if (enemy.GetCurrentState() != "ChaseState")
        {
            // Notifica chi ascolta questo evento (EnemyMessenger) --> sempre, anche se ha poca batteria!
            if (enemy.GetCurrentState()!="CheckState" && enemy.GetCurrentState()!="ChaseState")
                OnSuspiciousSoundHeard?.Invoke(lastHeardSound);
            
            // Se ha batteria sufficiente -> va a controllare
            if (!enemy.HasLowBattery())
                enemy.GoCheckPosition(lastHeardSound);
        }
    }
}

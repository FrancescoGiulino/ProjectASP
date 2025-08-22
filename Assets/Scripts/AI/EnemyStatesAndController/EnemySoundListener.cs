using UnityEngine;

public class EnemySoundListener : MonoBehaviour
{
    [Header("Sensibilità del nemico")]
    [SerializeField] private float reactionDelay = 0.2f; // serve per aggiungere un piccolo ritardo prima di reagire
    [SerializeField] private bool debugLog = true;

    private EnemyStateController enemy;

    private void Awake()
    {
        enemy = GetComponent<EnemyStateController>();
    }

    public void OnSoundHeard(Vector3 soundPosition)
    {
        if (debugLog)
            Debug.Log($"{gameObject.name} ha sentito un suono a {soundPosition}");

        // avvia la reazione del nemico
        Invoke(nameof(ReactToSound), reactionDelay);

        // salva la posizione del suono per la reazione
        lastHeardSound = soundPosition;
    }

    private Vector3 lastHeardSound;

    private void ReactToSound()
    {
        if (enemy == null) return;
        if (enemy.GetCurrentState()!="ChaseState")
            enemy.GoCheckPosition(lastHeardSound);
    }
}

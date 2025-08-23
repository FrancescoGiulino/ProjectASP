using System.Collections.Generic;
using UnityEngine;

public class EnemyMessenger : MonoBehaviour
{
    [SerializeField] private EnemyTargetDetectionController visualDetection;
    [SerializeField] private EnemySoundListener soundDetection;
    [SerializeField] private EnemyStateController enemy;

    private bool canSendTargetDetectionMsg = true;

    private void Awake()
    {
        if (soundDetection != null)
            soundDetection.OnSuspiciousSoundHeard += SendSuspiciousSoundDetected;
    }

    private void OnDestroy()
    {
        if (soundDetection != null)
            soundDetection.OnSuspiciousSoundHeard -= SendSuspiciousSoundDetected;
    }

    private void Update()
    {
        // Check if player was seen
        if (visualDetection.CheckForTargets() && canSendTargetDetectionMsg)
        {
            canSendTargetDetectionMsg = false;
            SendTargetDetected(visualDetection.GetDetectedTargetPosition());
        }
        if (enemy.GetCurrentState() != "ChaseState") canSendTargetDetectionMsg = true;
    }

    // -----------------------------------------------
    // Messaggi
    // -----------------------------------------------

    public void SendTargetDetected(Vector3 pos)
    {
        string coords = $"coordinates: x:{pos.x:F1}; y:{pos.y:F1}; z:{pos.z:F1}";

        AiMessageEmitter.Instance.EmitMessage(
            "TargetDetectedMsg",
            "Eco-Sentinel",
            coords,
            new Dictionary<string, string> {
                { "x", pos.x.ToString("F1") },
                { "y", pos.y.ToString("F1") },
                { "z", pos.z.ToString("F1") }
            }
        );
    }

    public void SendSuspiciousSoundDetected(Vector3 soundPosition)
    {
        string msgString = $"coordinates: x:{soundPosition.x:F1}, y:{soundPosition.y:F1}, z:{soundPosition.z:F1}";

        AiMessageEmitter.Instance.EmitMessage(
            "SuspiciousMovementMsg",
            "Eco-Sentinel",
            msgString,
            new Dictionary<string, string> {
                { "x", soundPosition.x.ToString("F1") },
                { "y", soundPosition.y.ToString("F1") },
                { "z", soundPosition.z.ToString("F1") }
            }
        );
    }

    public void SendBatteryLow(float level)
    {
        AiMessageEmitter.Instance.EmitMessage(
            "LowBatteryMsg",
            "Eco-Sentinel",
            $"battery: {level:F0}%",
            new Dictionary<string, string> { { "level", level.ToString("F0") } }
        );
    }
}

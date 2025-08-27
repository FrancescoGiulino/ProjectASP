using System.Collections.Generic;
using UnityEngine;

public class EnemyMessageSender : MonoBehaviour
{
    [SerializeField] private EnemyTargetDetectionController visualDetection;
    [SerializeField] private EnemySoundListener soundDetection;
    [SerializeField] private EnemyStateController enemy;

    private bool canSendTargetDetectionMsg = true;
    private bool canSendTargetDetectionLowBatteryMsg = true;
    private bool canSendLowBatteryMsg = true;
    private bool canSendBatteryDeplatedMsg = true;

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
        // Target Detected Message
        if (visualDetection.CheckForTargets())
        {
            if (!enemy.HasLowBattery() && canSendTargetDetectionMsg)
            {
                canSendTargetDetectionMsg = false;
                SendTargetDetected(visualDetection.GetDetectedTargetPosition());
            }
        }
        else canSendTargetDetectionMsg = true;

        // Target Detected Message (With Low Battery)
        if (visualDetection.CheckForTargets())
        {
            if (enemy.HasLowBattery() && canSendTargetDetectionLowBatteryMsg)
            {
                canSendTargetDetectionLowBatteryMsg = false;
                SendTargetDetectedLowBattery(visualDetection.GetDetectedTargetPosition());
            }
        }
        else canSendTargetDetectionLowBatteryMsg = true;

        // Low Battery Message
        if (enemy.HasLowBattery() && canSendLowBatteryMsg)
        {
            canSendLowBatteryMsg = false;
            SendBatteryLow(enemy.HealthController.CurrentHealth);
        }
        if (!enemy.HasLowBattery()) canSendLowBatteryMsg = true;

        // Battery Depleted Message
        if (enemy.HasNoBattery() && canSendBatteryDeplatedMsg)
        {
            canSendBatteryDeplatedMsg = false;
            SendBatteryDeplated();
        }
        if (enemy.HealthController.CurrentHealth > 0) canSendBatteryDeplatedMsg = true;
    }

    // -----------------------------------------------
    // Messaggi
    // -----------------------------------------------

    public void SendTargetDetected(Vector3 pos)
    {
        string coords = $"coordinates: x:{pos.x:F1}; y:{pos.y:F1}; z:{pos.z:F1}";

        MessageBus.Instance.EmitMessage(
            "TargetDetectedMsg",
            enemy.name,
            coords,
            new Dictionary<string, string> {
                { "x", pos.x.ToString("F1") },
                { "y", pos.y.ToString("F1") },
                { "z", pos.z.ToString("F1") }
            }
        );
    }

    public void SendTargetDetectedLowBattery(Vector3 pos)
    {
        string coords = $"coordinates: x:{pos.x:F1}; y:{pos.y:F1}; z:{pos.z:F1}";

        MessageBus.Instance.EmitMessage(
            "TargetDetectedLowBatteryMsg",
            enemy.name,
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

        MessageBus.Instance.EmitMessage(
            "SuspiciousMovementMsg",
            enemy.name,
            msgString,
            new Dictionary<string, string> {
                { "x", soundPosition.x.ToString("F1") },
                { "y", soundPosition.y.ToString("F1") },
                { "z", soundPosition.z.ToString("F1") }
            }
        );

        Debug.Log($"[EnemymessageSender] Suspicious Sound Detected!!!!");
    }

    public void SendBatteryLow(float level)
    {
        MessageBus.Instance.EmitMessage(
            "LowBatteryMsg",
            enemy.name,
            $"battery: {level:F0}%",
            new Dictionary<string, string> { { "level", level.ToString("F0") } }
        );
    }

    public void SendBatteryDeplated()
    {
        MessageBus.Instance.EmitMessage(
            "BatteryDepletedMsg",
            enemy.name,
            $"battery: 0%",
            null
        );
    }
}

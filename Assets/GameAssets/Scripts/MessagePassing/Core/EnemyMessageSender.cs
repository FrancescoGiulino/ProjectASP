using UnityEngine;
using System.Collections.Generic;

public class EnemyMessageSender : MonoBehaviour
{
    [SerializeField] private EnemyTargetDetectionController visualDetection;
    [SerializeField] private EnemySoundListener soundDetection;
    [SerializeField] private EnemyStateController enemy;

    [SerializeField] private float investigationCooldown = 8f;
    [SerializeField] private float reinforcementCooldown = 8f;

    private MessageData activeDetectionMessage;

    // Cooldown per messaggi temporizzati
    private Dictionary<string, float> messageCooldowns;
    private Dictionary<string, float> lastSentTimes = new Dictionary<string, float>();

    // Flag per messaggi di tipo informazione
    private bool lowBatterySent = false;
    private bool batteryDepletedSent = false;

    private void Awake()
    {
        messageCooldowns = new Dictionary<string, float>
        {
            { "SuspiciousSound", investigationCooldown },
            { "TargetDetected", reinforcementCooldown },
            { "TargetDetectedLowBattery", reinforcementCooldown }
        };

        if (soundDetection != null)
            soundDetection.OnSuspiciousSoundHeard += SendSuspiciousSoundDetected;
    }

    private void Update()
    {
        HandleTargetDetection();
        HandleBatteryMessages();
    }

    // ---------------------------------------------------
    // Gestione Target Detection
    // ---------------------------------------------------
    private void HandleTargetDetection()
    {
        if (visualDetection.CheckForTargets())
        {
            Vector3 playerPos = visualDetection.GetDetectedTargetPosition();

            if (activeDetectionMessage == null)
            {
                if (!enemy.HasLowBattery() && CanSend("TargetDetected"))
                {
                    activeDetectionMessage = SendTargetDetected(playerPos);
                    MarkSent("TargetDetected");
                }
                else if (enemy.HasLowBattery() && CanSend("TargetDetectedLowBattery"))
                {
                    activeDetectionMessage = SendTargetDetectedLowBattery(playerPos);
                    MarkSent("TargetDetectedLowBattery");
                }
            }
            else
            {
                activeDetectionMessage.X = Mathf.RoundToInt(playerPos.x);
                activeDetectionMessage.Y = Mathf.RoundToInt(playerPos.y);
                activeDetectionMessage.Z = Mathf.RoundToInt(playerPos.z);
                activeDetectionMessage.ParametersText =
                    $"coordinates: x:{playerPos.x:F1}; y:{playerPos.y:F1}; z:{playerPos.z:F1}";
            }
        }
        else
        {
            activeDetectionMessage = null;
        }
    }

    // ---------------------------------------------------
    // Gestione messaggi batteria
    // ---------------------------------------------------
    private void HandleBatteryMessages()
    {
        if (enemy.HasLowBattery() && !lowBatterySent)
        {
            SendBatteryLow(enemy.HealthController.CurrentHealth);
            lowBatterySent = true;
        }

        if (enemy.HasNoBattery() && !batteryDepletedSent)
        {
            SendBatteryDeplated();
            batteryDepletedSent = true;
        }
    }

    // ---------------------------------------------------
    // Cooldown helpers
    // ---------------------------------------------------
    private bool CanSend(string key)
    {
        if (!lastSentTimes.TryGetValue(key, out float lastTime))
            return true;

        float cooldown = messageCooldowns[key];
        return (Time.time - lastTime) >= cooldown;
    }

    private void MarkSent(string key)
    {
        lastSentTimes[key] = Time.time;
    }

    // ---------------------------------------------------
    // Messaggi
    // ---------------------------------------------------
    public MessageData SendTargetDetected(Vector3 pos)
    {
        string coords = $"coordinates: x:{pos.x:F1}; y:{pos.y:F1}; z:{pos.z:F1}";
        return MessageBus.Instance.EmitMessage(
            "TargetDetectedMsg",
            enemy.name,
            coords,
            Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y), Mathf.RoundToInt(pos.z),
            "reinforcement"
        );
    }

    public MessageData SendTargetDetectedLowBattery(Vector3 pos)
    {
        string coords = $"coordinates: x:{pos.x:F1}; y:{pos.y:F1}; z:{pos.z:F1}";
        return MessageBus.Instance.EmitMessage(
            "TargetDetectedLowBatteryMsg",
            enemy.name,
            coords,
            Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y), Mathf.RoundToInt(pos.z),
            "reinforcement"
        );
    }

    public void SendSuspiciousSoundDetected(Vector3 soundPosition)
    {
        if (!CanSend("SuspiciousSound"))
            return;

        string msgString = $"coordinates: x:{soundPosition.x:F1}, y:{soundPosition.y:F1}, z:{soundPosition.z:F1}";
        MessageBus.Instance.EmitMessage(
            "SuspiciousMovementMsg",
            enemy.name,
            msgString,
            Mathf.RoundToInt(soundPosition.x), Mathf.RoundToInt(soundPosition.y), Mathf.RoundToInt(soundPosition.z),
            "investigation"
        );
        MarkSent("SuspiciousSound");
    }

    public void SendBatteryLow(float level)
    {
        MessageBus.Instance.EmitMessage(
            "LowBatteryMsg",
            enemy.name,
            $"battery: {level:F0}%",
            Mathf.RoundToInt(enemy.transform.position.x), Mathf.RoundToInt(enemy.transform.position.y), Mathf.RoundToInt(enemy.transform.position.z),
            "information"
        );
    }

    public void SendBatteryDeplated()
    {
        MessageBus.Instance.EmitMessage(
            "BatteryDepletedMsg",
            enemy.name,
            $"battery: 0%",
            Mathf.RoundToInt(enemy.transform.position.x), Mathf.RoundToInt(enemy.transform.position.y), Mathf.RoundToInt(enemy.transform.position.z),
            "information"
        );
    }
}

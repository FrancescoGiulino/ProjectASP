using UnityEngine;
using System.Text;

public class LogManager : MonoBehaviour
{
    private StringBuilder levelBuffer = new StringBuilder();
    private bool bufferingLevelLogs = false;

    private void OnEnable()
    {
        Application.logMessageReceivedThreaded += HandleLog;
    }

    private void OnDisable()
    {
        Application.logMessageReceivedThreaded -= HandleLog;
    }

    private void HandleLog(string logString, string stackTrace, LogType type)
    {
        // --- Caso 1: log con "Solver executor" ---
        if (logString.Contains("Solver executor"))
        {
            string formattedLog = logString.Replace(";", ";\n");
            Debug.LogWarning(formattedLog+"\n----------------------------------------------------------\n");
            return;
        }

        // --- Caso 2: log con "level" ---
        if (logString.Contains("level"))
        {
            if (levelBuffer.Length > 0)
                levelBuffer.AppendLine();

            levelBuffer.Append(logString);
            bufferingLevelLogs = true;
        }
        else
        {
            // Se avevo accumulato log "level" e adesso arriva altro → stampo il blocco
            FlushLevelLogs();
        }
    }

    private void LateUpdate()
    {
        // Se alla fine del frame restano log "level" in sospeso, li stampo
        FlushLevelLogs();
    }

    private void FlushLevelLogs()
    {
        if (bufferingLevelLogs && levelBuffer.Length > 0)
        {
            Debug.LogWarning($"[Penality]\n{levelBuffer}");
            levelBuffer.Clear();
            bufferingLevelLogs = false;
        }
    }
}

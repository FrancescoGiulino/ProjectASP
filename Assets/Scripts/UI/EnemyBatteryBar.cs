using UnityEngine;
using UnityEngine.UI;

public class EnemyBatteryBar : MonoBehaviour
{
    [Header("Enemy")]
    [SerializeField] private HealthController healthController;

    [Header("Colors")]
    [SerializeField] private Color fullColor = Color.green;
    [SerializeField] private Color emptyColor = Color.red;

    [Header("UI Settings")]
    [SerializeField] private Canvas canvas;
    [SerializeField] private Image batteryFill;

    [Header("Fixed Direction")]
    [Tooltip("Direzione verso cui la barra deve sempre guardare (es. -Z o +Z a seconda della tua scena)")]
    [SerializeField] private Vector3 forwardDirection = Vector3.forward;

    private void Awake()
    {
        if (healthController == null)
            Debug.LogError("healthController non assegnato " + gameObject.name);

        if (canvas == null)
            Debug.LogError("Canvas non assegnato al nemico " + gameObject.name);

        if (batteryFill == null)
            Debug.LogError("Image batteryFill non assegnata al nemico " + gameObject.name);
    }

    private void LateUpdate()
    {
        if (canvas == null || healthController == null) return;

        // Mantiene il canvas rivolto verso una direzione fissa
        canvas.transform.rotation = Quaternion.LookRotation(forwardDirection);

        // Aggiorna la barra batteria in base alla vita attuale
        UpdateBar();

        if (healthController.IsDead)
            OnBatteryDepleted();
    }

    private void UpdateBar()
    {
        if (batteryFill == null) return;

        float ratio = healthController.CurrentHealth / healthController.MaxHealth;
        batteryFill.fillAmount = ratio;

        // Interpolazione tra pieno e vuoto
        batteryFill.color = Color.Lerp(emptyColor, fullColor, ratio);

        Debug.Log($"Update Fill --> Battery: {batteryFill.fillAmount}");
    }

    private void OnBatteryDepleted()
    {
        //Debug.Log(gameObject.name + " si è scaricato.");
    }
}

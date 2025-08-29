using UnityEngine;

public class LightController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private Light lightComponent;
    [SerializeField] private bool lightOnAtStart = true;
    [SerializeField] private Color defaultColor = Color.white;

    private void Awake()
    {
        if (!lightComponent)
            lightComponent = GetComponent<Light>();

        if (lightComponent.type != LightType.Spot)
            Debug.LogWarning($"SpotlightController attached to non-Spotlight object: {gameObject.name}");

        lightComponent.color = defaultColor;
        lightComponent.enabled = lightOnAtStart;
    }

    // === Setters ===
    public void TurnOn() => lightComponent.enabled = true;
    public void TurnOff() => lightComponent.enabled = false;
    public void Toggle() => lightComponent.enabled = !lightComponent.enabled;
    public void SetColor(Color newColor) => lightComponent.color = newColor;
    public void SetIntensity(float newIntensity) => lightComponent.intensity = newIntensity;
    public void SetSpotAngle(float newAngle) => lightComponent.spotAngle = newAngle;

    // === Getters ===
    public bool IsOn() => lightComponent.enabled;
    public Color GetColor() => lightComponent.color;
}

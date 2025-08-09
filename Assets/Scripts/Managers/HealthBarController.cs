using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour
{
    [SerializeField] private GameObject healthbar;
    [SerializeField] private float smoothSpeed = 15f;
    [SerializeField] private Color fullHealthColor, lowHealthColor;

    private Image image;
    private float targetFill;

    private void Start()
    {
        if (!healthbar)
        {
            Debug.LogWarning("Healthbar is null.");
            return;
        }

        image = healthbar.GetComponent<Image>();
        if (!image)
        {
            Debug.LogWarning("Healthbar image is null.");
            return;
        }

        image.fillAmount = 1;
        targetFill = image.fillAmount; // valore iniziale
    }

    public void SetValue(float value)
    {
        // Limita il valore tra 0 e 1
        targetFill = Mathf.Clamp01(value);
    }

    private void Update()
    {
        if (image)
        {
            image.fillAmount = Mathf.Lerp(image.fillAmount, targetFill, smoothSpeed * Time.deltaTime);
            image.color = Color.Lerp(lowHealthColor, fullHealthColor, image.fillAmount);
        }
    }
}

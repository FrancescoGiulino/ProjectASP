using UnityEngine;

public class RotationPatrol : MonoBehaviour
{
    [Header("Rotation Settings")]
    [SerializeField] private Vector3 rotationAxis = Vector3.up;
    [SerializeField] private float maxAngle = 45f;      // angolo massimo da ciascun lato (es. 45° -> ruota tra -45 e +45)
    [SerializeField] private float rotationSpeed = 10f;  // gradi al secondo
    public bool Active { get; set; } = true;

    private float currentAngle = 0f;
    private int direction = 1;

    private void Update()
    {
        if (!Active) return;

        float deltaAngle = rotationSpeed * Time.deltaTime * direction;
        currentAngle += deltaAngle;

        // Inverti direzione se superiamo l'angolo massimo
        if (Mathf.Abs(currentAngle) > maxAngle)
        {
            currentAngle = Mathf.Clamp(currentAngle, -maxAngle, maxAngle);
            direction *= -1;
            deltaAngle = 0f; // evita un piccolo scatto oltre il limite
        }

        // Applica la rotazione
        transform.Rotate(rotationAxis, deltaAngle, Space.Self);
    }
}

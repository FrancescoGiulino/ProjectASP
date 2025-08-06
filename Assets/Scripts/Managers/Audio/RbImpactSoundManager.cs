using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SoundEventComponent))]
public class RbImpactSoundHandler : MonoBehaviour
{
    [Header("Impostazioni Forza")]
    [SerializeField] private float minImpactForce = 1f;
    [SerializeField] private float maxImpactForce = 7f;

    [Header("Tipo di Suono")]
    [SerializeField] private SoundType impactSoundType = SoundType.Hit;

    private SoundEventComponent soundEventComponent;
    private float lastSoundTime = 0f;
    private float minInterval = 0.05f; // previene spam su urti multipli

    private void Awake()
    {
        soundEventComponent = GetComponent<SoundEventComponent>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Filtra collisioni non rilevanti (con trigger, layer inutili ecc. se vuoi)
        if (collision.contactCount == 0) return;

        float impactForce = collision.relativeVelocity.magnitude;

        if (impactForce >= minImpactForce && Time.time - lastSoundTime > minInterval)
        {
            float normalizedVolume = Mathf.InverseLerp(minImpactForce, maxImpactForce, impactForce);
            if (normalizedVolume < 0.1f) normalizedVolume = 0.1f; // volume minimo udibile
            soundEventComponent.Volume = normalizedVolume;

            //Debug.Log($"[ImpactSound] Forza: {impactForce:F2}, Volume: {normalizedVolume:F2}, Oggetto colpito: {collision.collider.name}");

            soundEventComponent.PlaySoundWithVolume(impactSoundType);
            lastSoundTime = Time.time;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Bullet"))
            return;

        if (Time.time - lastSoundTime > minInterval)
        {
            soundEventComponent.Volume = 1f;

            //Debug.Log($"[ImpactTrigger] Colpito da proiettile: {other.name}");

            soundEventComponent.PlaySoundWithVolume(impactSoundType);
            lastSoundTime = Time.time;
        }
    }
}

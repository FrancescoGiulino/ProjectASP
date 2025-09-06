using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SoundEventComponent))]
public class RbImpactSoundHandler : MonoBehaviour
{
    [SerializeField] private SoundType impactSound = SoundType.Hit;
    [SerializeField] private float minVelocity = 1f;
    [SerializeField] private float maxVelocity = 10f;

    [Header("Trigger Settings")]
    [SerializeField] private float minInterval = 0.1f; // tempo minimo tra due suoni di trigger

    private Rigidbody rb;
    private SoundEventComponent soundComponent;
    private float lastSoundTime;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        soundComponent = GetComponent<SoundEventComponent>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        float impact = collision.relativeVelocity.magnitude;

        if (impact >= minVelocity)
        {
            float normalized = Mathf.InverseLerp(minVelocity, maxVelocity, impact);
            soundComponent.Volume = normalized;
            soundComponent.PlaySoundWithVolume(impactSound,soundComponent.Volume);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Bullet"))
            return;

        if (Time.time - lastSoundTime > minInterval)
        {
            soundComponent.Volume = 1f;
            soundComponent.PlaySoundWithVolume(impactSound,soundComponent.Volume);
            lastSoundTime = Time.time;
        }
    }
}

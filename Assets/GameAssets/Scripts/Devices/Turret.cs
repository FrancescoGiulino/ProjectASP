using UnityEngine;

public class Turret : Device
{
    [Header("Turret Settings")]
    [SerializeField] private Shooter shooter;
    [SerializeField] private float fireRate = 1f; // Proiettili al secondo
    private float nextFireTime = 0f;

    protected override void HandleLogic()
    {
        if (!rotationPatrol) return;
        rotationPatrol.SetActive(active);
        HandleTargetDetection();
        
        if (active)
        {
            if (targetDetectionController.CheckForTargets())
            {
                rotationPatrol.AimAt(targetDetectionController.GetDetectedTargetPosition());

                if (Time.time >= nextFireTime)
                {
                    nextFireTime = Time.time + 1f / fireRate;
                    shooter.Shoot();
                    
                    soundEventComponent?.PlaySound(SoundType.Attack); // emit sound
                }
            }
        }
    }
}

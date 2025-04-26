using UnityEngine;

public class DoubleTurret : StateChangeable
{
    [SerializeField] private TurretShooting turretShooting;
    [SerializeField] private Light securityLight;
    [SerializeField] private SphereCollider sphereCollider;
    public float rotationSpeed = 30f; // Velocità di rotazione in gradi al secondo
    public float maxRotationAngle = 45f; // Angolo massimo di rotazione a destra e sinistra
    public Transform target; // Bersaglio della torretta
    public float activationRange = 8f; // Raggio di attivazione della torretta
    private float currentAngle = 0f; // Angolo corrente della torretta
    private int rotationDirection = 1; // Direzione della rotazione (1 = destra, -1 = sinistra)
    private Quaternion initialRotation; // Rotazione iniziale della torretta
    private bool returningToInitial = false; // Flag per indicare se sta tornando alla posizione iniziale
    private bool targetDetected = false; // Flag per indicare se il bersaglio è stato rilevato

    protected override void Awake()
    {
        base.Awake();

        // Trova il componente SphereCollider nella gerarchia
        sphereCollider=GetComponentInChildren<SphereCollider>();
        if (sphereCollider == null) {
            Debug.LogError("Componente CapsuleCollider non trovato nella gerarchia di " + gameObject.name);
        }
    }

    void Start()
    {
        // Salva la rotazione iniziale della torretta
        initialRotation = transform.localRotation;
    }

    protected override void Update() {
        base.Update();

        // Ruota solo se lo stato è attivo
        if (state)
        {
            // Controlla se il bersaglio è entro il raggio di attivazione
            if (targetDetected)
            {
                returningToInitial = false; // Interrompe il ritorno alla posizione iniziale
                RotateTowardsTarget();                    
            }
            else
            {
                ReturnToInitialPosition(); // Torna gradualmente alla posizione iniziale
            }
            
        }

        else
        {
            // Abilita/disabilita lo sparo in base allo stato
            if (turretShooting != null) { turretShooting.SetActive(false); }
        }

        // Abilita/disabilita la luce in base allo stato
        if (securityLight != null) { securityLight.enabled = state; }

        // Abilita/disabilita il rilevamento delle collisioni in base allo stato
        if (sphereCollider != null) { sphereCollider.enabled = state; }
    }

    void RotateTowardsTarget()
    {
        if (target == null) return;

        Vector3 direction = target.position - transform.position;

        // Mantieni solo la componente Y della rotazione
        direction.y = 0f;

        // Calcola la rotazione verso il bersaglio
        Quaternion rotation = Quaternion.LookRotation(direction);

        // Applica la rotazione limitata sull'asse Y
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);
    }

    void ReturnToInitialPosition()
    {
        if (!returningToInitial)
        {
            // Interpola gradualmente verso la rotazione iniziale
            transform.localRotation = Quaternion.Lerp(transform.localRotation, initialRotation, Time.deltaTime * rotationSpeed / 10f);

            // Controlla se la torretta è vicina alla posizione iniziale
            if (Quaternion.Angle(transform.localRotation, initialRotation) < 0.1f)
            {
                returningToInitial = true; // Una volta raggiunta la posizione iniziale, inizia a oscillare
                currentAngle = 0f; // Resetta l'angolo corrente
            }
        }
        else
        {
            // Riprendi l'oscillazione
            Oscillate();
        }
    }

    void Oscillate()
    {
        // Calcola l'angolo da ruotare in questo frame
        float rotationStep = rotationSpeed * Time.deltaTime * rotationDirection;

        // Aggiorna l'angolo corrente
        currentAngle += rotationStep;

        // Controlla se l'angolo corrente supera i limiti
        if (currentAngle > maxRotationAngle)
        {
            currentAngle = maxRotationAngle;
            rotationDirection = -1; // Cambia direzione verso sinistra
        }
        else if (currentAngle < -maxRotationAngle)
        {
            currentAngle = -maxRotationAngle;
            rotationDirection = 1; // Cambia direzione verso destra
        }

        // Applica la rotazione oscillante
        transform.localRotation = Quaternion.Euler(0f, currentAngle, 0f);
    }

    public void SetLightColor(Color color) {
        if (securityLight != null) {
            securityLight.color = color; // Cambia il colore della luce
        } else {
            Debug.LogWarning("Componente Light non trovato, impossibile cambiare il colore.");
        }
    }

    public void SetTargetDetected(bool detected)
    {
        targetDetected = detected;
        if (detected)
        {
            SetLightColor(Color.red);
            turretShooting.SetActive(true); // Abilita lo sparo
        }
        else
        {
            SetLightColor(Color.white);
            turretShooting.SetActive(false); // Disabilita lo sparo
        }
    }
}

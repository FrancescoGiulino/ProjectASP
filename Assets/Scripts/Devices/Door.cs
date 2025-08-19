using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshObstacle))]
public class Door : Device
{
    private NavMeshObstacle obstacle;
    private bool isOpenApplied; // stato già applicato all'obstacle
    [SerializeField] private BoxCollider slidingDoorBoxCollider;

    protected void Awake()
    {
        obstacle = GetComponent<NavMeshObstacle>();
        if (obstacle == null)
        {
            Debug.LogError("Door richiede un NavMeshObstacle sullo stesso GameObject.");
        }
    }

    protected override void Start()
    {
        base.Start();
        // All'avvio allinea subito il NavMeshObstacle allo stato 'active'
        ApplyState(force: true);
    }

    protected override void Update()
    {
        base.Update();
        // Applica solo se lo stato 'active' è cambiato rispetto all'ultimo applicato
        ApplyState();
    }

    // Se vuoi richiamarle dall'esterno (leve/animazioni), va bene.
    // Non toccano 'active': quello lo decidi altrove (es. Device o Animator).
    public void Open()
    {
        if (obstacle == null) return;
        obstacle.carving = false;
        obstacle.enabled = false; // porta aperta: non blocca la NavMesh
        isOpenApplied = true;
        slidingDoorBoxCollider.enabled = false;
    }

    public void Close()
    {
        if (obstacle == null) return;
        obstacle.enabled = true;
        obstacle.carving = true;          // porta chiusa: blocca la NavMesh
        obstacle.carveOnlyStationary = false; // carve immediato anche se si muove
        isOpenApplied = false;
        slidingDoorBoxCollider.enabled = true;
    }

    private void ApplyState(bool force = false)
    {
        if (obstacle == null) return;

        // 'active' = porta aperta (come nel tuo codice precedente)
        // Se nel tuo progetto 'active' significa "porta CHIUSA", inverti semplicemente i rami.
        bool shouldBeOpen = active;

        if (!force && shouldBeOpen == isOpenApplied)
            return;

        if (shouldBeOpen)
            Open();
        else
            Close();
    }

#if UNITY_EDITOR
    // Qualche comodità in editor: vedere subito l’effetto quando cambi 'active' da Inspector
    private void OnValidate()
    {
        obstacle = GetComponent<NavMeshObstacle>();
        if (obstacle == null) return;

        if (!Application.isPlaying)
        {
            if (active)
            {
                obstacle.carving = false;
                obstacle.enabled = true;
                isOpenApplied = true;
            }
            else
            {
                obstacle.enabled = true;
                obstacle.carving = true;
                isOpenApplied = false;
            }
        }
    }
#endif
}

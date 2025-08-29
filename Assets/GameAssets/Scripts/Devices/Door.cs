using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(BoxCollider))]
public class Door : Device
{
    [Header("Door Settings")]
    [SerializeField] private BoxCollider boxCollider;
    [SerializeField] private string enemyTag = "Enemy";
    [SerializeField] private Vector3 detectionOffset = Vector3.zero;
    [SerializeField] private Vector3 detectionSize = new Vector3(2, 2, 2);

    private bool enemyNearby = false;

    protected override void Update()
    {
        // rilevamento nemici
        Vector3 center = transform.position + detectionOffset;
        Collider[] hits = Physics.OverlapBox(center, detectionSize * 0.5f, transform.rotation);

        enemyNearby = false;
        foreach (Collider hit in hits)
        {
            if (hit.CompareTag(enemyTag))
            {
                enemyNearby = true;
                break;
            }
        }

        // se non ci sono nemici → normale gestione Device (player può sbloccare la porta)
        if (!enemyNearby)
        {
            base.Update();

            // se la porta è stata sbloccata, disattiva il collider
            boxCollider.enabled = !active ? true : false;
        }
        else
        {
            // se c'è un nemico vicino: forzo l'animazione di apertura
            if (animated && animationController.GetAnimator() != null)
                animationController.PlayAnimation("Activate");

            // ma lascio il collider attivo → player non può entrare
            //boxCollider.enabled = true;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Matrix4x4 rotationMatrix = Matrix4x4.TRS(transform.position + detectionOffset, transform.rotation, Vector3.one);
        Gizmos.matrix = rotationMatrix;
        Gizmos.DrawWireCube(Vector3.zero, detectionSize);
    }
}

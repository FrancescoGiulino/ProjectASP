using UnityEngine;

public class BulletLife : MonoBehaviour
{
    public float lifeTime = 3f; // durata della vita del proiettile in secondi

    void Start()
    {
        Destroy(gameObject, lifeTime); // distrugge dopo 3 secondi
    }

    void OnCollisionEnter(Collision collision)
    {
        // Distrugge il proiettile quando colpisce un oggetto
        Destroy(gameObject);
    }
}

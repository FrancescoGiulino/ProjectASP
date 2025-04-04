using UnityEngine;

public class BoxFactory : StateChangeable {
    [SerializeField] private GameObject[] boxPrefabs; // Array di prefab delle scatole da generare
    private bool boxGenerated = false; // Indica se la scatola è già stata generata

    public override void HandleState() {
        // Calcola lo stato in base alle dipendenze
        if (dependency != null && dependency.Length > 0) {
            foreach (var interactable in dependency) {
                if (!interactable.IsActive()) {
                    state = false;
                    boxGenerated = false; // Resetta lo stato per consentire una nuova generazione
                    return;
                }
            }
        }
        state = true;

        // Se lo stato è attivo e la scatola non è stata ancora generata, genera la scatola
        if (state && !boxGenerated) {
            GenerateBox();
        }
    }

    private void GenerateBox() {
        // Controlla se l'array di prefab non è vuoto
        if (boxPrefabs != null && boxPrefabs.Length > 0) {
            // Scegli un prefab casuale dall'array
            int randomIndex = Random.Range(0, boxPrefabs.Length);
            GameObject selectedPrefab = boxPrefabs[randomIndex];

            // Calcola la posizione della scatola con un offset verticale
            Vector3 boxPosition = transform.position;
            boxPosition.y += 2f; // Aggiungi un offset verticale (ad esempio, 2 unità più in alto)

            // Genera la scatola nella posizione calcolata
            Instantiate(selectedPrefab, boxPosition, Quaternion.identity);
            boxGenerated = true; // Imposta la variabile per evitare ulteriori generazioni
        } else {
            Debug.LogError("Array di prefab delle scatole è vuoto o non assegnato!");
        }
    }
}
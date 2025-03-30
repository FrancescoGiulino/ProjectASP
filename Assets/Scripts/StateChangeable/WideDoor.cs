using UnityEngine;

public class WideDoor : StateChangeable{
    private BoxCollider boxCollider;

    protected void Awake(){
        // Inizializza il BoxCollider
        boxCollider = GetComponent<BoxCollider>();
        if (boxCollider == null){
            Debug.LogError("BoxCollider non trovato! Assicurati che il componente BoxCollider sia presente.");
        }
    }

    // La keyword new serve a nascondere il metodo Update della classe padre
    // In questo caso non è necessario, ma è una buona pratica per evitare confusione
    protected new void Update(){
        base.Update();
        if (state){
            OpenDoor();
        }else{
            CloseDoor();
        }
    }

    private void OpenDoor(){
        // Logica per aprire la porta
        Debug.Log("Porta aperta!");
        boxCollider.enabled = false;
    }
    private void CloseDoor(){
        // Logica per chiudere la porta
        Debug.Log("Porta chiusa!");
        boxCollider.enabled = true;
    }
}

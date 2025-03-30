using UnityEngine;

public class WideDoor : StateChangeable{
    private BoxCollider boxCollider;

    private new void Awake(){
        base.Awake();
        boxCollider = GetComponent<BoxCollider>();
    }

    private new void Update(){
        base.Update();
        if (state){
            boxCollider.enabled = false; // Disabilita il collider quando la porta è aperta
        }else{
            boxCollider.enabled = true; // Abilita il collider quando la porta è chiusa
        }
    }
}

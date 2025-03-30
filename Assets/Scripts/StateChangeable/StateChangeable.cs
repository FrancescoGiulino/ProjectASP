using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class StateChangeable : MonoBehaviour{
    [SerializeField] protected Interactable[] dependency;
    [SerializeField] protected bool state;

    protected void Update(){
        EvaluateState();
    }

    // Questo metodo serve a calcolare lo stato dell'oggetto in base allo stato degli interactable da cui dipende
    protected void EvaluateState(){
        if (dependency!=null && dependency.Length>0){
            foreach (var interactable in dependency){
                if (!interactable.IsActive()){
                    state=false;
                    return;
                }
            }
        }
        state=true;
    }
}

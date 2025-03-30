using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour{
    public event EventHandler OnInteractAction;
    private PlayerInputActions playerInputActions;

    private void Awake(){
        playerInputActions=new PlayerInputActions();
        playerInputActions.Player.Enable();

        playerInputActions.Player.Interact.performed+=Event_InteractPerformed;
        // in questo caso:
        //  playerInputActions.Player.Interact.performed indica che l'azione è stata eseguita
        //  e quindi viene eseguito il metodo Event_InteractPerformed
    }

    private void Event_InteractPerformed(UnityEngine.InputSystem.InputAction.CallbackContext obj){
        /*if (OnInteractAction!=null){
            OnInteractAction(this,EventArgs.Empty);
        }*/
        //equivale a scrivere:
        OnInteractAction?.Invoke(this,EventArgs.Empty);
        // Che vuol dire:
        //  se OnInteractAction non è null, allora invoca l'evento
        //  e passa come parametri this (il GameInput) e EventArgs.Empty (un oggetto vuoto)
        //  Ciò avvisa tutti gli iscritti all'evento che l'azione di interazione è stata eseguita.
    }

    public Vector2 GetInputVectorNormalized(){
        Vector2 input=playerInputActions.Player.Move.ReadValue<Vector2>();
        input=input.normalized;
        return input;
    }
}

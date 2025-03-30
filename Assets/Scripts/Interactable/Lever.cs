using Unity.VisualScripting;
using UnityEngine;

public class Lever : Interactable {
    public override void Interact() {
        active = !active;
        PlayAnimation();
    }
}
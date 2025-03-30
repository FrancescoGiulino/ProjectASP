using UnityEngine;

public class PlayerAnimationStateController : MonoBehaviour{
    private Animator animator;
    private PlayerController playerController;

    private void Start(){
        animator=GetComponentInChildren<Animator>();
        playerController=GetComponent<PlayerController>();
    }
    private void Update(){
        animator.SetBool("isMoving",playerController.IsMoving());
    }
}

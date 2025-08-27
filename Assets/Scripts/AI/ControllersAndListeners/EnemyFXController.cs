using UnityEngine;

public class EnemyFXController : MonoBehaviour
{
    [Header("Extra Controllers:")]
    [SerializeField] private LightController lightController;
    [SerializeField] private SoundEventComponent soundEventComponent;
    [SerializeField] private AnimationController animationController;
    [SerializeField] private EnemyStateController enemy;

    public void HandleFX(string currentState)
    {
        SetLightColor(currentState);
    }

    public void SetLightColor(string currentState)
    {
        if (currentState == "ChaseState") lightController?.SetColor(Color.red);
        else if (currentState == "LookState" || currentState == "CheckState") lightController?.SetColor(Color.yellow);
        else if (currentState == "GoToRechargeState") lightController?.SetColor(Color.cyan);
        else lightController?.SetColor(Color.white);
    }

    private void Update()
    {
        animationController.SetBool("isWalking", enemy.IsWalking);
        animationController.SetBool("isRunning", enemy.IsRunning);

        if (enemy.IsRunning) animationController.SetBool("isWalking", false);
    }
}

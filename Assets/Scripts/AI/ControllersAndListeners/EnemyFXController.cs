using UnityEngine;

public class EnemyFXController : MonoBehaviour
{
    [Header("Extra Controllers:")]
    [SerializeField] private LightController lightController;
    [SerializeField] private SoundEventComponent soundEventComponent;

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
}

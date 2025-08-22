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
        else lightController?.SetColor(Color.white);
    }
}

using UnityEngine;

public class VictoryTrigger : MonoBehaviour
{
    [SerializeField] private string playerTag = "Player"; // Assicurati che il tuo player abbia questo tag

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            Debug.Log("Player has won!");
            PauseManager.Instance.DisablePause = true;
            PlayerController.Instance.CanMove = false;
            PlayerController.Instance.GetRigidbody().isKinematic = true; // Disabilita la fisica del player
            //PlayerController.Instance.GetSoundEventComponent().PlaySound(SoundType.Victory);
            LevelUIManager.Instance.ActivatePauseMenu();
            LevelUIManager.Instance.GetMenuHandler().ShowVictoryScreen();
        }
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneEventsDispatcher : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(DelayedHandleMusic());
    }

    private IEnumerator DelayedHandleMusic()
    {
        yield return null; // Aspetta un frame per assicurarti che la scena sia completamente attiva
        Debug.Log($"[SceneEventsDispatcher] Scena attiva confermata: {SceneManager.GetActiveScene().name}");
        GameManager.Instance.GetMusicManager()?.HandleMusicForCurrentScene();
    }
}

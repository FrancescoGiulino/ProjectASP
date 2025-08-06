using UnityEngine;

public class SceneMusicHandler : MonoBehaviour
{
    void Start()
    {
        Debug.Log("[SceneMusicHandler] Start called, handling music for current scene.");
        GameManager.Instance.GetMusicManager().HandleMusicForCurrentScene();
    }
}

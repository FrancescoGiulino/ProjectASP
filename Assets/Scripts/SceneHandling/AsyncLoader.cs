using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class AsyncLoader
{
    public static void LoadScene(MonoBehaviour caller, string sceneName)
    {
        caller.StartCoroutine(LoadSceneAsync(sceneName));
    }

    private static IEnumerator LoadSceneAsync(string sceneName)
    {
        Debug.Log($"[AsyncLoader] Loading scene: {sceneName}");
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        // Aspetta il completamento del caricamento
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        // Setta la scena attiva
        Scene loadedScene = SceneManager.GetSceneByName(sceneName);
        if (loadedScene.IsValid())
        {
            SceneManager.SetActiveScene(loadedScene);
            Debug.Log($"[AsyncLoader] Scene {sceneName} set as active");
        }
        else
        {
            Debug.LogWarning($"[AsyncLoader] Scene {sceneName} was not found or is invalid");
        }

        // Trova il MusicManager e chiedigli di aggiornare la musica
        var musicManagers = Object.FindObjectsByType<MusicManager>(FindObjectsSortMode.None);
        if (musicManagers.Length > 0 && musicManagers[0] != null)
        {
            musicManagers[0].HandleMusicForCurrentScene();
        }
        else
        {
            Debug.LogWarning("[AsyncLoader] MusicManager not found in the loaded scene.");
        }
    }
}

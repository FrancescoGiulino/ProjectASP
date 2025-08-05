using UnityEngine;

public static class AsyncLoader
{
    public static void LoadScene(string sceneName)
    {
        Debug.Log($"Loading scene: {sceneName}");
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName);
    }
}

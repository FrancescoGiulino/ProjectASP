public static class SceneEvents
{
    public delegate void SceneLoadedHandler(string sceneName);
    public static event SceneLoadedHandler OnSceneLoaded;

    public static void TriggerSceneLoaded(string sceneName)
    {
        OnSceneLoaded?.Invoke(sceneName);
    }
}
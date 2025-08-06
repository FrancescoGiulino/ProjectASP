using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Managers")]
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private MusicManager musicManager;
    [SerializeField] private SoundManager soundManager;

    private void Awake()
    {
        // Singleton base
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        InitializeManagers();
    }

    private void InitializeManagers()
    {
        if (soundManager != null) soundManager.Init();
        else Debug.LogWarning("SoundManager non assegnato nel GameManager!");
    }

    public AudioManager GetAudioManager() => audioManager;
    public MusicManager GetMusicManager() => musicManager;
    public SoundManager GetSoundManager() => soundManager;
}

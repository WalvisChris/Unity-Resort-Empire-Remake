using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // --- SCOPE ---
    // - background music shuffling
    // - sound effects (SFX)

    public static AudioManager instance;

    public AudioClip[] backgroundMusic;
    private int bgMusicIndex = 0;

    public float musicVolume = 1f;
    public float SFXVolume = 1f;

    private AudioSource audioSource;

    void Awake()
    {
        if (instance == null || instance != this) instance = this;
        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        PlayNextBackgroundMusic();
    }

    void Update()
    {
        if (!audioSource.isPlaying) PlayNextBackgroundMusic();
    }

    private void PlayNextBackgroundMusic()
    {
        if (backgroundMusic.Length == 0) return;
        audioSource.clip = backgroundMusic[bgMusicIndex];
        audioSource.volume = musicVolume;
        audioSource.Play();
        bgMusicIndex = (bgMusicIndex + 1) % backgroundMusic.Length;
    }

    public void ChangeMusicVolume(float v) => audioSource.volume = v;

    public void ChangeSFXVolume(float v) => SFXVolume = v;
}

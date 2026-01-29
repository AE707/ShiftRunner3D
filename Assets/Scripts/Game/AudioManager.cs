using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Background Music")]
    public AudioClip backgroundMusic;
    private AudioSource musicSource;
    public float baseMusicPitch = 1f;
    public float maxMusicPitch = 1.5f;

    [Header("SFX")]
    public AudioClip laneSwitchSound;
    public AudioClip jumpSound;
    public AudioClip collisionSound;
    public AudioClip uiClickSound;
    public AudioClip milestoneSound;

    private AudioSource sfxSource;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Create two AudioSources: one for music, one for SFX
        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.loop = true;
        musicSource.playOnAwake = false;
        musicSource.volume = 0.5f;

        sfxSource = gameObject.AddComponent<AudioSource>();
        sfxSource.playOnAwake = false;
        sfxSource.volume = 0.7f;
    }

    void Start()
    {
        if (backgroundMusic != null)
        {
            musicSource.clip = backgroundMusic;
            musicSource.Play();
        }
    }

    /// <summary>
    /// Adjust music pitch based on game difficulty (0-1)
    /// </summary>
    public void SetMusicPitch(float difficulty)
    {
        if (musicSource != null)
        {
            musicSource.pitch = Mathf.Lerp(baseMusicPitch, maxMusicPitch, difficulty);
        }
    }

    public void PlayLaneSwitch()
    {
        PlaySFX(laneSwitchSound);
    }

    public void PlayJump()
    {
        PlaySFX(jumpSound);
    }

    public void PlayCollision()
    {
        PlaySFX(collisionSound);
    }

    public void PlayUIClick()
    {
        PlaySFX(uiClickSound);
    }

    public void PlayMilestone()
    {
        PlaySFX(milestoneSound);
    }

    private void PlaySFX(AudioClip clip)
    {
        if (clip != null && sfxSource != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }

    public void StopMusic()
    {
        if (musicSource != null)
            musicSource.Stop();
    }

    public void SetMusicVolume(float volume)
    {
        if (musicSource != null)
            musicSource.volume = Mathf.Clamp01(volume);
    }

    public void SetSFXVolume(float volume)
    {
        if (sfxSource != null)
            sfxSource.volume = Mathf.Clamp01(volume);
    }
}

using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
    [SerializeField] private AudioSource sfxAudioSource;
    public AudioSource SfxAudioSource
    {
        get
        {
            return sfxAudioSource;
        }
    }

    [Header("Audio Clip SFX"), Space(5)]
    [SerializeField] private AudioClip hornClip;
    public AudioClip HornClip
    {
        get
        {
            return hornClip;
        }
    }
    [SerializeField] private AudioClip maximizingSwitchClip;
    public AudioClip MaximizingSwitchClip
    {
        get
        {
            return maximizingSwitchClip;
        }
    }
    [SerializeField] private AudioClip buttonClip;
    public AudioClip ButtonClickClip
    {
        get
        {
            return buttonClip;
        }
    }

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public void PlayOneShotSFX(AudioClip clip)
    {
        SfxAudioSource.loop = false;

        if(clip != null)
            SfxAudioSource.PlayOneShot(clip);
        else
            Debug.LogError("Audio Clip Not Found!");
    }

    public void PlaySFX(AudioClip clip, bool isLoop = false)
    {
        SfxAudioSource.loop = isLoop;
        SfxAudioSource.clip = clip;

        if(clip != null)
            SfxAudioSource.Play();
        else
            Debug.LogError("Audio Clip Not Found!");
    }

    public void StopAudio()
    {
        if(SfxAudioSource.isPlaying)
            SfxAudioSource.Stop();
    }
}

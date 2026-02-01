using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource _musicSource;
    [SerializeField] private AudioSource _sfxSource;

    [Header("Background Music")]
    [SerializeField] private AudioClip _bgMusic;

    [Header("Sound Effects")]
    [SerializeField] private AudioClip[] _recruitSounds;

    [SerializeField] private AudioClip _changeMask;
    [SerializeField] private AudioClip _deathSound;
    [SerializeField] private AudioClip _menuClickSound;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject); 
        }
    }

    private void Start()
    {
        PlayMusic(_bgMusic);
    }

    #region Music Control

    public void PlayMusic(AudioClip musicClip)
    {
        if (musicClip == null) return;

        _musicSource.clip = musicClip;
        _musicSource.loop = true;
        _musicSource.Play();
    }

    public void StopMusic()
    {
        _musicSource.Stop();
    }

    #endregion

    #region SFX Methods

    public void PlayRecruitSound()
    {
        if (_recruitSounds.Length == 0) return;
        
        int randomIndex = Random.Range(0, _recruitSounds.Length);
        AudioClip randomClip = _recruitSounds[randomIndex];

        _sfxSource.PlayOneShot(randomClip);
    }

    public void PlayDeathSound()
    {
        PlaySFX(_deathSound);
    }

    public void PlayMenuClick()
    {
        PlaySFX(_menuClickSound);
    }

    public void PlayChangeMaskSound()
    {
        PlaySFX(_changeMask);
    }
    
    private void PlaySFX(AudioClip clip)
    {
        if (clip != null)
        {
            _sfxSource.PlayOneShot(clip);
        }
    }

    #endregion
}
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance;

    [SerializeField] private AudioSource[] _kickSounds;
    [SerializeField] private Vector2 _kickSoundVolumeRange = new Vector2(0.5f, 0.7f);
    [SerializeField] private AudioSource[] _hitSounds;
    [SerializeField] private Vector2 _hitSoundVolumeRange = new Vector2(0.2f, 0.4f);

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
    }

    public static void PlayKickSound()
    {
        _instance.PlayKickSoundInternal();
    }

    public static void PlayHitSound()
    {
        _instance.PlayHitSoundInternal();
    }

    public void PlayKickSoundInternal()
    {
        PlayRandom(_instance._kickSounds, _instance._kickSoundVolumeRange);
    }

    public void PlayHitSoundInternal()
    {
        PlayRandom(_instance._hitSounds, _instance._hitSoundVolumeRange);
    }

    public void PlayRandom(AudioSource[] audioSources, Vector2 volumeRange)
    {
        int randomIndex = Random.Range(0, audioSources.Length);
        AudioSource audioSource = audioSources[randomIndex];
        audioSource.volume = Random.Range(volumeRange.x, volumeRange.y);
        audioSource.Play();
    }
}

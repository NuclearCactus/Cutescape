using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public static SFXManager Instance;

    [Header("Player Sounds")]
    public AudioClip walkSound;
    public AudioClip jumpSound;
    public AudioClip fallDeathSound;

    [Header("NPC Sounds")]
    public AudioClip[] cuteNPCSounds;
    public AudioClip[] realNPCSounds;

    [Header("Phone Sounds")]
    public AudioClip phoneOnSound;
    public AudioClip phoneOffSound;

    [Header("Battery Sounds")]
    public AudioClip batteryPickupSound;
    public AudioClip batteryEmptySound;     // battery level hits 0 but player still has ammo
    public AudioClip batteryFullDeathSound; // player fully dies when no battery ammo left

    private AudioSource audioSource;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            audioSource = GetComponent<AudioSource>();
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            return; // prevent further execution
        }
    }


    public void PlaySFX(AudioClip clip)
    {
        if (clip != null) audioSource.PlayOneShot(clip);
    }

    public void PlayRandomSFX(AudioClip[] clips)
    {
        if (clips.Length == 0) return;
        int index = Random.Range(0, clips.Length);
        PlaySFX(clips[index]);
    }
}
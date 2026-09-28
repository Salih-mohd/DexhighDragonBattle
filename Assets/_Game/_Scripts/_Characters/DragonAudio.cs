using UnityEngine;

public class DragonAudio : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private DragonHealth health;

    [Header("SFX")]
    [SerializeField] private AudioClip hitSFX;
    [SerializeField] private AudioClip fireSFX;
    [SerializeField] private AudioClip takeOffSFX;

    private void OnEnable()
    {
        health.OnDamaged += PlayHit;
    }

    private void OnDisable()
    {
        health.OnDamaged -= PlayHit;
    }

    private void PlayHit(float damage)
    {
        PlayClip(hitSFX);
    }

    public void PlayFire()
    {
        PlayClip(fireSFX);
    }

    public void PlayTakeOff()
    {
        PlayClip(takeOffSFX);
    }

    private void PlayClip(AudioClip clip)
    {
        if (clip == null || audioSource == null)
            return;

        audioSource.PlayOneShot(clip);
    }
}
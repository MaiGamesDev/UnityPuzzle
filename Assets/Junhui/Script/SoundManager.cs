using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    private static SoundManager instance;


    public static SoundManager Instance
    {
        get
        {
            if (instance == null) instance = new SoundManager();
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public AudioSource audioSource;

    private void Start()
    {
        audioSource.volume = 0.5f;
    }

    public void PlaySound(AudioClip clip)
    {
        // ���� ���
        audioSource.PlayOneShot(clip);
    }

    public void PlayLoopSound(AudioClip clip)
    {
        // BGM ���
        audioSource.loop = true;
        audioSource.clip = clip;
        audioSource.Play();
    }
}

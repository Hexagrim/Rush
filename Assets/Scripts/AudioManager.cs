using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public AudioClip Jump, Die, Dash, DoubleJump , slam;
    public AudioSource Sfx;
    AudioSource BgMusic;

    public static AudioManager Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        BgMusic = GetComponent<AudioSource>();
    }
    void Update()
    {
        if(SceneManager.GetActiveScene().name == "MainMenu")
        {
            BgMusic.volume = 0.1f;
        }
        else if(SceneManager.GetActiveScene().name == "End")
        {
            BgMusic.volume = 0f;
        }
        else
        {
            BgMusic.volume = 0.05f;
        }
    }
    public void PlaySfx(AudioClip s)
    {
        if(Sfx != null)
        {
            Sfx.pitch = Random.Range(0.85f, 1.15f);
            Sfx.PlayOneShot(s);

        }
    }
}

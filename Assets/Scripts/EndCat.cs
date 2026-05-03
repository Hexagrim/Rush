using System.Collections;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class EndCat : MonoBehaviour
{
    public Animator cat;
    public AudioSource catAudio;
    public LayerMask catLayer;
    Coroutine SpinCat;

    bool stopped = true;

    bool overCat;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
      
        if (overCat && stopped)
        {
             SpinCat = StartCoroutine(StartCat());

        }
        else if(!stopped && !overCat)
        {
            StopCat();
        }
    }
    void FixedUpdate()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 10f;

        Vector2 worldPos = Camera.main.ScreenToWorldPoint(mousePos);

        overCat = Physics2D.OverlapCircle(worldPos, 0.01f, catLayer);
    }
    IEnumerator StartCat()
    {
        cat.SetBool("playing", true);
        stopped = false;
        catAudio.Play();

        float time = 0f;
        float duration = 5f;

        while (time < duration)
        {
            time += Time.deltaTime;

            catAudio.pitch = Mathf.Lerp(0.95f, 1.2f, time / duration);

            yield return null;
        }

        catAudio.pitch = 1.2f;
        SceneManager.LoadSceneAsync("MainMenu");

    }

    void StopCat()
    {
        if (SpinCat != null)
        {
            StopCoroutine(SpinCat);
        }
        stopped = true;
        catAudio.pitch = 0.95f;
        cat.SetBool("playing", false);
        catAudio.Stop();
    }


}

using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    public Animator T_Anim;

    bool done;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (!done)
            {
                done = true;
                StartCoroutine(End());
            }
        }
    }

    IEnumerator End()
    {
        T_Anim.SetTrigger("fadeOut");
        yield return new WaitForSeconds(0.25f);
        SceneManager.LoadSceneAsync("End");
        PlayerPrefs.DeleteAll();
    }
}

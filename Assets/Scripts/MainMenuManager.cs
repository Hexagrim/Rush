using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{

    public Animator T_Anim;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Continue()
    {
        StartCoroutine(C());
    }
    public void NewSave()
    {
        StartCoroutine(S());
    }
    public void Quit()
    {
        Application.Quit();
    }
    IEnumerator C()
    {
        T_Anim.SetTrigger("fadeOut");
        yield return new WaitForSeconds(0.25f);
        SceneManager.LoadSceneAsync("MainMap");
    }
    IEnumerator S()
    {
        T_Anim.SetTrigger("fadeOut");
        PlayerPrefs.DeleteAll();
        yield return new WaitForSeconds(0.25f);
        SceneManager.LoadSceneAsync("MainMap");
    }
}

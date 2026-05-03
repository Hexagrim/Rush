using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{

    public Animator T_Anim;

    bool ActionDone = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Continue()
    {
        if (ActionDone) return;
        StartCoroutine(C());
    }
    public void NewSave()
    {
        if (ActionDone) return;
        StartCoroutine(S());
    }
    public void Quit()
    {
        if (ActionDone) return;
        Application.Quit();
    }
    IEnumerator C()
    {
        T_Anim.SetTrigger("fadeOut");
        ActionDone = true;
        yield return new WaitForSeconds(0.25f);
        SceneManager.LoadSceneAsync("MainMap");
    }
    IEnumerator S()
    {
        T_Anim.SetTrigger("fadeOut");
        PlayerPrefs.DeleteAll();
        ActionDone = true;
        yield return new WaitForSeconds(0.25f);
        SceneManager.LoadSceneAsync("MainMap");
    }
}

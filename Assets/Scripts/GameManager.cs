using System.Collections;
using System.Linq.Expressions;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public AbilityManager am;
    public GameObject dash, slam, dj,ws;
    public GameObject Player;
    bool escaped;
    public Animator T_Anim;
    void Start()
    {
        Player.transform.position = new Vector2
            (
                PlayerPrefs.GetFloat("SpawnX",-35)
                ,PlayerPrefs.GetFloat("SpawnY",-18.5f)
            );
        if(PlayerPrefs.GetString("dash","false") != "false")
        {
            dash.SetActive(false);
            am.dash = true;
        }
        if (PlayerPrefs.GetString("doubleJump", "false") != "false")
        {
            dj.SetActive(false);
            am.doubleJump = true;
        }
        if (PlayerPrefs.GetString("slam", "false") != "false")
        {
            slam.SetActive(false);
            am.slam = true;
        }
        if (PlayerPrefs.GetString("wallSlide", "false") != "false")
        {
            ws.SetActive(false);
            am.wallSlide = true;
        }
    }
    IEnumerator GoMenu()
    {
        PlayerPrefs.Save();
        T_Anim.SetTrigger("fadeOut");
        Player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        yield return new WaitForSeconds(0.26f);
        SceneManager.LoadSceneAsync("MainMenu");
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !escaped)
        {
            escaped = true;
            StartCoroutine(GoMenu());
        }
    }
}

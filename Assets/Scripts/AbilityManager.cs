using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class AbilityManager : MonoBehaviour
{
    public bool dash, slam, wallSlide, doubleJump;
    public GameObject dashIcon, djIcon, wsIcon, slamIcon;
    public TMP_Text abilityGainText;
    public Animator abilityGainScreen;
    public TMP_Text abilityControlText;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator GetDash()
    {
        StartCoroutine(LerpTimeScale(0f, 0.5f));
        yield return new WaitForSecondsRealtime(0.7f);
        abilityGainText.text = "Dash";
        KeyCode button = GetComponent<PlayerMovement>().dashButton;
        dashIcon.SetActive(true);
        abilityControlText.text = button.ToString();
        abilityGainScreen.SetBool("show", true);
        yield return new WaitForSecondsRealtime(0.5f);
        while (!Input.anyKey)
        {
            yield return null;
        }
        abilityGainScreen.SetBool("show", false);
        dash = true;
        dashIcon.SetActive(false);
        StartCoroutine(LerpTimeScale(1f, 0.1f));

    }

    public IEnumerator GetDoubleJump()
    {
        StartCoroutine(LerpTimeScale(0f, 0.5f));
        djIcon.SetActive(true);
        yield return new WaitForSecondsRealtime(0.7f);
        abilityGainText.text = "Double Jump";
        KeyCode button = GetComponent<PlayerMovement>().Up;
        abilityControlText.text = button.ToString();
        abilityGainScreen.SetBool("show", true);
        yield return new WaitForSecondsRealtime(0.5f);
        while (!Input.anyKey)
        {
            yield return null;
        }
        djIcon.SetActive(false);
        abilityGainScreen.SetBool("show", false);
        doubleJump = true;
        StartCoroutine(LerpTimeScale(1f, 0.1f));

    }

    public IEnumerator GetWallSlide()
    {
        StartCoroutine(LerpTimeScale(0f, 0.5f));
        wsIcon.SetActive(true);
        yield return new WaitForSecondsRealtime(0.7f);
        abilityGainText.text = "Wall Slide";
        abilityControlText.text = "";
        abilityGainScreen.SetBool("show", true);
        yield return new WaitForSecondsRealtime(0.5f);
        while (!Input.anyKey)
        {
            yield return null;
        }
        wsIcon.SetActive(false);
        abilityGainScreen.SetBool("show", false);
        wallSlide = true;
        StartCoroutine(LerpTimeScale(1f, 0.1f));

    }
    public IEnumerator GetSlam()
    {
        StartCoroutine(LerpTimeScale(0f, 0.5f));
        slamIcon.SetActive(true);
        yield return new WaitForSecondsRealtime(0.7f);
        abilityGainText.text = "Down Slam";
        KeyCode button = GetComponent<PlayerMovement>().Down;
        abilityControlText.text = button.ToString();
        abilityGainScreen.SetBool("show", true);
        yield return new WaitForSecondsRealtime(0.5f);
        while (!Input.anyKey)
        {
            yield return null;
        }
        slamIcon.SetActive(false);
        abilityGainScreen.SetBool("show", false);
        slam = true;
        StartCoroutine(LerpTimeScale(1f, 0.1f));

    }

    // this lerp time without being affected by time! also why are you reading this??
    public IEnumerator LerpTimeScale(float target, float duration)
    {
        float start = Time.timeScale;
        float timer = 0f;
        while (timer < duration)
        {
            timer += Time.unscaledDeltaTime;
            float t = timer / duration;
            Time.timeScale = Mathf.Lerp(start, target, t);
            yield return null;
        }
        Time.timeScale = target;
    }

}

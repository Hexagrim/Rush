using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class AbilityManager : MonoBehaviour
{
    public bool dash, slam, wallSlide, doubleJump;

    public TMP_Text abilityGainText;
    public Animator abilityGainScreen;
    public TMP_Text abilityControlText;
    void Start()
    {
        StartCoroutine(GetDash());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator GetDash()
    {
        yield return new WaitForSecondsRealtime(5f);
        abilityGainText.text = "Dash";
        KeyCode button = GetComponent<PlayerMovement>().dashButton;
        abilityControlText.text = button.ToString();
        abilityGainScreen.SetBool("show", true);
        StartCoroutine(LerpTimeScale(0f, 0.25f));
        yield return new WaitForSecondsRealtime(2f);
        while (!Input.anyKey)
        {
            yield return null;
        }
        abilityGainScreen.SetBool("show", false);
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

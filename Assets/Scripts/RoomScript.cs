using System.Collections;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;


public class RoomScript : MonoBehaviour
{
    private GameObject Vcam;
    void Start()
    {
        Vcam = transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {

            
            Vcam.SetActive(true);

            Vcam.GetComponent<CinemachineBasicMultiChannelPerlin>().AmplitudeGain = 0f;

            Vcam.GetComponent<CinemachineBasicMultiChannelPerlin>().FrequencyGain = 0f;

            StartCoroutine(TimeStop());

        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Vcam.SetActive(false);
            Vcam.GetComponent<CinemachineBasicMultiChannelPerlin>().AmplitudeGain = 0f;

            Vcam.GetComponent<CinemachineBasicMultiChannelPerlin>().FrequencyGain = 0f;
        }
    }
    //time stop so we cant move when transitioning yeas!
    IEnumerator TimeStop()
    {
        yield return new WaitForSeconds(0.15f);
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(0.8f);
        Time.timeScale = 1f;
    }
}

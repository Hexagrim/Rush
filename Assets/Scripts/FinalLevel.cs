using System.Collections;
using UnityEngine;

public class FinalLevel : MonoBehaviour
{
    public Transform spawnPos;
    public Animator T_Anim;
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
            PlayerDeath pd = collision.GetComponent<PlayerDeath>();
            StartCoroutine(ChangePos(pd));

        }
    }
    IEnumerator ChangePos(PlayerDeath pd)
    {
        pd.SpawnPos = spawnPos;
        T_Anim.SetTrigger("fadeOut");
        pd.StartCoroutine(pd.Die());
        yield return new WaitForSeconds(1f);
        T_Anim.SetTrigger("fadeIn");

    }
}

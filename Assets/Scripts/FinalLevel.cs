using System.Collections;
using UnityEditor;
using UnityEngine;

public class FinalLevel : MonoBehaviour
{
    public Transform spawnPos;
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
        if (collision.gameObject.CompareTag("Player") && !done)
        {
            PlayerDeath pd = collision.GetComponent<PlayerDeath>();
            StartCoroutine(ChangePos(pd));
            done = true;
        }
    }
    IEnumerator ChangePos(PlayerDeath pd)
    {
        pd.SpawnPos = spawnPos;
        T_Anim.SetTrigger("fadeOut");
        pd.StartCoroutine(pd.Die());
        yield return new WaitForSeconds(2f);
        pd.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        T_Anim.SetTrigger("fadeIn");

    }
}

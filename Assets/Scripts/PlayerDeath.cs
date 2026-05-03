using System.Collections;
using System.Linq.Expressions;
using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
    bool isDead;
    private Animator Anim;
    private PlayerMovement mov;

    public Transform SpawnPos;
    AudioManager AudioM;
    public GameObject DieParticle;
    void Start()
    {
        Anim= GetComponent<Animator>();
        mov = GetComponent<PlayerMovement>();
        AudioM = FindFirstObjectByType<AudioManager>();
    }

    // Update is called once per frame
    void Update()
    {
        Anim.SetBool("isDead", isDead);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") && !isDead)
        {
            StartCoroutine(Die());
        }
    }
    public IEnumerator Die()
    {
        Anim.ResetControllerState(true);
        RigidbodyConstraints2D rbc = GetComponent<Rigidbody2D>().constraints;
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        mov.dashtrail.Stop();
        mov.slamtrail.Stop();
        AudioM.PlaySfx(AudioM.Die);
        mov.cancleAbility = true;
        isDead = true;
        Anim.SetTrigger("die");
        Instantiate(DieParticle , transform.position , Quaternion.identity);
        mov.enabled = false;
        FindFirstObjectByType<CamShake>().ShakeCam(10, 4, 0.15f);
        yield return new WaitForSeconds(1f);
        transform.position = SpawnPos.position;
        isDead = false;
        Anim.SetBool("isRunning", false);
        Anim.SetBool("isJumping", false);
        Anim.SetBool("isDashing", false);
        Anim.SetBool("isWallSliding", false);
        yield return new WaitForSeconds(0.2f);
        GetComponent<Rigidbody2D>().constraints = rbc;
        mov.cancleAbility = false;
        mov.enabled = true;


    }
}

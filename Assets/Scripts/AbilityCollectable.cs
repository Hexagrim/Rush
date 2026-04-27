using JetBrains.Annotations;
using System.Collections;
using UnityEngine;

public class AbilityCollectable : MonoBehaviour
{
    public bool dash, slam, wallSlide, doubleJump;
    public GameObject collectParticle;
    void Start()
    {
        
    }


    // Update is called once per frame
    void Update()
    { 

        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {

            AbilityManager am = collision.gameObject.GetComponent<AbilityManager>();
            if (dash)
            {
                am.StartCoroutine(am.GetDash());
            }
            else if (doubleJump)
            {
                am.StartCoroutine(am.GetDoubleJump());
            }
            else if (slam)
            {
                am.StartCoroutine(am.GetSlam());
            }
            else if (wallSlide)
            {
                am.StartCoroutine(am.GetWallSlide());
            }
            FindFirstObjectByType<CamShake>().ShakeCam(7, 2, 0.1f);
            Instantiate(collectParticle, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }
}

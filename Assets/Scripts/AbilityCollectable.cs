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
            if (dash)
            {
                AbilityManager am = collision.gameObject.GetComponent<AbilityManager>();
                am.StartCoroutine(am.GetDash());
                FindFirstObjectByType<CamShake>().ShakeCam(7, 2, 0.1f);
                Instantiate(collectParticle, transform.position, Quaternion.identity);
                Destroy(this.gameObject);
            }
        }
    }
}

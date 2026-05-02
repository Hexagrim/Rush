using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    private PlayerDeath pd;
    public GameObject SetSpawnParticle;
    public GameObject Overlay;
    void Start()
    {
        pd = FindFirstObjectByType<PlayerDeath>();
    }

    // Update is called once per frame
    void Update()
    {
        //Overlay.SetActive(pd.SpawnPos == transform);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") && pd.SpawnPos != transform)
        {
            pd.SpawnPos = transform;
            PlayerPrefs.SetFloat("SpawnX", transform.position.x);
            PlayerPrefs.SetFloat("SpawnY", transform.position.y);

            //FindFirstObjectByType<CamShake>().ShakeCam(5, 2, 0.2f);
            //Instantiate(SetSpawnParticle, transform.parent.position, Quaternion.identity);
        }

    }
}

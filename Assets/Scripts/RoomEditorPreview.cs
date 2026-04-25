using UnityEngine;

[ExecuteAlways]
public class RoomEditorPreview : MonoBehaviour
{
    private GameObject Vcam;

    private void OnEnable()
    {
        if (transform.childCount > 0)
            Vcam = transform.GetChild(0).gameObject;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (Application.isPlaying) return; // ignore in play mode

        if (collision.CompareTag("Player"))
        {
            if (Vcam != null)
                Vcam.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (Application.isPlaying) return; // ignore in play mode

        if (collision.CompareTag("Player"))
        {
            if (Vcam != null)
                Vcam.SetActive(false);
        }
    }
}

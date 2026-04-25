using UnityEngine;

[ExecuteAlways]
public class RoomEditorPreview : MonoBehaviour
{
    private GameObject vcam;
    private Collider2D roomCollider;
    private Transform player;

    void OnEnable()
    {
        if (transform.childCount > 0)
            vcam = transform.GetChild(0).gameObject;

        roomCollider = GetComponent<Collider2D>();
    }

    void Update()
    {
        if (Application.isPlaying) return;
        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null)
                player = p.transform;
        }
        if (player == null || roomCollider == null || vcam == null)
            return;
        bool inside = roomCollider.bounds.Contains(player.position);
        vcam.SetActive(inside);
    }
}

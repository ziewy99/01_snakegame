using UnityEngine;

public class TitleManager : MonoBehaviour
{
    void Start()
    {
        // Player��T��
        Player player = FindObjectOfType<Player>();
        if (player != null)
        {
            Destroy(player.gameObject);
        }
    }
}

using UnityEngine;

public class TitleManager : MonoBehaviour
{
    void Start()
    {
        // Player‚ð’T‚·
        Player player = FindObjectOfType<Player>();
        if (player != null)
        {
            Destroy(player.gameObject);
        }
    }
}

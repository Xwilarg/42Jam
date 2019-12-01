using UnityEngine;

public class DungeonHeart : MonoBehaviour
{
    private Character player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Character>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Hero"))
            player.LooseHp(9999);
    }
}

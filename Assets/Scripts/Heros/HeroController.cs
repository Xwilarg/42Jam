using UnityEngine;

public class HeroController : MonoBehaviour
{
    [SerializeField]
    private Transform player;

    [SerializeField]
    private HeroClass heroClass;

    private const int avoidPlayerLayer = ~(1 << 8 | 1 << 10);

    private void Update()
    {
        if (!Physics2D.Linecast(transform.position, player.position, avoidPlayerLayer)) // Can see player, battle mode
        {

        }
    }

    private enum HeroClass
    {
        Warrior,
        Healer,
        Rogue,
        Bowman,
        Mage
    }
}

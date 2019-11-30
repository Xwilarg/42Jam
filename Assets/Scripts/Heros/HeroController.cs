using UnityEngine;

public class HeroController : MonoBehaviour
{
    private Transform player;

    private HeroClass heroClass;

    private string heroName;
    private const int avoidPlayerLayer = ~(1 << 8 | 1 << 10);

    public void Init(string nameValue, HeroClass heroValue)
    {
        heroName = nameValue;
        heroClass = heroValue;
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        if (!Physics2D.Linecast(transform.position, player.position, avoidPlayerLayer)) // Can see player, battle mode
        {

        }
    }

    public enum HeroClass
    {
        Warrior,
        Healer,
        Rogue,
        Bowman,
        Mage
    }
}

using System.Linq;
using UnityEngine;

public class HeroController : MonoBehaviour
{
    [SerializeField]
    private TextMesh infos;
    private SpriteRenderer sr; // Move sr.transform instead of transform
    private Transform player;
    private HeroClass heroClass;
    private string heroName;
    private const int avoidPlayerLayer = ~(1 << 8 | 1 << 10);

    private Node objective; // Destination the heroes need to reach

    public void Init(string nameValue, HeroClass heroValue)
    {
        heroName = nameValue;
        heroClass = heroValue;        infos.text = "Name: " + heroName + "\nClass: " + heroClass;
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        objective = GetClosestNode(player.position);
        sr = GetComponentInChildren<SpriteRenderer>();
    }

    private void Update()
    {
        if (!Physics2D.Linecast(transform.position, player.position, avoidPlayerLayer)) // Can see player, battle mode
        {

        }
    }

    private void OnMouseEnter()
    {
        infos.gameObject.SetActive(true);
    }

    private void OnMouseExit()
    {
        infos.gameObject.SetActive(false);
    }

    private Node GetClosestNode(Vector2 pos)
    {
        float dist = float.MaxValue;
        GameObject closest = null;
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("Node"))
        {
            float currDist = Vector2.Distance(transform.position, pos);
            if (closest == null || currDist < dist)
            {
                closest = go;
                dist = currDist;
            }
        }
        return closest.GetComponent<Node>();
    }

    public enum HeroClass
    {
        Warrior,
        Healer,
        Rogue,
        Archer,
        Mage
    }
}

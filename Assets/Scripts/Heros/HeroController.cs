using UnityEngine;

public class HeroController : MonoBehaviour
{
    [SerializeField]
    private TextMesh infos;

    private SpriteRenderer sr; // Move sr.transform instead of transform
    private Rigidbody2D rb;
    private Transform player;
    private HeroClass heroClass;
    private string heroName;
    private const int avoidPlayerLayer = ~(1 << 8 | 1 << 10);
    private Node[] path;
    private Node objective; // Destination the heroes need to reach
    private int index;

    private const float minDistNode = .5f;
    private const float speed = 7f;

    public void Init(string nameValue, HeroClass heroValue, Node[] pathValue)
    {
        heroName = nameValue;
        heroClass = heroValue;
        path = pathValue;
        infos.text = "Name: " + heroName + "\nClass: " + heroClass;
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        objective = GetClosestNode(player.position);
        sr = GetComponentInChildren<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        index = 1;
    }

    private void FixedUpdate()
    {
        if (!Physics2D.Linecast(transform.position, player.position, avoidPlayerLayer)) // Can see player, battle mode
        {

        }
        else
        {
            int x = 0, y = 0;
            var node = path[index];

            if (transform.position.x + minDistNode < node.transform.position.x)
                x = 1;
            else if (transform.position.x - minDistNode > node.transform.position.x)
                x = -1;
            if (transform.position.y + minDistNode < node.transform.position.y)
                y = 1;
            else if (transform.position.y - minDistNode > node.transform.position.y)
                y = -1;

            if (x == 0 && y == 0)
            {
                if (index < path.Length)
                    index++;
                else
                    rb.velocity = Vector2.zero;
            }
            else
                rb.velocity = new Vector2(x, y) * speed;
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

    public static Node GetClosestNode(Vector2 pos)
    {
        float dist = float.MaxValue;
        GameObject closest = null;
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("Node"))
        {
            float currDist = Vector2.Distance(go.transform.position, pos);
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

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

    private bool enemyInRange;

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
        objective = GetClosestNode<Node>(player.position, "Node");
        sr = GetComponentInChildren<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        index = 1;
        enemyInRange = false;
    }

    private void FixedUpdate()
    {
        if (enemyInRange)
        {
            var closestEnnemy = GetClosestNode<Character>(transform.position, "Enemy");
            rb.velocity = Vector2.zero;
        }
        else if (!Physics2D.Linecast(transform.position, player.position, avoidPlayerLayer)) // Can see player, battle mode
        {

        }
        else
        {
            int x = 0, y = 0;
            var node = path[index];

            if (transform.position.x + minDistNode < node.transform.position.x)
            {
                x = 1;
                sr.transform.rotation = Quaternion.Euler(0f, 0f, 180f);
            }
            else if (transform.position.x - minDistNode > node.transform.position.x)
            {
                x = -1;
                sr.transform.rotation = Quaternion.identity;
            }
            if (transform.position.y + minDistNode < node.transform.position.y)
            {
                y = 1;
                sr.transform.rotation = Quaternion.Euler(0f, 0f, -90f);
            }
            else if (transform.position.y - minDistNode > node.transform.position.y)
            {
                y = -1;
                sr.transform.rotation = Quaternion.Euler(0f, 0f, 90f);
            }

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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        enemyInRange = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        enemyInRange = false;
    }

    private void OnMouseEnter()
    {
        infos.gameObject.SetActive(true);
    }

    private void OnMouseExit()
    {
        infos.gameObject.SetActive(false);
    }

    public static T GetClosestNode<T>(Vector2 pos, string tag)
    {
        float dist = float.MaxValue;
        GameObject closest = null;
        foreach (GameObject go in GameObject.FindGameObjectsWithTag(tag))
        {
            float currDist = Vector2.Distance(go.transform.position, pos);
            if (closest == null || currDist < dist)
            {
                closest = go;
                dist = currDist;
            }
        }
        return closest.GetComponent<T>();
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

using UnityEngine;

public class HeroController : MonoBehaviour
{
    [SerializeField]
    private TextMesh infos;

    private SpriteRenderer sr; // Move sr.transform instead of transform
    private Rigidbody2D rb;
    private Character charac;
    private Transform player;
    private HeroClass heroClass;
    private string heroName;
    private const int avoidPlayerLayer = ~(1 << 8 | 1 << 10);
    private Node[] path;
    private Node objective; // Destination the heroes need to reach
    private int index;

    private const float minDistNode = .5f;
    private const float speed = 7f;
    private const int avoidHeroLayer = ~(1 << 10);

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
        charac = GetComponent<Character>();
        index = 1;
        enemyInRange = false;
    }

    private void FixedUpdate()
    {
        if (enemyInRange)
        {
            var enPos = GetClosestNode<Character>(transform.position, "Enemy").transform;
            var finalPos = enPos.position - transform.position;
            if (Mathf.Abs(finalPos.x) > Mathf.Abs(finalPos.y))
            {
                if (finalPos.x > 0f)
                    sr.transform.rotation = Quaternion.Euler(0f, 0f, 180f);
                else if (finalPos.x < 0f)
                    sr.transform.rotation = Quaternion.identity;
            }
            else
            {
                if (finalPos.y > 0f)
                    sr.transform.rotation = Quaternion.Euler(0f, 0f, -90f);
                else if (finalPos.y < 0f)
                    sr.transform.rotation = Quaternion.Euler(0f, 0f, 90f);
            }
            rb.velocity = Vector2.zero;
            charac.SwordAttack(-rb.transform.right, avoidHeroLayer);
        }
        else if (!Physics2D.Linecast(transform.position, player.position, avoidPlayerLayer)) // Can see player, battle mode
        {

        }
        else
        {
            int x = 0, y = 0;
            var node = path[index];
            Vector2? dir = null;

            if (transform.position.x + minDistNode < node.transform.position.x)
            {
                x = 1;
                sr.transform.rotation = Quaternion.Euler(0f, 0f, 180f);
                dir = Vector2.right;
            }
            else if (transform.position.x - minDistNode > node.transform.position.x)
            {
                x = -1;
                sr.transform.rotation = Quaternion.identity;
                dir = Vector2.left;
            }
            if (transform.position.y + minDistNode < node.transform.position.y)
            {
                y = 1;
                sr.transform.rotation = Quaternion.Euler(0f, 0f, -90f);
                dir = Vector2.up;
            }
            else if (transform.position.y - minDistNode > node.transform.position.y)
            {
                y = -1;
                sr.transform.rotation = Quaternion.Euler(0f, 0f, 90f);
                dir = Vector2.down;
            }

            if (x == 0 && y == 0)
            {
                if (index < path.Length)
                    index++;
                else
                    rb.velocity = Vector2.zero;
            }
            else
            {
                if (dir == null || !Physics.Linecast((Vector2)transform.position + dir.Value, (Vector2)transform.position + dir.Value * 2))
                    rb.velocity = new Vector2(x, y) * speed;
                else
                    rb.velocity = Vector2.zero;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
            enemyInRange = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
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

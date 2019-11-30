using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Player Speed")]
    private float speed;

    [SerializeField]
    [Tooltip("Damage Display Prefab")]
    private GameObject damagePrefab;

    private Rigidbody2D rb;

    private const float swordRange = 1f;
    private const int avoidPlayerLayer = ~(1 << 8);
    private const int swordDamage = 10;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")) * speed;

        // Set player orientation
        if (Mathf.Abs(rb.velocity.x) > Mathf.Abs(rb.velocity.y))
        {
            if (rb.velocity.x > 0f)
                transform.rotation = Quaternion.Euler(0f, 0f, 180f);
            else if (rb.velocity.x < 0f)
                transform.rotation = Quaternion.identity;
        }
        else
        {
            if (rb.velocity.y > 0f)
                transform.rotation = Quaternion.Euler(0f, 0f, -90f);
            else if (rb.velocity.y < 0f)
                transform.rotation = Quaternion.Euler(0f, 0f, 90f);
        }

        // Attacks
        if (Input.GetKeyDown(KeyCode.Z)) // Sword attack
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.position - transform.right, swordRange, avoidPlayerLayer);
            if (hit.collider != null)
            {
                Character charac = hit.collider.GetComponent<Character>();
                charac.LooseHp(swordDamage);
                GameObject go = Instantiate(damagePrefab, hit.point, Quaternion.identity);
                go.GetComponent<TextMesh>().text = "-" + swordDamage;
                go.transform.position = new Vector3(transform.position.x, transform.position.y, -1f);
            }
        }
    }
}

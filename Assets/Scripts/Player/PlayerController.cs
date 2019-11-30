using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Player Speed")]
    private float speed;

    [SerializeField]
    private GameObject iceSpearPrefab;

    [SerializeField]
    private GameObject fireballPrefab;

    private Rigidbody2D rb;

    private const float swordRange = 1f;
    private const int avoidPlayerLayer = ~(1 << 8);
    private const int swordDamage = 10;
    private const float swordReloadRef = 1f;
    private const float iceReloadRef = 5f;
    private const float iceForce = 5f;
    private const int iceDamage = 5;
    private const float fireReloadRef = 10f;
    private const float fireForce = 5f;
    private const int fireDamage = 8;

    private float swordReloadTimer;
    private float iceReloadTimer;
    private float fireReloadTimer;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        swordReloadTimer = 0f;
        iceReloadTimer = 0f;
        fireReloadTimer = 0f;
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
    }

    private void Update()
    {
        // Attacks
        if (Input.GetKeyDown(KeyCode.Z) && swordReloadTimer < 0f) // Sword attack
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, -transform.right, swordRange, avoidPlayerLayer);
            hit.collider?.GetComponent<Character>()?.LooseHp(swordDamage);
            swordReloadTimer = swordReloadRef;
        }
        else if (Input.GetKeyDown(KeyCode.X) && iceReloadTimer < 0f) // Ice spear
        {
            GameObject go = Instantiate(iceSpearPrefab, transform.position, Quaternion.identity);
            go.transform.rotation = transform.rotation;
            go.GetComponent<Rigidbody2D>().AddForce(-transform.right * iceForce, ForceMode2D.Impulse);
            go.GetComponent<Bullet>().SetDamage(iceDamage);
            iceReloadTimer = iceReloadRef;
        }
        else if (Input.GetKeyDown(KeyCode.C) && fireReloadTimer < 0f) // Fireball
        {
            GameObject go = Instantiate(fireballPrefab, transform.position, Quaternion.identity);
            go.transform.rotation = transform.rotation;
            go.GetComponent<Rigidbody2D>().AddForce(-transform.right * fireForce, ForceMode2D.Impulse);
            go.GetComponent<Bullet>().SetDamage(iceDamage);
            fireReloadTimer = fireReloadRef;
        }

        // Reload time
        swordReloadTimer -= Time.deltaTime;
        iceReloadTimer -= Time.deltaTime;
        fireReloadTimer -= Time.deltaTime;
    }
}

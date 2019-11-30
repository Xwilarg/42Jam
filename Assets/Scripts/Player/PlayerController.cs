using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Player speed")]
    private float speed;

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")) * speed;
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
}

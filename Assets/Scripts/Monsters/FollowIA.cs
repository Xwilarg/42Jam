using UnityEngine;

public class FollowIA : MonoBehaviour
{
    [HideInInspector]
    public Spawner _spawnMother;
    public float triggerDistance = 5.0f;
    public float speed = 1.0f;
    private Rigidbody2D rb;
    private Collider2D collider2d;
    private GameObject target = null;
    private const float minDistanceToAI = 1f;
    private Character charac;
    private const int avoidMonsterLayer = ~(1 << 11);
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        collider2d = GetComponent<Collider2D>();
        charac = GetComponent<Character>();
    }

    void Update()
    {
        float minDistance = triggerDistance;
        GameObject[] heros = GameObject.FindGameObjectsWithTag("Hero");
        for (int i = 0; i < heros.Length; i++) {
            float distance = collider2d.Distance(heros[i].GetComponent<Collider2D>()).distance;
            if (distance <= minDistance) {
                minDistance = distance;
                target = heros[i];
            }
        }

        if (target != null && minDistance > minDistanceToAI) {
            Vector2 velocity = target.transform.position - transform.position;
            velocity.Normalize();
            velocity *= speed;
            rb.velocity = velocity;
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
        else
        {
            rb.velocity = Vector2.zero;
            charac.SwordAttack(-rb.transform.right, avoidMonsterLayer);
        }
    }

    void Die()
    {
        _spawnMother.MobDying(this.gameObject);
    }
}

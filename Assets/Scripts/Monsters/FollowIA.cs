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
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        collider2d = GetComponent<Collider2D>();
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
        }
        else
            rb.velocity = Vector2.zero;
    }

    void Die()
    {
        _spawnMother.MobDying(this.gameObject);
    }
}

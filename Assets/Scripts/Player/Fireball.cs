using UnityEngine;

public class Fireball : Bullet
{
    [SerializeField]
    private GameObject explosion;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        foreach (Collider2D collider in Physics2D.OverlapCircleAll(transform.position, 2f))
        {
            collider.GetComponent<Character>()?.LooseHp(damage);
        }
        Instantiate(explosion, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

}

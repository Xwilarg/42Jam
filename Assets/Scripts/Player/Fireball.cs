using UnityEngine;

public class Fireball : Bullet
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        foreach (Collider2D collider in Physics2D.OverlapCircleAll(transform.position, 2f))
        {
            collider.GetComponent<Character>()?.LooseHp(damage);
        }
        Destroy(gameObject);
    }

}

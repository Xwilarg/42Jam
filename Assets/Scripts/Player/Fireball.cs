using UnityEngine;

public class Fireball : Bullet
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        foreach (Collider2D collider in Physics2D.OverlapCircleAll(transform.position, 3f))
        {
            collider.GetComponent<Character>()?.LooseHp(damage);
        }
        Destroy(gameObject);
    }

}

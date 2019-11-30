using UnityEngine;

public class Bullet : MonoBehaviour
{
    protected int damage;

    public void SetDamage(int value)
        => damage = value;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        collision.collider.GetComponent<Character>()?.LooseHp(damage);
        Destroy(gameObject);
    }
}

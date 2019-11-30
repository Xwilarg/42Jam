using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField]
    private int hp;

    private int or;

    private void Start()
    {
        or = 0;
    }

    public void LooseHp(int value)
    {
        hp -= value;
        if (hp <= 0)
            Destroy(gameObject);
    }

    public int GetOr()
        => or;

    public int GainOr(int value)
        => or += value;
}

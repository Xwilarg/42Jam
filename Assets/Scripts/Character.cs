using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField]
    private int hp;

    public void LooseHp(int value)
        => hp -= value;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    private Character player;

    // Upgrade
    public GameObject panel;
    public int Cost;
    public int maxLevel;
    public int Level;
    public SpriteRenderer upgradeObj;
    public List<Sprite> upgradeSprite;

    // Effect
    public GameObject projectile;
    private bool _cooldown = false;
    private bool _isTarget = false;
    public float rate;
    public float power;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Character>();
    }

    private void FixedUpdate()
    {
        if (_isTarget)
            AwakeTrap();
    }

    void AwakeTrap()
    {
        if (!_cooldown)
        {
            Debug.Log("oui");
            GameObject _trap = Instantiate(projectile, transform.position, Quaternion.identity);
            _trap.GetComponent<Rigidbody2D>().AddForce(-transform.up * power);
            _cooldown = true;
            StartCoroutine(cooldown());
        }
    }

    IEnumerator cooldown()
    {
        yield return new WaitForSeconds(rate);
        _cooldown = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            _isTarget = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            _isTarget = false;
    }

    public void Upgrade()
    {
        if (player.GetOr() >= Cost)
        {
            Level += 1;
            upgradeObj.sprite = upgradeSprite[Level];
        }
        else
            Debug.Log("Not enough Gold");
    }

    public void Delete()
    {
        player.GainOr(Cost * Level);
        Destroy(this.gameObject);
    }

    private void OnMouseDown()
    {
        panel.SetActive(true);
    }
}

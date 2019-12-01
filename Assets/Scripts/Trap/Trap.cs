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
    public GameObject _parent;

    // Effect
    public GameObject projectile;
    private bool _cooldown = false;
    private bool _isTarget = false;
    public float rate;
    public float power;
    public int damage = 5;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Character>();
        panel = player.GetComponent<TrapShop>().UpgradeShop;
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
            Vector3 position = transform.position;

            GameObject _trap = Instantiate(projectile, new Vector3(transform.position.x, transform.position.y - 0.5f, 0), Quaternion.identity, gameObject.transform);
            _trap.GetComponent<Rigidbody2D>().AddForce(-transform.up * power);
            _trap.GetComponent<Projectile>().caller = gameObject;
            _trap.GetComponent<Projectile>().damage = damage;

            _trap = Instantiate(projectile, new Vector3(transform.position.x + 0.5f, transform.position.y, 0), Quaternion.Euler(0, 0, 90), gameObject.transform);
            _trap.GetComponent<Rigidbody2D>().AddForce(transform.right * power);
            _trap.GetComponent<Projectile>().caller = gameObject;
            _trap.GetComponent<Projectile>().damage = damage;

            _trap = Instantiate(projectile, new Vector3(transform.position.x, transform.position.y + 0.5f, 0), Quaternion.Euler(0, 0, 180), gameObject.transform);
            _trap.GetComponent<Rigidbody2D>().AddForce(transform.up * power);
            _trap.GetComponent<Projectile>().caller = gameObject;
            _trap.GetComponent<Projectile>().damage = damage;

            _trap = Instantiate(projectile, new Vector3(transform.position.x - 0.5f, transform.position.y, 0), Quaternion.Euler(0, 0, 270), gameObject.transform);
            _trap.GetComponent<Rigidbody2D>().AddForce(-transform.right * power);
            _trap.GetComponent<Projectile>().caller = gameObject;
            _trap.GetComponent<Projectile>().damage = damage;

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
        if (collision.tag == "Hero")
            _isTarget = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Hero")
            _isTarget = false;
    }


    public void Upgrade()
    {
        if (player.GetOr() >= Cost)
        {
            player.GainOr(-Cost * (Level + 1));
            Level += 1;
            damage = (int)(damage * 1.5f);
            player.GetComponent<Economy>().UpdateGold();
            GetComponent<SpriteRenderer>().sprite = upgradeSprite[Level];
        }
        else
            Debug.Log("Not enough Gold");
    }

    public void Delete()
    {
        player.GainOr(Cost * (Level + 1));
        player.GetComponent<Economy>().UpdateGold();
        player.GetComponent<TrapShop>().DeleteTrap(transform.parent.gameObject);
        Destroy(_parent);
    }

    private void OnMouseDown()
    {
        panel.SetActive(true);
        panel.GetComponentInChildren<UpgradeTrap>().trap = this.gameObject;
        panel.GetComponentInChildren<DeleteTrap>().trap = this.gameObject;
    }
}

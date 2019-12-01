using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Spawner : MonoBehaviour
{
    public GameObject gobelin;
    public float rate;
    public int maxLevel;
    public int Level;
    public List<Sprite> upgradeSprite;
    public int Cost;
    private Character player;
    public GameObject panel;
    private List<GameObject> _mobs;
    public int maxMob = 10;
    private bool _isCooldown = false;

    private void Start()
    {
        _mobs = new List<GameObject>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Character>();
        panel = player.GetComponent<TrapShop>().UpgradeShop;
    }

    private void Update()
    {
        if (!_isCooldown)
            Spawn();
    }

    IEnumerator cooldown()
    {
        yield return new WaitForSeconds(rate);
        _isCooldown = false;
    }

    void Spawn()
    {
        if (_mobs.Count < maxMob)
        {
            GameObject mob = Instantiate(gobelin, transform.position, Quaternion.identity, gameObject.transform);
            _mobs.Add(mob);
            mob.GetComponent<FollowIA>()._spawnMother = this;
            mob.GetComponent<Character>().SetOrigin(this.gameObject);
            _isCooldown = true;
            StartCoroutine(cooldown());
        }
    }

    public void Upgrade()
    {
        if (player.GetOr() >= Cost)
        {
            player.GainOr(-Cost);
            Level += 1;
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
        player.GetComponent<TrapShop>().DeleteTrap(this.gameObject);
        Destroy(this.gameObject);
    }

    private void OnMouseDown()
    {
        Debug.Log("Fuck");
        panel.SetActive(true);
        panel.GetComponentInChildren<UpgradeTrap>().trap = this.gameObject;
        panel.GetComponentInChildren<DeleteTrap>().trap = this.gameObject;
    }

    public void MobDying(GameObject mob)
    {
        _mobs.Remove(mob);
    }
}

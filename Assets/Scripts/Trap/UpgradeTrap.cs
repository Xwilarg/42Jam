using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeTrap : MonoBehaviour
{
    public GameObject trap;

    public void SetUpdateTrap()
    {
        if (trap.GetComponent<Spawner>() != null)
            trap.GetComponent<Spawner>().Upgrade();
        //else if (trap.GetComponent<Hole>() != null)
        //    trap.GetComponent<Hole>().Upgrade();
        if (trap.GetComponent<GoldPile>() != null)
            trap.GetComponent<GoldPile>().Upgrade();
        if (trap.GetComponent<Trap>() != null)
            trap.GetComponent<Trap>().Upgrade();
    }
}

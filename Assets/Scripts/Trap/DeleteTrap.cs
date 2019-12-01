using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteTrap : MonoBehaviour
{
    public GameObject trap;

    public void SetDeleteTrap()
    {
        if (trap.GetComponent<Spawner>() != null)
            trap.GetComponent<Spawner>().Delete();
        //else if (trap.GetComponent<Hole>() != null)
        //    trap.GetComponent<Hole>().Upgrade();
        if (trap.GetComponent<GoldPile>() != null)
            trap.GetComponent<GoldPile>().Delete();
        if (trap.GetComponent<Trap>() != null)
            trap.GetComponent<Trap>().Delete();
    }
}

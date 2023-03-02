using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public string[] weapons;    // for test
    public string[] armors;     // for test
    //private LinkedList<string> weapons = new LinkedList<string>();
    //private LinkedList<string> slots = new LinkedList<string>();
    public string CurrWeapon { get; private set; }
    public string CurrArmor { get; private set; }

    public void SetWeapon(int index)
    {
        CurrWeapon = weapons[index];
        weapons[index] = null;
    }

    public void SetArmor(int index)
    {
        CurrArmor = armors[index];
        armors[index] = null;
    }
}

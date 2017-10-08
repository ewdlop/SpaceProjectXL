using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour {

    public GameObject[] WeaponList = new GameObject[10];
    // DEBUG: For quick testing of weapons
    public bool[] debugUnlockList = new bool[10];

    public static List<GameObject> playerWeaponList = new List<GameObject>();
    public static WeaponManager instance;

    void Awake ()
    {    
        for (int i = 0; i < WeaponList.Length; ++i)
        {
            if (WeaponList[i] != null) 
                playerWeaponList.Add(WeaponList[i]);
        }
        // Set the first weapon to be unlocked (Typically the default red laser)
        debugUnlockList[0] = true;
    }

    void Update()
    {
        // Set unlock status of player's weapons
        for (int i = 0; i < WeaponList.Length; ++i)
        {
            if (WeaponList[i] != null)
            {
                //playerWeaponList.Add(WeaponList[i]);
                playerWeaponList[i].GetComponent<Weapon>().isUnlocked = debugUnlockList[i];
            }
        }
    }
}

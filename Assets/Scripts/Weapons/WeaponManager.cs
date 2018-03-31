using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour {
    // Not really needed anymore, 
    // Refactor to have list of main and subs for 
    // easy testing? 
    public GameObject[] WeaponList = new GameObject[10];
    // DEBUG: For quick testing of weapons
    public bool isDebugMode = false;
    public bool[] debugUnlockList = new bool[10];
    public static List<GameObject> playerWeaponList = new List<GameObject>();

    void Awake()
    {    
        for (int i = 0; i < WeaponList.Length; ++i)
        {
            if (WeaponList[i] != null)
            {
                playerWeaponList.Add(WeaponList[i]);
                // Lock all weapons
                playerWeaponList[i].GetComponent<Weapon>().isUnlocked = debugUnlockList[i];
            }
        }
    }

    void Update()
    {
        // Set unlock status of player's weapons
        for (int i = 0; i < WeaponList.Length; ++i)
        {
            if (WeaponList[i] != null && isDebugMode)
            {
                playerWeaponList[i].GetComponent<Weapon>().isUnlocked = debugUnlockList[i];
            }
        }
    }
}

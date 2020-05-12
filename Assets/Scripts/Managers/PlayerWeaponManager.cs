using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponManager : MonoBehaviour
{

    public PlayerController playerController;
    public GameObject[] PlayerMainWeaponsObject;
    public GameObject[] PlayerSupportWeaponsObject;

    void Awake()
    {
        playerController.mainWeaponObject = PlayerMainWeaponsObject[GameController.mainWeaponInt];
        playerController.supportWeaponObject = PlayerSupportWeaponsObject[GameController.supportWeaponInt];
    }
}

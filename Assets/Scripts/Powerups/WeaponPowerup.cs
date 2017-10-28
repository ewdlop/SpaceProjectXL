using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPowerup : Powerup {

    // Unlock a weapon in the object's weapon list
    public override void ActivateEffect(GameObject touchedObject)
    {
        /*
        List<GameObject> weaponList = 
            touchedObject.GetComponent<PlayerController>().getWeaponList();

        // TODO add duration instead?
        int weaponToUnlock = UnityEngine.Random.Range(1, weaponList.Count);
        touchedObject.GetComponent<PlayerController>().unlockWeapon(weaponToUnlock);

        SoundController.Play((int)SFX.PickupHealth);
        DestroyObject(this.gameObject);
        */
    }
    
}

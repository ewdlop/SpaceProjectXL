using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireRateBoostedPowerUp : Powerup {

    public override void ActivateEffect(GameObject touchedObject)
    {
        touchedObject.GetComponent<PlayerController>().EnableFireRateBoost(1);
        SoundController.Play((int)SFX.PickupHealth);
        DestroyObject(this.gameObject);
    }

}

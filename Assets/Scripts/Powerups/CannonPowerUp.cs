using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonPowerUp : Powerup {

    public override void ActivateEffect(GameObject touchedObject)
    {
        touchedObject.GetComponent<PlayerController>().EnableCannon(true);
        SoundController.Play((int)SFX.PickupHealth);
        DestroyObject(this.gameObject);
    }

}

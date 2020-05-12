using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldPowerUp : Powerup
{
    public override void ActivateEffect(GameObject touchedObject)
    {
        touchedObject.GetComponent<PlayerController>().EnableShield(true);
        SoundController.Play((int)SFX.PickupHealth);
        Destroy(this.gameObject);
    }
}

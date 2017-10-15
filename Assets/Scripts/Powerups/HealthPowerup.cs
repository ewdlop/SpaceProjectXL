using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPowerup : Powerup {

    public int healAmount = 50;

    public override void ActivateEffect(GameObject touchedObject)
    {
        // Heal player or other object (this requires that object has a Destructible component
        touchedObject.GetComponent<PlayerController>().AddHealth(healAmount);
        SoundController.Play((int)SFX.PickupHealth);
        DestroyObject(this.gameObject);
    }
}

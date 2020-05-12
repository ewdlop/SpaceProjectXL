using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UltimatePowerUp : Powerup {

    public override void ActivateEffect(GameObject touchedObject)
    {
        touchedObject.GetComponent<PlayerController>().FillUltimateProgressToFull();
        SoundController.Play((int)SFX.PickupHealth);
        Destroy(this.gameObject);
    }

}

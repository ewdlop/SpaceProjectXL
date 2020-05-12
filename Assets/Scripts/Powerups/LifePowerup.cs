using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifePowerup : Powerup {
  
    public override void ActivateEffect(GameObject player)
    {        
        // Add a life to the player
        player.GetComponent<PlayerController>().AddLife();
        SoundController.Play((int)SFX.PickupHealth);
        Destroy(this.gameObject);
    }
}

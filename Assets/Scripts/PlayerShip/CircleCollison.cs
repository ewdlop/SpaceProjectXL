using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleCollison : MonoBehaviour {

    public GameObject playerShip;
    private PlayerController playerController;

    void Start()
    {
        playerController = playerShip.GetComponent<PlayerController>();
    }


    // Ship physically collided with an enemy
    void OnTriggerEnter2D(Collider2D collidedTarget)
    {
        if (collidedTarget.gameObject.GetComponent<Enemy>() != null &&
            !playerController.isInvincible)
        {
            playerController.Hit();
        }

        // Ship collided with enemy projectile
        if (collidedTarget.tag == "EnemyProjectile")
        {

            if (collidedTarget.gameObject.GetComponent<Weapon>() != null)
             {
                Instantiate(collidedTarget.GetComponent<Weapon>().hiteffect,
                            new Vector3(collidedTarget.gameObject.transform.position.x, collidedTarget.gameObject.transform.position.y, -0.01f),
                            Quaternion.identity);
                Destroy(collidedTarget.gameObject);
                // Set ship to invincible if it's not current invincible
                if (!playerController.isInvincible)
                {
                    playerController.Hit();
                }
            }
        }
    }
}

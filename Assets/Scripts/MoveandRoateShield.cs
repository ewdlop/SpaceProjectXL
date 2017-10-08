using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveandRoateShield : MonoBehaviour {


    public GameObject playerSpaceShip;

	void Update () {
        if (!MenuManager.isPaused)
        {
            if (playerSpaceShip != null)
            {
                transform.position = playerSpaceShip.transform.position;
                Vector2 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                float facingAngle = Mathf.Atan2(mouseWorldPosition.y - transform.position.y, mouseWorldPosition.x - transform.position.x) * Mathf.Rad2Deg;
                transform.eulerAngles = new Vector3(0f, 0f, facingAngle - 90f);
            }
        }
        if (PowerUpManger.shieldOn)
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
	}
}

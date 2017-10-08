using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayerShip : MonoBehaviour {

    public GameObject playerSpaceShip;

    public float stageXSize;
    public float stageYSize;
    public float cameraXMax;
    public float cameraXMin;
    public float cameraYMax;
    public float cameraYMin;
    void Start()
    {
        cameraXMax = stageXSize/2 - GetComponent<Camera>().orthographicSize * GetComponent<Camera>().aspect;
        cameraXMin = GetComponent<Camera>().orthographicSize * GetComponent<Camera>().aspect - stageXSize / 2;
        cameraYMax = stageYSize / 2 - GetComponent<Camera>().orthographicSize;
        cameraYMin = GetComponent<Camera>().orthographicSize - stageYSize / 2; ;
    }

    void Update()
    {
        //z is fixed
        Vector3 playerSpaceShipPosition = new Vector3(playerSpaceShip.transform.position.x, playerSpaceShip.transform.position.y, transform.position.z);
        playerSpaceShipPosition.x= Mathf.Clamp(playerSpaceShipPosition.x, cameraXMin, cameraXMax);
        playerSpaceShipPosition.y = Mathf.Clamp(playerSpaceShipPosition.y, cameraYMin, cameraYMax);
        transform.position = playerSpaceShipPosition;
    }

}

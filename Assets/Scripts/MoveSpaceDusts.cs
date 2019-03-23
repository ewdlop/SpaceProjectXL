using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSpaceDusts : MonoBehaviour {

    public Transform playerShip;
    public Vector2 oldPos;
    public Vector2 currentPos;
    public float scale;
    public bool isMenu;
    private void Start()
    {
        if (isMenu)
            return;
        currentPos = playerShip.position;
    }

    void Update ()
    {
        if (isMenu)
            return;
        if(playerShip!=null)
        {
            oldPos = currentPos;
            currentPos = playerShip.position;
            Vector2 displacement = currentPos - oldPos;
            transform.position += -1f * scale * new Vector3(displacement.x, displacement.y,0f);
        }

    }
}

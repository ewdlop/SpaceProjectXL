using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trajectory : MonoBehaviour {

    public LineRenderer trajectoryLine;
    public GameObject playerSpaceShip;
    public int lineSegmentCount = 0;

    void Start () {
        trajectoryLine = GetComponent<LineRenderer>();
        trajectoryLine.widthCurve = new AnimationCurve(new Keyframe(0, 0.1f));
        trajectoryLine.SetPosition(0, new Vector3(playerSpaceShip.transform.position.x, playerSpaceShip.transform.position.y, 0));
        if (LaunchPlayerShip.isPlayerShipDebugMode==false)
        {
            InvokeRepeating("DrawTrajectory", 6f, 0.1f);//call DrawTajectory() every 0.1s after launch
        }
        else
        { 
            InvokeRepeating("DrawTrajectory", 0.1f, 0.1f);
        }
    }
	
	
	void DrawTrajectory () {
        if (LaunchPlayerShip.isPlayerShipLaunched)
        {
            lineSegmentCount++;
            
           if (lineSegmentCount <1000)
            {
                trajectoryLine.positionCount++;
                trajectoryLine.SetPosition(lineSegmentCount, new Vector3(playerSpaceShip.transform.position.x, playerSpaceShip.transform.position.y, 0));
               
            }
                
        }

    }
}

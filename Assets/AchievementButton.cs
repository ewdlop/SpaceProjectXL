using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementButton : MonoBehaviour {

    public GameObject achievementList;
    public Sprite normal;
    public Sprite highlight;

    void Start () {
		
	}
	

	void Update () {
		
	}

    public void Click()
    {
        achievementList.SetActive(!achievementList.activeSelf);
    }
}

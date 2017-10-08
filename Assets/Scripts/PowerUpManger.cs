using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class PowerUp
{
    public GameObject powerUp;
    public GameObject weaponPanel;
    public GameObject weaponPanelBar;
    public string powerUpName;
    public float healthRepair;
    public float duration;
    public float currentDuration;
    public float maxDuration;
    public int playerWeaponIndex;

    public PowerUp(GameObject powerUp, GameObject weaponPanel, GameObject weaponPanelBar, string powerUpName, float healthRepair, float duration, float currentDuration, float maxDuration, int playerWeaponIndex)
    {
        this.powerUp = powerUp;
        this.weaponPanel = weaponPanel;
        this.weaponPanelBar = weaponPanelBar;
        this.powerUpName = powerUpName;
        this.healthRepair = healthRepair;
        this.duration = duration;
        this.currentDuration = currentDuration;
        this.maxDuration = maxDuration;
        this.playerWeaponIndex = playerWeaponIndex;

    }
    public void IncreaseMaxDuration()
    {
        this.maxDuration += this.duration;
    }
    public void IncreaseDuration()
    {
        this.currentDuration = Mathf.Clamp(this.currentDuration + this.duration, 0, this.maxDuration);
    }

    public void DecreaseDuration()
    {
        this.currentDuration = Mathf.Clamp(this.currentDuration-Time.deltaTime, 0, this.maxDuration);
    }

    public void SetProgress()
    {
        this.weaponPanelBar.GetComponent<Slider>().value=this.currentDuration/this.maxDuration;
    }
}

public class PowerUpManger : MonoBehaviour {

    public static PowerUpManger instance;
    public static float weaponPanelOffset;
    List<PowerUp> powerUpList = new List<PowerUp>();
    public List<GameObject> powerUp;
    public List<GameObject> weaponPanel;
    public List<GameObject> weaponPanelBar;
    public static bool shieldOn;
    public GameObject playerShip;
    void Awake()
    {


        PowerUp healthRepair = new PowerUp(powerUp[0], weaponPanel[0], weaponPanelBar[0], "HealthRepairPowerUp",50f, 0f, 0f, 0f, 100);
        PowerUp shield = new PowerUp(powerUp[1], weaponPanel[1], weaponPanelBar[1], "ShieldPowerUp", 0f, 10f, 0f, 60f, 101);
        PowerUp blueLaser = new PowerUp(powerUp[2], weaponPanel[2], weaponPanelBar[2], "BlueLaserBulletPowerUp", 0f, 10f, 0f, 60f, 1);
        PowerUp sineMissiles = new PowerUp(powerUp[3], weaponPanel[3], weaponPanelBar[3], "SineMissilesPowerUp", 0f, 10f, 0f, 60f, 2);
        PowerUp circleMissiles = new PowerUp(powerUp[4], weaponPanel[4], weaponPanelBar[4], "CircleMissilesPowerUp", 0f, 10f, 0f, 60f, 3);
        PowerUp spreadLaser = new PowerUp(powerUp[5], weaponPanel[5], weaponPanelBar[5], "SpreadLaserPowerUp", 0f, 10f, 0f, 60f, 4);
        PowerUp redRocketMissiles = new PowerUp(powerUp[6], weaponPanel[6], weaponPanelBar[6], "RedRocketMissilesPowerUp", 0f, 10f, 0f, 60f, 5);
        PowerUp chasingMissiles = new PowerUp(powerUp[7], weaponPanel[7], weaponPanelBar[7], "ChasingMissilesPowerUp", 0f, 10f, 0f, 60f, 6);

        powerUpList.Add(healthRepair);
        powerUpList.Add(shield);
        powerUpList.Add(blueLaser);
        powerUpList.Add(sineMissiles);
        powerUpList.Add(circleMissiles);
        powerUpList.Add(spreadLaser);
        powerUpList.Add(redRocketMissiles);
        powerUpList.Add(chasingMissiles);


    }
    public List<PowerUp> GetPowerUpList()
    {
        return powerUpList;
    }


     void Update()
    {
        if (playerShip != null)
        {
            foreach (PowerUp powerUps in playerShip.GetComponent<PlayerShipActions>().powerUpList)//public list too avoid losing reference issue

            {
                {
                    if (powerUps.currentDuration > 0f)//gains duration after we eat the power up.
                    {

                        powerUps.DecreaseDuration();
                        powerUps.SetProgress();
                    }
                    else
                    {
                        //duration=0 second=no unlocked=hide the bar
                        if (powerUps.playerWeaponIndex == 100 || powerUps.playerWeaponIndex == 101)//health power up doesnt have panel and bar.
                        {
                            if (powerUps.playerWeaponIndex == 101)
                            {

                                powerUps.weaponPanelBar.SetActive(false);
                                shieldOn = false;//i added a bool check for the shield in the Update() of the shied(scene object)
                            }
                        }
                        else
                        {

                            powerUps.weaponPanelBar.SetActive(false);
                            WeaponManager.playerWeaponList[powerUps.playerWeaponIndex].isUnlocked = false;
                        }
                    }
                }
            }
        }
    }

}

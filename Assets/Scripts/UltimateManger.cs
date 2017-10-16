using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Ultimate
{
    public string name;
    public List<GameObject> summonPrefab;
    public GameObject ultimateSlider;
    public Sprite sprite;
    public bool drainingProgess;
    public float progress;
    public float chargingDuration;
    public float ultimateDuration;


    public Ultimate(string name, List<GameObject> summonPrefab, GameObject ultimateSlider,Sprite sprite,bool drainingProgess, float progress, float chargingDuration, float ultimateDuration)
    {
        this.name = name;
        this.summonPrefab = summonPrefab;
        this.ultimateSlider = ultimateSlider;
        this.sprite =sprite;
        this.drainingProgess = drainingProgess;
        this.progress = progress;
        this.chargingDuration = chargingDuration;
        this.ultimateDuration = ultimateDuration;
        FillProgress(progress);
    }
    public void FillProgress(float amount)
    {
        this.progress = Mathf.Clamp(this.progress+amount,0f,1f);
        this.ultimateSlider.GetComponent<Slider>().value = progress;
    }

    public float GetProgress()
    {
        return this.progress;
    }
    
}

public class UltimateManger : MonoBehaviour {

    public List<GameObject> summonPrefab;
    public GameObject ultimateSlider;
    public GameObject playerShip;
    public GameObject playerShipIcon;
    public Ultimate playerShipUltimate;
    public float time;

    void Awake () { 
        playerShipUltimate = new Ultimate("YouShallNotPass", summonPrefab, ultimateSlider, GameController.inGameSprite, false, 0f, 20f, 10f);

        playerShip.GetComponent<SpriteRenderer>().sprite = playerShipUltimate.sprite;
        foreach (PolygonCollider2D cols in playerShip.GetComponents<PolygonCollider2D>())//reset collider size/shape after we change sprite
        {
            Destroy(cols);

        }
        PolygonCollider2D pol = playerShip.AddComponent<PolygonCollider2D>();
        pol.isTrigger = true;
        playerShip.AddComponent<PolygonCollider2D>();
        playerShipIcon.GetComponent<Image>().sprite = playerShipUltimate.sprite;
    }
	

	void Update () {
        time+=Time.deltaTime;
        
        if (playerShipUltimate.GetProgress()>=1f)//ultimate is ready
        {
            if (Input.GetKeyDown("u"))//activate ultmiate
            {
                foreach (GameObject summon in playerShipUltimate.summonPrefab)//active the child ships, they have their own weaponlist from thew weaponmanger
                {
                    summon.SetActive(true);
                }
                playerShipUltimate.drainingProgess = true;
                playerShipUltimate.FillProgress(-1 * Time.deltaTime / playerShipUltimate.ultimateDuration);
            }
        }           
        else
        {

            if (playerShipUltimate.drainingProgess)//ultimate was activated
            {
                playerShipUltimate.FillProgress(-1 * Time.deltaTime / playerShipUltimate.ultimateDuration);
                if (playerShipUltimate.GetProgress() <= 0f)//ultimate ends
                {
                    playerShipUltimate.drainingProgess = false;
                    foreach (GameObject summon in playerShipUltimate.summonPrefab)
                    {
                        summon.SetActive(false);
                    }
                }
            }
            else
            {
                //charging
                playerShipUltimate.FillProgress(Time.deltaTime / playerShipUltimate.chargingDuration);
            }
        }        
    }
}

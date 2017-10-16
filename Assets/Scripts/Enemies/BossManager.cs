using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TheBoss {

    public GameObject theBoss;
    public List<GameObject> weaponParts;

    public GameObject GetBoss()
    {
        return theBoss;
    }
    public static int IncreaseBossIndex(int index,int size)
    {
        return Mathf.Clamp(index + 1, 0, size - 1);
    }
}//TheBossNah?



public class BossManager : MonoBehaviour {

    public GameObject spawnManager;
    public TheBoss boss;
    void Start () {

        //boss.theBoss = spawnManager.GetComponent<SpawnManager>().bossList[0].type;

        //oss.weaponParts = spawnManager.GetComponent<SpawnManager>().bossList[0].parts;
    }

    // Update is called once per frame
    void Update () {
        PolygonCollider2D col=null;
        PolygonCollider2D colwithIsTrigger = null;
        if (boss.theBoss != null)
        {
            PolygonCollider2D[] colliders = boss.theBoss.GetComponents<PolygonCollider2D>();
            foreach (PolygonCollider2D cols in colliders)
            {
                if (cols.isTrigger)
                    colwithIsTrigger = cols;
                else
                    col = cols;

            }
            if (boss.theBoss.GetComponent<Destructibles>().currentHealth > 0)
            {
                foreach (GameObject parts in boss.weaponParts)
                {
                    if (parts != null && parts.GetComponent<Destructibles>().currentHealth > 0)
                    {
                        col.enabled = false;
                        colwithIsTrigger.enabled = false;
                        boss.theBoss.GetComponent<Destructibles>().isItTakesDamage = false;
                        break;
                    }
                    col.enabled = true;
                    boss.theBoss.GetComponent<Destructibles>().isItTakesDamage = true;
                }

            }
        }

        

       
	}
}

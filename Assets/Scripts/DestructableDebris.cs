using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructableDebris : MonoBehaviour {

    public int maxHealth;
    private float health;

    private Renderer renderer;
    private Color originalColor; 

	// Use this for initialization
	void Start () {
        renderer = GetComponent<Renderer>();
        originalColor = renderer.material.color;
        health = maxHealth;
	}
	
	// Update is called once per frame
	void Update () {
        if (health <= 0)
        {
            Destroy(this.gameObject);

            /*
            float randomAngle = Random.Range(0f, 120f);
            float randomSpeed = Random.Range(1f, 5f);
            
            int numberOfSpawn = enemyTypeManger.GetComponent<EnemyTypeManger>().numberOfSpawn[index];
            for (int i = 0; i < numberOfSpawn; i++)
            {
                GameObject clone = Instantiate(enemyTypeManger.GetComponent<EnemyTypeManger>().spawnEnemyAfterDeath[index], 
                    new Vector3(transform.position.x + 1f * Mathf.Cos((360f / numberOfSpawn * i + randomAngle)), 
                                transform.position.y + 1f * Mathf.Sin((360f / numberOfSpawn * i + randomAngle)), 
                                transform.position.z), Quaternion.identity);
                clone.GetComponent<Rigidbody2D>().velocity = new Vector2(randomSpeed * Mathf.Cos((360f / numberOfSpawn * i + randomAngle) * Mathf.PI / 180), randomSpeed * Mathf.Sin((360f / numberOfSpawn * i + randomAngle) * Mathf.PI / 180));
            }
            */
        }
	}

    void OnTriggerStay2D(Collider2D other)
    {
        //GameController.playerScore += enemyTypeManger.GetComponent<EnemyTypeManger>().scoreFromHitting[index];
        if (other.tag == "Projectile")
        {
            Destroy(other.gameObject);
            Instantiate(other.gameObject.GetComponent<ProjectileController>().hiteffect,
                        new Vector3(other.gameObject.transform.position.x, other.gameObject.transform.position.y, -0.01f),
                        Quaternion.identity);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Projectile")
        {
            Destroy(other.gameObject);
            StartCoroutine("HitFlash");
            health--;
        }
    }

    IEnumerator HitFlash()
    {
        this.renderer.material.color = GameController.instance.hitColor;
        yield return new WaitForSeconds(0.05f);
        this.renderer.material.color = originalColor;
    }
}

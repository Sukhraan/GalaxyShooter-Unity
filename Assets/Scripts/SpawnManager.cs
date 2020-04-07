using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject _enemyContainer;
    [SerializeField]
    private GameObject[] powerups;

    private bool _stopSpawning = false;

    

    public void StartSpawning()
	{
        //two ways to start coroutine
        //StartCoroutine("SpawnRoutine");
        StartCoroutine(SpawnEnemyRoutine());//more optimized
        StartCoroutine(SpawnPowerupRoutine());
    }

  

    //spawn game objects every 5 seconds
    //create a coroutine of type IEnumerator -- Yield Events
    //while loop

    IEnumerator SpawnEnemyRoutine()
	{

        yield return new WaitForSeconds(3.0f);
        //yield return null; //waits 1 frame

        ////then this line is called

        //yield return new WaitForSeconds(5.0f); // wait 5 seconds

        ////then this line is called

        while (_stopSpawning == false)
		{
            //Instantiate enemy prefab
            //yield wait for 5 seconds
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
            GameObject newEnemy = Instantiate(_enemyPrefab,posToSpawn,Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform; // assigning parent of enemy  as enemy container
            yield return new WaitForSeconds(5.0f);

        }

        //WE NEVER GET HERE


        //while loop (infine loop) //infine loops used in coroutines only because we can yield events in coroutines
        
	}

    IEnumerator SpawnPowerupRoutine()
	{
        yield return new WaitForSeconds(3.0f);

        //every 3-7 seconds, spwan in a powerup
        while (_stopSpawning == false)
		{
            Vector3 postToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
            int randomPowerUp = Random.Range(0, 3);
            Instantiate(powerups[randomPowerUp], postToSpawn, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(3, 8));
        }
	}

    public void OnPlayerDeath()
	{
        _stopSpawning = true;
	}

}

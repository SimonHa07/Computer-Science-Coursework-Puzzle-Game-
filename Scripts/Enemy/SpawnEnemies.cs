using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemies : MonoBehaviour
{
    public GameObject enemy_prefab;
    public bool coroutine_in_progress = false;
    public IEnumerator Spawn_Enemies(float SpawnTime, int enemy_num, Transform[] locations)
    {
        yield return new WaitForSecondsRealtime(SpawnTime);
        coroutine_in_progress = false;
        SpawnEnemy(enemy_num, locations);
    }
    void SpawnEnemy(int enemy_num, Transform[]locations)
    {
        for (int i = 0; i < enemy_num; i++)
        {
            //pick random number to choose between one location or the other
            int choice = Random.Range(0, locations.Length);
            Debug.Log(locations);
            Debug.Log(choice);
            Vector2 position = new Vector2(locations[choice].position.x, locations[choice].position.y);
            Instantiate(enemy_prefab, position, transform.rotation);
        }
    }
}

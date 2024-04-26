using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class EnemySpawner : MonoBehaviour
{
    public Enemy zomBearPrefab;
    // public Enemy zomBunnyPrefab;
    // public Enemy hellephantPrefab;
    private IObjectPool<Enemy> poolEnemy;

    public List<Transform> spawnPoints;

    public float interval = 1f;
    private float nextCreateTime;

    private void Start()
    {
        nextCreateTime = Time.time + interval;

        poolEnemy = new ObjectPool<Enemy>(
            CreatePooledItem,
            OnTakeFromPool,
            OnReturnToPool,
            OnDestroyPoolObject,
            true, 10, 1000
        );
    }

    void Update()
    {
        if (Time.time > nextCreateTime)
        {
            CreateEnemy();
            nextCreateTime = Time.time + interval;
        }
    }

    private void CreateEnemy()
    {
        var newEnemy = poolEnemy.Get();

        var spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];
        newEnemy.transform.position = spawnPoint.position;
    }

    private Enemy CreatePooledItem()
    {
        var enemy = Instantiate(zomBearPrefab);
        enemy.pool = poolEnemy;
        return enemy;
    }

    private void OnTakeFromPool(Enemy enemy)
    {
        enemy.gameObject.SetActive(true);
    }

    private void OnReturnToPool(Enemy enemy)
    {
        enemy.gameObject.SetActive(false);
    }

    private void OnDestroyPoolObject(Enemy enemy)
    {
        Destroy(enemy);
    }
}

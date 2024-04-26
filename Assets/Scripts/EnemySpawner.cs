using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Il2Cpp;
using UnityEngine;
using UnityEngine.Pool;

public class EnemySpawner : MonoBehaviour
{
    public enum EnemyType
    {
        ZomBear,
        ZomBunny,
        Hellephant,
        Count,
    }
    float[] weights = { 4.5f, 4.5f, 1f };
    public Enemy[] enemyPrefabs;
    private Dictionary<EnemyType, IObjectPool<Enemy>> poolEnemies = new Dictionary<EnemyType, IObjectPool<Enemy>>();

    public List<Transform> spawnPoints;

    public float interval;
    private float nextCreateTime;

    private void Start()
    {
        nextCreateTime = Time.time + interval;

        for (int i = 0; i < (int)EnemyType.Count; ++i)
        {
            int index = i;

            IObjectPool<Enemy> poolEnemy = new ObjectPool<Enemy>(
            () => {
                var enemy = Instantiate(enemyPrefabs[index]);
                enemy.pool = poolEnemies[(EnemyType)index];
                return enemy;
            },
            OnTakeFromPool,
            OnReturnToPool,
            OnDestroyPoolObject,
            true, 10, 100);

            poolEnemies.Add((EnemyType)i, poolEnemy);
        }
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
        var newEnemyType = (EnemyType)WeightedRandomPick();
        var newEnemy = poolEnemies[newEnemyType].Get();

        var spawnPoint = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Count)];
        newEnemy.transform.position = spawnPoint.position;
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

    private int WeightedRandomPick()
    {
        float totalWeight = 0f;
        foreach (var weight in weights)
        {
            totalWeight += weight;
        }

        var pick = UnityEngine.Random.Range(0.0f, 1.0f);
        int index = 0;
        for (int j = 0; j < weights.Length; j++)
        {
            if (weights[j] / totalWeight >= pick)
            {
                index = j;
                break;
            }
            pick -= weights[j] / totalWeight;
        }

        return index;
    }
}

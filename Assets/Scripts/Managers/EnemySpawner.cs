using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Enemy _enemyPrefab;
    [SerializeField] private Transform[] _spawnPoints;
    [SerializeField] private float _spawnTime = 3f;
    [SerializeField] private EnemySpawnerManager _enemySpawnerManager;
    private Queue<Enemy> enemyPool = new Queue<Enemy>();

    void Start ()
    {
        FillPool(10);
        InvokeRepeating ("Spawn", _spawnTime, _spawnTime);
    }

    private void FillPool(int instanceToCreateCount)
    {
        for (int i = 0; i < instanceToCreateCount; i++)
        {
            Enemy newEnemy = Instantiate(_enemyPrefab);
            newEnemy.gameObject.SetActive(false);
            enemyPool.Enqueue(newEnemy);
        }
    }

    private Enemy GetEnemy()
    {
        if (enemyPool.Count == 0)
        {
            FillPool(1);
        }
 
        return enemyPool.Dequeue();
    }
 
    private void ReturnToPool(Enemy enemyToReturn)
    {
        enemyToReturn.gameObject.SetActive(false);
        enemyPool.Enqueue(enemyToReturn);
    }

    private void Spawn ()
    {
        if(_enemySpawnerManager.PlayerHealth.currentHealth <= 0f)
        {
            return;
        }

        int spawnPointIndex = Random.Range (0, _spawnPoints.Length);

        Enemy newEnemy = GetEnemy();
        newEnemy.transform.position = _spawnPoints[spawnPointIndex].position;
        newEnemy.transform.rotation = _spawnPoints[spawnPointIndex].rotation;
        newEnemy.Initialize(_enemySpawnerManager.PlayerTransform, _enemySpawnerManager.PlayerHealth);
        newEnemy.OnRemoved += OnEnemyRemovedHandler;

        newEnemy.gameObject.SetActive(true);
        
        _enemySpawnerManager.AddNewEnemy(newEnemy.gameObject);
    }

    private void OnEnemyRemovedHandler(Enemy removeEnemy)
    {
        removeEnemy.OnRemoved -= OnEnemyRemovedHandler;
        ReturnToPool(removeEnemy);
    }
}

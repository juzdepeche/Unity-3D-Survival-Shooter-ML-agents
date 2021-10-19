using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Enemy _enemyPrefab;
    [SerializeField] private Transform[] _spawnPoints;
    [SerializeField] private float _spawnTime = 3f;
    [SerializeField] private EnemySpawnerManager _enemySpawnerManager;

    void Start ()
    {
        InvokeRepeating ("Spawn", _spawnTime, _spawnTime);
    }

    void Spawn ()
    {
        if(_enemySpawnerManager.PlayerHealth.currentHealth <= 0f)
        {
            return;
        }

        int spawnPointIndex = Random.Range (0, _spawnPoints.Length);

        Enemy newEnemy = Instantiate (_enemyPrefab, _spawnPoints[spawnPointIndex].position, _spawnPoints[spawnPointIndex].localRotation);
        newEnemy.Initialize(_enemySpawnerManager.PlayerTransform, _enemySpawnerManager.PlayerHealth);
        _enemySpawnerManager.AddNewEnemy(newEnemy.gameObject);
    }
}

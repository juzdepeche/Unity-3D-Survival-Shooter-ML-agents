using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerManager : MonoBehaviour
{
    public int EnemyCount => enemies.Count;
    [SerializeField] private Transform _player;
    [SerializeField] private PlayerAgent _playerAgent;
    private List<GameObject> enemies = new List<GameObject>();

    public void AddNewEnemy(GameObject enemy)
    {
        enemies.Add(enemy);
        enemy.GetComponent<EnemyHealth>().OnDeath += OnEnemyDeathHandler;
        enemy.GetComponent<EnemyHealth>().OnHit += OnEnemyHitHandler;
    }

    public Vector3 GetClosestEnemyPositionFromPlayer()
    {
        if (enemies.Count == 0)
            return Vector3.zero;

        SortEnemiesByDistance();
        return enemies[0].transform.position;
    }

    private void OnEnemyHitHandler()
    {
        if (_playerAgent != null)
        {
            _playerAgent.AddReward(0.1f);
        }
    }

    private void OnEnemyDeathHandler(EnemyHealth enemy)
    {
        if (_playerAgent != null)
        {
            _playerAgent.AddReward(1f);
        }

        enemy.OnDeath -= OnEnemyDeathHandler;
        enemy.OnHit -= OnEnemyHitHandler;
        enemies.Remove(enemy.gameObject);
    }

    private void SortEnemiesByDistance()
    {
        enemies.Sort(delegate(GameObject a, GameObject b)
        {
            return Vector3.Distance(_player.transform.position, a.transform.position)
            .CompareTo(
                Vector3.Distance(_player.transform.position, b.transform.position) );
        });
    }
}

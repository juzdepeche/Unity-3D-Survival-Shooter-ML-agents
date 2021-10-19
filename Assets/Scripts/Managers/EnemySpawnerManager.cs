using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerManager : MonoBehaviour
{
    public int EnemyCount => _enemies.Count;
    public Transform PlayerTransform => _playerTransform;
    [SerializeField] private Transform _playerTransform;
    public PlayerHealth PlayerHealth => _playerHealth;
    [SerializeField] private PlayerHealth _playerHealth;
    [SerializeField] private PlayerAgent _playerAgent;
    private List<GameObject> _enemies = new List<GameObject>();

    public void ResetEnemies()
    {
        foreach (GameObject enemy in _enemies)
        {
            Destroy(enemy);
        }
        _enemies.Clear();
    }

    public void AddNewEnemy(GameObject enemy)
    {
        _enemies.Add(enemy.gameObject);
        enemy.GetComponent<EnemyHealth>().OnDeath += OnEnemyDeathHandler;
        enemy.GetComponent<EnemyHealth>().OnHit += OnEnemyHitHandler;
    }

    public Vector3 GetClosestEnemyPositionFromPlayer()
    {
        if (_enemies.Count == 0)
            return Vector3.zero;

        SortEnemiesByDistance();
        return _enemies[0].transform.localPosition;
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
        _enemies.Remove(enemy.gameObject);
    }

    private void SortEnemiesByDistance()
    {
        _enemies.Sort(delegate(GameObject a, GameObject b)
        {
            return Vector3.Distance(_playerTransform.transform.localPosition, a.transform.localPosition)
            .CompareTo(
                Vector3.Distance(_playerTransform.transform.localPosition, b.transform.localPosition));
        });
    }
}

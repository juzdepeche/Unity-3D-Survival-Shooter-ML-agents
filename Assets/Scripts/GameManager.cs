using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private PlayerAgent _playerAgent;
    [SerializeField] private EnemySpawnerManager _enemySpawnerManager;

    private void Start()
    {
        _playerAgent.OnEpisodeBeginEvent += OnEpisodeBeginEventHandler;
    }

    private void OnEpisodeBeginEventHandler()
    {
        _playerAgent.ResetPlayer();
        _enemySpawnerManager.ResetEnemies();
    }
}

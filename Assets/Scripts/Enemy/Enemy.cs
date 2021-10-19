using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public event Action<Enemy> OnRemoved;
    [SerializeField] private EnemyAttack _enemyAttack;
    [SerializeField] private EnemyMovement _enemyMovement; 
    [SerializeField] private EnemyHealth _enemyHealth; 

    public void Initialize(Transform playerTransform, PlayerHealth playerHealth)
    {
        _enemyAttack.Initialize(playerTransform, playerHealth);
        _enemyMovement.Initialize(playerTransform, playerHealth);
        _enemyHealth.OnRemoved += OnRemovedHandler;
    }

    private void OnRemovedHandler()
    {
        _enemyHealth.OnRemoved -= OnRemovedHandler;
        OnRemoved?.Invoke(this);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public event Action<Enemy> OnRemoved;
    [SerializeField] private EnemyAttack _enemyAttack;
    [SerializeField] private EnemyMovement _enemyMovement; 
    [SerializeField] private EnemyHealth _enemyHealth; 
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private NavMeshAgent _navMeshAgent;

    public void Initialize(Transform playerTransform, PlayerHealth playerHealth)
    {
        _enemyAttack.Initialize(playerTransform, playerHealth);
        _enemyMovement.Initialize(playerTransform, playerHealth);
        _enemyHealth.OnRemoved += OnRemovedHandler;

        _navMeshAgent.enabled = true;
        _rigidbody.isKinematic = false;
    }

    private void OnRemovedHandler()
    {
        _enemyHealth.OnRemoved -= OnRemovedHandler;

        _navMeshAgent.enabled = false;
        _rigidbody.isKinematic = true;

        OnRemoved?.Invoke(this);
    }
}

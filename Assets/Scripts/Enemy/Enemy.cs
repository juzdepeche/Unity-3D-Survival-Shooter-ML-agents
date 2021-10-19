using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private EnemyAttack _enemyAttack;
    [SerializeField] private EnemyMovement _enemyMovement; 

    public void Initialize(Transform playerTransform, PlayerHealth playerHealth)
    {
        _enemyAttack.Initialize(playerTransform, playerHealth);
        _enemyMovement.Initialize(playerTransform, playerHealth);
    }
}

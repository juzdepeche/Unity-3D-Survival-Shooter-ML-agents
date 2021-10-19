using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private EnemyHealth _enemyHealth;
    [SerializeField] private NavMeshAgent _nav;
    private Transform _playerTransform;
    private PlayerHealth _playerHealth;

    public void Initialize(Transform playerTransform, PlayerHealth playerHealth)
    {
        _playerTransform = playerTransform;
        _playerHealth = playerHealth;
    }

    private void Update ()
    {
        if(_enemyHealth.currentHealth > 0 && _playerHealth.currentHealth > 0)
        {
            _nav.SetDestination (_playerTransform.position);
        }
        else
        {
            _nav.enabled = false;
        }
    }
}

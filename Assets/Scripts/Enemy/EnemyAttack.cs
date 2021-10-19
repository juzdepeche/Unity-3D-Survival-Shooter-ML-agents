using UnityEngine;
using System.Collections;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private EnemyHealth enemyHealth;
    [SerializeField] private float timeBetweenAttacks = 0.5f;
    [SerializeField] private int attackDamage = 10;

    private PlayerHealth _playerHealth;
    private Transform _playerTransform;
    private bool _playerInRange;
    private float _timer;

    public void Initialize(Transform playerTransform, PlayerHealth playerHealth)
    {
        _playerTransform = playerTransform;
        _playerHealth = playerHealth;
    }

    void OnTriggerEnter (Collider other)
    {
        if(other.transform == _playerTransform)
        {
            _playerInRange = true;
        }
    }


    void OnTriggerExit (Collider other)
    {
        if(other.transform == _playerTransform)
        {
            _playerInRange = false;
        }
    }


    void Update ()
    {
        _timer += Time.deltaTime;

        if(_timer >= timeBetweenAttacks && _playerInRange && enemyHealth.currentHealth > 0)
        {
            Attack ();
        }

        if(_playerHealth.currentHealth <= 0)
        {
            anim.SetTrigger ("PlayerDead");
        }
    }


    void Attack ()
    {
        _timer = 0f;

        if(_playerHealth.currentHealth > 0)
        {
            _playerHealth.TakeDamage (attackDamage);
        }
    }
}

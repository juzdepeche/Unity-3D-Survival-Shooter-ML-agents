using System;
using System.Collections;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public event Action<EnemyHealth> OnDeath;
    public event Action OnHit;
    public event Action OnRemoved;

    public int startingHealth = 100;
    public int currentHealth;
    public float sinkSpeed = 2.5f;
    public int scoreValue = 10;
    public AudioClip deathClip;


    [SerializeField] private Animator _anim;
    [SerializeField] private AudioSource _enemyAudio;
    [SerializeField] private ParticleSystem _hitParticles;
    [SerializeField] private CapsuleCollider _capsuleCollider;

    private bool _isDead;
    private bool _isSinking;

    private void OnEnable()
    {
        currentHealth = startingHealth;
        _capsuleCollider.isTrigger = false;
        _isDead = false;
        _isSinking = false;
    }

    void Update ()
    {
        if(_isSinking)
        {
            transform.Translate (-Vector3.up * sinkSpeed * Time.deltaTime);
        }
    }

    public void TakeDamage (int amount, Vector3 hitPoint)
    {
        if(_isDead)
            return;

        OnHit?.Invoke();
        _enemyAudio.Play();

        currentHealth -= amount;
            
        _hitParticles.transform.position = hitPoint;
        _hitParticles.Play();

        if(currentHealth <= 0)
        {
            Death ();
        }
    }


    void Death ()
    {
        OnDeath?.Invoke(this);

        _isDead = true;

        _capsuleCollider.isTrigger = true;

        _anim.SetTrigger ("Dead");

        _enemyAudio.clip = deathClip;
        _enemyAudio.Play ();
    }


    public void StartSinking ()
    {
        _isSinking = true;
        ScoreManager.score += scoreValue;
        StartCoroutine(Remove());
    }

    private IEnumerator Remove()
    {
        yield return new WaitForSeconds(2f);

        OnRemoved?.Invoke();
    }
}

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;


public class PlayerHealth : MonoBehaviour
{
    public int startingHealth = 100;
    public int currentHealth;
    public Slider healthSlider;
    public Image damageImage;
    public AudioClip deathClip;
    public float flashSpeed = 5f;
    public Color flashColour = new Color(1f, 0f, 0f, 0.1f);

    [SerializeField] private PlayerAgent _playerAgent;

    Animator anim;
    AudioSource playerAudio;
    PlayerMovement playerMovement;
    PlayerShooting playerShooting;
    bool isDead;
    bool damaged;


    void Awake ()
    {
        anim = GetComponent <Animator> ();
        playerAudio = GetComponent <AudioSource> ();
        playerMovement = GetComponent <PlayerMovement> ();
        playerShooting = GetComponentInChildren <PlayerShooting> ();

        ResetHealth();
    }


    void Update ()
    {
        if (_playerAgent == null)
        {
            if(damaged)
            {
                damageImage.color = flashColour;
            }
            else
            {
                damageImage.color = Color.Lerp (damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
            }
        }
        damaged = false;
    }

    public void ResetHealth()
    {
        currentHealth = startingHealth;

        damaged = false;
        isDead = false;

        playerMovement.enabled = true;
        playerShooting.enabled = true;
    }

    public void TakeDamage (int amount)
    {
        damaged = true;
        currentHealth -= amount;

        if (_playerAgent != null)
        {
            _playerAgent.AddReward(-1f);
        }
        else
        {
            healthSlider.value = currentHealth;
            playerAudio.Play ();
        }

        if(currentHealth <= 0 && !isDead)
        {
            Death ();
        }
    }


    void Death()
    {
        isDead = true;

        playerAudio.clip = deathClip;
        playerAudio.Play ();

        playerMovement.enabled = false;
        playerShooting.enabled = false;

        if (_playerAgent != null)
        {
            _playerAgent.AddReward(-5f);
            _playerAgent.EndEpisode();
        }
        else
        {
            playerShooting.DisableEffects ();
            anim.SetTrigger ("Die");
        }
    }

    public void RestartLevel()
    {
        if (_playerAgent == null)
        {
            SceneManager.LoadScene (0);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "DeathWall")
        {
            Death();
        }
    }
}

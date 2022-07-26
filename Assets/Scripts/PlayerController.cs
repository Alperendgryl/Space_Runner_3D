using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private GameManager gameManager;

    [SerializeField] private Rigidbody rb;

    [SerializeField] private Animator animator;

    [Header("Particle")]
    [SerializeField] private ParticleSystem mainEngineParticles;
    public ParticleSystem explosionParticles;

    [Header("ContinueTheGame")]
    [SerializeField] private float invincibleTime;
    [SerializeField] private float invincibleTimer;

    private Vector3 startPos;
    private Quaternion startRotation;

    [SerializeField] private float forceMultiplier;

    [Header("Fuel")]
    [SerializeField] private Slider fuelSlider;

    [SerializeField] private float fuel = 100f;
    [SerializeField] private TMP_Text fuelTxt;
    
    private float currentFuel;
    [SerializeField] private TMP_Text currentFuelTXT;

    [SerializeField] private float fuelConsumer;
    [SerializeField] private float fuelMultiplier;
    private bool enoughFuel = true;

    private void Start()
    {
        startPos = transform.position; //keep the inital pos of player
        startRotation = transform.rotation; //keep the initial rotation of player
        currentFuel = fuel;
    }

    void Update()
    {
        ApplyThrust();
        fuelController();

        if (invincibleTimer > 0)
        {
            invincibleTimer -= Time.deltaTime;
        }
    }

    private void fuelController()
    {
        currentFuelTXT.text = ((int)currentFuel).ToString();
        fuelSlider.value = currentFuel / fuel;
        
        if(currentFuel > 40)
        {
            fuelTxt.text = "";
        }
        if (currentFuel > 0f && currentFuel <= 40f)
        {
            fuelTxt.text = "Low Fuel !";
        }
        if(currentFuel <= 0f)
        {
            fuelTxt.text = "Not Enough Fuel !";
            enoughFuel = false;
        }
    }

    private void ApplyThrust()
    {
        if (gameManager.canMove)
        {
            if (Input.GetMouseButton(0) && enoughFuel)
            {
                animator.SetBool("onGround", false);

                rb.AddRelativeForce(Vector3.up * forceMultiplier);
                currentFuel -= fuelConsumer * Time.deltaTime;

                if (!audioManager.rocket.isPlaying)
                {
                    mainEngineParticles.Play();
                    audioManager.rocket.Play();
                }
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Obstacles" || other.tag == "Meteor")
        {
            if (invincibleTimer <= 0)
            {
                gameManager.Hit();
                rb.constraints = RigidbodyConstraints.None;
                rb.velocity = new Vector3(Random.Range(GameManager._worldSpeed / 2f, -GameManager._worldSpeed / 2f), 2.5f, -GameManager._worldSpeed / 2f);
                gameObject.GetComponent<Animator>().enabled = false;
                ShakeController._isShake = true;
            }
        }

        if (other.tag == "Collactable")
        {
            other.transform.GetChild(0).gameObject.SetActive(false);

            audioManager.coin.Stop();
            audioManager.coin.Play();

            currentFuel += (gameManager.worldSpeed * fuelMultiplier) * Time.deltaTime;
        }
    }

    public void ResetPos()
    {
        gameObject.GetComponent<Animator>().enabled = true; //re-enable the animations
        rb.constraints = RigidbodyConstraints.FreezeRotation; // Cannot diseppear after death

        transform.position = startPos;
        transform.rotation = startRotation;

        invincibleTimer = invincibleTime;
    }
}
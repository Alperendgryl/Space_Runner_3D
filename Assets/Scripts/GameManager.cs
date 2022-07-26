using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject plane;

    [Header("Scripts")]
    public PlayerController player;

    public bool canMove;
    public static bool _canMove;

    private bool gameStarded;

    public AudioManager audioManager;

    [Header("Character")]
    public GameObject currentChar;

    [Header("SetActive")]
    public GameObject[] NotActiveStart;
    public GameObject[] NotActiveEnd;
    public GameObject DeathPanel;

    [Header("Score")]
    public TMP_Text scoreTXT;
    private float score;

    public TMP_Text bestScoreTXT;
    public TMP_Text NewBestScoreTXT;
    public TMP_Text endScoreTXT;
    private float bestScore;

    [Header("Speed")]
    public float worldSpeed;
    public static float _worldSpeed;

    public float IncreaseSpeedTimeDiff;
    public float speedMultiplier;
    public static float _speedMultiplier;

    private float Counter;


    [SerializeField] private GameObject SunLight;

    public void Start()
    {
        WhileStart();
        StartCoroutine(Rotate(180));
    }

    IEnumerator Rotate(float duration)
    {
        float startRotation = SunLight.transform.eulerAngles.y;
        float endRotation = startRotation + 360.0f;
        float t = 0.0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            float yRotation = Mathf.Lerp(startRotation, endRotation, t / duration) % 360.0f;
            SunLight.transform.eulerAngles = new Vector3(SunLight.transform.eulerAngles.x, yRotation, SunLight.transform.eulerAngles.z);
            yield return null;
        }
    }

    public void Update()
    {
        Score();
        IncreaseSpeed();
        WhileUpdate();
        LosePanel();
    }

    private void LosePanel()
    {
        if (score > bestScore)
        {
            bestScore = score;
            NewBestScoreTXT.text = "NEW BEST SCORE";
            PlayerPrefs.SetFloat("BestScore", bestScore);
        }
        bestScoreTXT.text = ((int)bestScore).ToString();
        endScoreTXT.text = ((int)score).ToString();
    }

    private void WhileUpdate()
    {
        bestScore = PlayerPrefs.GetFloat("BestScore");

        _canMove = canMove;
        _worldSpeed = worldSpeed;
        _speedMultiplier = speedMultiplier;
    }

    private void WhileStart()
    {
        Counter = IncreaseSpeedTimeDiff;
        plane.tag = "Obstacles";
    }

    public void GameStarts()
    {
        if (!gameStarded)
        {
            canMove = true;
            gameStarded = true;

            for (int i = 0; i < NotActiveStart.Length; i++)
            {
                NotActiveStart[i].SetActive(false);
            }
        }
    }
    private void IncreaseSpeed()
    {
        if (canMove)
        {
            Counter -= Time.deltaTime;

            if (Counter <= 0)
            {
                Counter = IncreaseSpeedTimeDiff;
                worldSpeed *= speedMultiplier;
            }
        }
    }
    public void Hit()
    {
        canMove = false;
        plane.tag = "Untagged";
        StartCoroutine(GameEnd());
        player.explosionParticles.Play(); // play the explosion effect
        audioManager.explosion.Play();
    }

    private IEnumerator GameEnd()
    {
        audioManager.gameMusic.Stop();
        yield return new WaitForSeconds(1.5f);
        for (int i = 0; i < NotActiveEnd.Length; i++)
        {
            NotActiveEnd[i].SetActive(false);
        }
        DeathPanel.SetActive(true);
        audioManager.gameOver.Play();
    }

    private void Score()
    {
        if (canMove)
        {
            score += Time.deltaTime * worldSpeed;
            scoreTXT.text = ((int)score).ToString();
        }
    }
}

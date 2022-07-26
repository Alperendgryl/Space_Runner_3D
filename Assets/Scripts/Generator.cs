using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{
    [SerializeField] private Transform generationThreshold;
    private void Start()
    {
        TimeDiff();
        InitiliazeFuels();
        InitiliazeObjects();
    }
    private void Update()
    {
        if (GameManager._canMove)
        {
            SpawnMeteors();
            SpawnObjects();
            SpawnFuels();
        }
    }

    #region Fuels
    [Header("Fuel Section")]
    [Space]
    [SerializeField] private GameObject fuel;
    [SerializeField] private float fuelTimeDiff;
    private GameObject[] fuelPrefabs = new GameObject[5];
    private float fuelConstant;
    private void InitiliazeFuels()
    {
        for (int i = 0; i < 5; i++)
        {
            int randomPos = Random.Range(4, 20);
            fuelPrefabs[i] = Instantiate(fuel, (generationThreshold.position + (new Vector3(0,1,1) * randomPos)), Quaternion.identity);
            fuelPrefabs[i].SetActive(false);
        }
    }

    private void SpawnFuels()
    {
        fuelConstant -= Time.deltaTime;

        if (fuelConstant <= 0)
        {
            int random = Random.Range(0, fuelPrefabs.Length);

            if (!fuelPrefabs[random].activeInHierarchy)
            {
                fuelPrefabs[random].SetActive(true);
            }
            else
            {
                random = Random.Range(0, fuelPrefabs.Length);
            }
            fuelConstant = Random.Range(fuelTimeDiff / 2, fuelTimeDiff);
        }
    }
    #endregion

    #region Objects
    [SerializeField] private GameObject[] objects;
    [SerializeField] private float objectsTimeDiff;
    [SerializeField] private float minPos, maxPos;
    private float objectConstant;

    private GameObject[] objectsPrefabs = new GameObject[67]; // objects.length
    private void InitiliazeObjects()
    {
        for (int i = 0; i < objects.Length; i++)
        {
            float randomPos = Random.Range(minPos, maxPos);

            objectsPrefabs[i] = Instantiate(objects[i], new Vector3(randomPos, 0f, generationThreshold.position.z), Quaternion.Euler(0f, Random.Range(-180f, 180f), 0f));
            objectsPrefabs[i].SetActive(false);
        }
    }
    private void SpawnObjects()
    {
        objectConstant -= Time.deltaTime;

        if (objectConstant <= 0)
        {
            int randomObject = Random.Range(0, objects.Length);

            if (!objectsPrefabs[randomObject].activeInHierarchy)
            {
                objectsPrefabs[randomObject].SetActive(true);
            }
            else
            {
                randomObject = Random.Range(0, objects.Length);
            }

            objectConstant = Random.Range(objectsTimeDiff / 4, objectsTimeDiff);
        }
    }
    #endregion

    #region Meteors
    [Header("Meteors Section")]
    [Space]
    [SerializeField] private GameObject[] meteors;
    [SerializeField] private Transform meteorTransform;
    [SerializeField] private float meteorTimeDiff = 5f;
    private float meteorConstant;

    [SerializeField] private AudioManager audioManager;
    [SerializeField] private ParticleSystem meteorFire;

    private void SpawnMeteors()
    {
        if (GameManager._canMove)
        {
            meteorConstant -= Time.deltaTime;

            if (meteorConstant <= 0)
            {
                int random = Random.Range(0, meteors.Length);

                Instantiate(meteors[random], meteorTransform.position, Quaternion.Euler(Random.Range(-180f, 180f), Random.Range(-180f, 180f), Random.Range(-180f, 180f)));
                StartCoroutine(meteorShake());
                meteorConstant = Random.Range(meteorTimeDiff / 4, meteorTimeDiff);
            }
        }
    }

    private IEnumerator meteorShake()
    {
        ShakeController._isShake = true;
        yield return new WaitForSeconds(2.80f);
        ShakeController._isShake = false;
        audioManager.meteorHit.Play();
    }
    #endregion

    private void TimeDiff()
    {
        meteorFire.playOnAwake = true;

        meteorConstant = meteorTimeDiff;
        objectConstant = objectsTimeDiff;
        fuelConstant = fuelTimeDiff;
    }
}
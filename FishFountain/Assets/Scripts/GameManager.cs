using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Info")]
    [SerializeField] bool isAlive = true;
    [SerializeField] List<Fruit> fruitsOnStage;
    [SerializeField] List<Fruit> fruitsCollected;
    [SerializeField] float currentCountdown;
    [SerializeField] bool runningCountdown = false;
    [SerializeField] bool isDead = false;

    [Header("Config")]
    [SerializeField] float timeToCountdown = 60f;
    [SerializeField] int maxNumberOfFruitsOnStage = 5;
    [SerializeField] float chanceOfSpawn = 30;
    [SerializeField] int[] spawnRangeOnX = new int[2];
    [SerializeField] int[] spawnRangeTime = new int[2];
    [SerializeField] int heightOdSpawn = 7;
    [SerializeField] bool canSpawn = true;


    [Header("Data")]
    [SerializeField] List<Fruit> allSpawnableFruits;
    [SerializeField] FruitPickup pickupPrefab;

    [Header("UI Elements")]
    [SerializeField] TMP_Text uiTimer;
    [SerializeField] GameObject gameOverText;
    [SerializeField] Slider breathSlider;

    [Header("Components")]
    [SerializeField] FishController controller;
    [SerializeField] Backpack backpack;
    [SerializeField] Mover mover;
    [SerializeField] UiController ui;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
        currentCountdown = timeToCountdown;
    }

    // Start is called before the first frame update
    void Start()
    {
        controller = FindObjectOfType<FishController>();
        backpack = FindObjectOfType<Backpack>();
        mover = FindObjectOfType<Mover>();
        ui = FindObjectOfType<UiController>();
        Setup();
    }

    // Update is called once per frame
    void Update()
    {
        if (runningCountdown)
        {
            controller.SetCanCount(true);
            CountDown();
            if (canSpawn)
            {
                StartCoroutine(SpawnFruit());
            }
            UpdateBreathSlider();
        }
        else
        {
            if (Input.GetKey(KeyCode.Space))
            {
                SceneManager.LoadScene(0);
            }
        }
    }

    private void Setup()
    {
        SetupBreathSlider();
        runningCountdown = true;
    }
    private void SetupBreathSlider()
    {
        breathSlider.maxValue = controller.GetBreathingCapacity();
        breathSlider.minValue = 0;
        breathSlider.value = controller.GetBreathCountdown();
    }

    private void UpdateBreathSlider()
    {
        breathSlider.value = controller.GetBreathCountdown();
        if (controller.GetBreathCountdown() <= 0)
        {
            isDead = true;
            GameOver();
        }
    }

    private IEnumerator SpawnFruit()
    {

        yield return new WaitForSeconds(Random.Range(spawnRangeTime[0], spawnRangeTime[1]));
        if (canSpawn)
        {
            canSpawn = false;
            if (fruitsOnStage.Count < maxNumberOfFruitsOnStage)
            {
                int index = Random.Range(0, allSpawnableFruits.Count);
                Fruit fruit = allSpawnableFruits[index];
                if (Random.Range(0, 100) < fruit.GetSpawnChance())
                {
                    var spawnedFruit = Instantiate(pickupPrefab, new Vector3(Random.Range(spawnRangeOnX[0], spawnRangeOnX[1]), heightOdSpawn, 0), Quaternion.identity);
                    spawnedFruit.SetupFruit(fruit);
                    fruitsOnStage.Add(fruit);
                }
            }
            yield return new WaitForSeconds(1f);
            canSpawn = true;
        }
    }
    private void UpdateTimer(float value)
    {
        if(value < 0)
        {
            uiTimer.text = 0.ToString();
        }
        else
        {
            uiTimer.text = Mathf.FloorToInt(value).ToString();
        }
        
        if(value == 0)
        {
            uiTimer.color = Color.red;
            return;
        }
        if(value <= (timeToCountdown / 2))
        {
            if(value <= 15)
            {
                uiTimer.color = Color.red;
                return;
            }
            uiTimer.color = Color.yellow;
        }
    }
    private void CountDown()
    {
        if (currentCountdown > 0 && runningCountdown)
        {
            currentCountdown -= Time.deltaTime;
            UpdateTimer(currentCountdown);
        }
        if (currentCountdown <= 0)
        {
            GameOver();
        }
    }

    private void DestroyFruitsOnStage()
    {
        foreach(FruitPickup fruit in FindObjectsOfType<FruitPickup>())
        {
            Destroy(fruit.gameObject);
        }
        fruitsOnStage.Clear();
    }

    private void GameOver()
    {
        StopAllCoroutines();
        DestroyFruitsOnStage();
        controller.SetCanCount(false);
        runningCountdown = false;
        canSpawn = false;
        mover.SetCanMove(false);
        if (isDead)
        {
            ui.ShowGameOver();
        }
        else
        {
            ui.SetupResultsPanel(fruitsCollected, backpack.GetWeight());
        }
    }

    private void StartCountdown()
    {
        runningCountdown = true;
        canSpawn = true;
        currentCountdown = timeToCountdown;
    }

    #region PUBLIC METHODS
    public bool GetIsAlive()
    {
        return isAlive;
    }

    public void ColectFruit(Fruit fruit)
    {
        fruitsOnStage.Remove(fruit);
    }
    public void InteractWithFountain()
    {
        if (!runningCountdown) return;

        List<Fruit> backpackContent = backpack.GetBackpackContent();
        foreach(Fruit fruit in backpackContent)
        {
            fruitsCollected.Add(fruit);
        }
        backpack.ResetBackpack();
    }
    #endregion
}

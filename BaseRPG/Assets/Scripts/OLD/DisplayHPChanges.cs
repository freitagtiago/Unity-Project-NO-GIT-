using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class DisplayHPChanges : MonoBehaviour
{
    [SerializeField] Text displayer;
    float lifetime = 1f;
    float moveSpeed = 1;
    float placementJitter = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {
        Destroy(gameObject, lifetime);
        transform.position += new Vector3(0f, moveSpeed * Time.deltaTime, 0f);
    }

    public void DisplayHPChange(int amount)
    {
        displayer.text = amount.ToString();
        SetColor(amount);
        transform.position += new Vector3(Random.Range(-placementJitter, placementJitter)
                                          , Random.Range(-placementJitter, placementJitter)
                                          , transform.position.z);
    }

    private void SetColor(int amount)
    {
        if (amount > 0)
        {
            displayer.color = Color.green;

        }
        else if (amount < 0)
        {
            displayer.color = Color.red;
        }
        else
        {
            displayer.color = Color.gray;
        }
    }
}

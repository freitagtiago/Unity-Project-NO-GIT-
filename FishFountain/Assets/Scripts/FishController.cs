using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishController : MonoBehaviour
{
    [SerializeField] float generalCountdown;
    [SerializeField] float breathCountdown;
    [SerializeField] float breathingCapacity;
    [SerializeField] bool canCount = false;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (canCount)
        {
            BreathCountdown();
        }
    }

    private void BreathCountdown()
    {
        if(breathCountdown > 0)
        {
            breathCountdown -= Time.deltaTime;
        }
    }

    public float GetBreathCountdown()
    {
        return breathCountdown;
    }

    public float GetBreathingCapacity()
    {
        return breathingCapacity;
    }

    public void ResetBreathCountdown()
    {
        breathCountdown = breathingCapacity;
    }

    public void SetCanCount(bool value)
    {
        canCount = value;
    }

    public void RestoreBreath()
    {
        breathCountdown = breathingCapacity;
    }
}

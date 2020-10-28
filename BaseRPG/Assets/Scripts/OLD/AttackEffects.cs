using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackEffects : MonoBehaviour
{
    [SerializeField] float effectLength;
    [SerializeField] int soundEffect;

    // Start is called before the first frame update
    void Start()
    {
        AudioController.instance.PlaySFX(soundEffect);
    }

    // Update is called once per frame
    void Update()
    {
        Destroy(gameObject, effectLength);
    }
}

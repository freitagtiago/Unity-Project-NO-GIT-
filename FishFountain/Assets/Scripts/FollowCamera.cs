using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] Transform target;

    private Mover mover = null;

    private void Awake()
    {
        if(mover == null)
        {
            mover = GameObject.FindGameObjectWithTag("Player").GetComponent<Mover>();
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (target == null) return;

        transform.position = new Vector3(target.position.x
                                        , transform.position.y
                                        , transform.position.z);
    }
}

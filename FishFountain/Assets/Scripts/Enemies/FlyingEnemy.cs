using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemy : MonoBehaviour, IEnemy
{
    enum EnemyState
    {
        Spawning,
        Moving,
        Chasing,
        Attacking,
        Vanishing,
        None
    }

    [SerializeField] FishController target;
    [SerializeField] Rigidbody2D rig;

    [SerializeField] float spawnChance;
    [SerializeField] float timeToReduce;
    [SerializeField] float timeToAttack = 5f;
    
    EnemyState state;
    SpriteRenderer renderer;

    [Header("Movement Config")]
    Vector3 initialCyclePosition;
    Vector3 endCyclePosition;
    Vector3 targetPosition;
    bool switching = false;
    float maxSpeed = 4f;
    [SerializeField] float flyingRange = 5;
    [SerializeField] float flyingHeight = 4;

    private void Awake()
    {
        target = FindObjectOfType<FishController>();
        rig = GetComponent<Rigidbody2D>();
        renderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        state = EnemyState.Spawning;
        SetLimits();
        StartCoroutine(SetCanAttack());
    }

    private void SetLimits()
    {
        initialCyclePosition = new Vector3(transform.position.x - flyingRange, flyingHeight, transform.position.z);
        endCyclePosition = new Vector3(transform.position.x + flyingRange, flyingHeight, transform.position.z);
    }

    void FixedUpdate()
    {
        if(state == EnemyState.Spawning)
        {
            SetHeight();
            if(transform.position.y <= flyingHeight)
            {
                state = EnemyState.Moving;
                initialCyclePosition = transform.position;
            }
        }else if(state == EnemyState.Chasing)
        {
            Attack();
        }
        else if(state == EnemyState.Moving)
        {
            Move();
        }else if(state == EnemyState.Attacking)
        {
            if(transform.position.y <= targetPosition.y && (transform.position.x != targetPosition.x))
            {
                state = EnemyState.Vanishing;
            }
            else
            {
                if (rig.velocity.magnitude > maxSpeed)
                {
                    rig.velocity = rig.velocity.normalized * maxSpeed;
                }
            }
        }else if (state == EnemyState.Vanishing)
        {
            Vanish();
        }
    }

    private void SetHeight()
    {
        rig.velocity = new Vector3(0,-flyingHeight, 0);
    }


    private void Vanish()
    {
        state = EnemyState.None;
        rig.velocity = new Vector3(transform.position.x, transform.position.y + 10, transform.position.z);
        FindObjectOfType<GameManager>().ReduceAmountOfEnemies();
        Destroy(this.gameObject, 3f);
    }

    private IEnumerator SetCanAttack()
    {
        yield return new WaitForSeconds(timeToAttack);
        targetPosition = target.transform.position;
        state = EnemyState.Chasing;
    }

    private void Movement()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            target.ReduceBreath(timeToReduce);
        }
    }

    #region IEnemy Methods
    public void Attack()
    {
        state = EnemyState.Attacking;
        if(targetPosition.x < transform.position.x)
        {
            renderer.flipX = true;
        }
        else
        {
            renderer.flipX = false;
        }
        rig.velocity = targetPosition - transform.position;
    }

    public void Chase()
    {
      
    }

    public void Die()
    {
        
    }

    public void Move()
    {
        if (switching == false)
        {
            transform.position = Vector3.MoveTowards(transform.position, endCyclePosition, 5 * Time.deltaTime);
        }
        else if (switching == true)
        {
            transform.position = Vector3.MoveTowards(transform.position, initialCyclePosition, 5 * Time.deltaTime);
        }

        if (transform.position == endCyclePosition)
        {
            switching = true;
            renderer.flipX = true;
        }
        else if (transform.position == initialCyclePosition)
        {
            switching = false;
            renderer.flipX = false;
        }
    }

    public float GetSpawnChance()
    {
        return spawnChance;
    }
    #endregion
}

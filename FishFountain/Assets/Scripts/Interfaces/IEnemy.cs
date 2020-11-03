
using System.Collections;

public interface IEnemy
{
    void Move();
    void Attack();
    void Chase();
    void Die();
    float GetSpawnChance();

}

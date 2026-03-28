using UnityEngine;

public interface IHealth
{
    public void TakeDamage(float damage); 
    public void TakeDamage(float damage, Vector2 damageDirection); 
}
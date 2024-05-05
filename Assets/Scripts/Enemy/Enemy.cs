using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private GameManager gameManager => GameManager.instance;

    private float speed;
    [SerializeField] private float startSpeed = 10f;
    public float Speed => speed;
    [SerializeField] private int damage = 1;
    public int Damage => damage;
    [SerializeField] private float hp = 100;
    [SerializeField] private int value = 50;

    [SerializeField] private GameObject deathEffect;
    private bool isDead = false;

    private void Start()
    {
        speed = startSpeed;
        isDead = false;
    }

    public void TakeDamage(float amount)
    {
        if (isDead) return;

        hp -= amount;
        if (hp <= 0)
            Die();
    }

    private void Die()
    {
        isDead = true;
        GameObject effect = Instantiate(deathEffect,transform.position,Quaternion.identity);
        Destroy(effect,5f);

        WaveSpawner.EnemiesAlive--;

        gameManager.AddMoney(value);
        Destroy(gameObject);
    }

    public void Slow(float pct)
    {
        speed = startSpeed * (1f - pct);
    }

    public void ResetSpeed()
    {
        speed = startSpeed;
    }
}

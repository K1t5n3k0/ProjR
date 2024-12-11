using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;
    private Animator anim;
    
    void Start()
    {
        currentHealth = maxHealth;
        anim = GetComponent<Animator>();
    }
    
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        
        // 播放受伤动画
        anim?.SetTrigger("hurt");
        
        if(currentHealth <= 0)
        {
            Die();
        }
    }
    
    void Die()
    {
        // 播放死亡动画
        anim?.SetTrigger("die");
        
        // 禁用敌人
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
        
        // 可以选择延迟销毁对象
        Destroy(gameObject, 1f);
    }
}
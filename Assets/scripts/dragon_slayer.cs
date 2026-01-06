using System.Runtime.CompilerServices;
using UnityEngine;

public class dragon_slayer : MonoBehaviour
{
    private BoxCollider swordCollider;
    private void Start()
    {
        swordCollider = GetComponentInChildren<BoxCollider>();
        swordCollider.enabled = false;
    }
    public void EnableSwordCollider()
    {
        swordCollider.enabled = true;
    }
    public void DisableSwordCollider()
    {
        swordCollider.enabled = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<enemyAI>().TakeDamage(10);
            UnityEngine.Debug.Log("Enemy hit by sword!");
        }
    }
    private void Hit()
    {
        Debug.Log("Hit detected!");
    }
}

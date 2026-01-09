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
        if (other.gameObject.CompareTag("Enemy"))
        {
            IDamageable damageable = other.GetComponentInParent<IDamageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(10);
            }
        }
    }
}

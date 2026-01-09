using UnityEngine;

public class monstru2_attack : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            transform.parent.GetComponentInParent<monstru2>().Staggered();
            IDamageable damageable = other.GetComponentInParent<IDamageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(10);
            }
        }
        if (other.CompareTag("map"))
        { transform.parent.GetComponentInParent<monstru2>().Staggered(); 
        }
        if (other.CompareTag("Tower"))
        {
            transform.parent.GetComponentInParent<monstru2>().Staggered();
            other.GetComponent<SimpleBreakTower>().Break();
        }
    }

}

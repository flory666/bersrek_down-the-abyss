using UnityEngine;

public class SimpleBreakTower : MonoBehaviour
{
    public float destroyDelay = 4f;

    private Rigidbody[] cells;
    private bool broken = false;
    private BoxCollider bc;
    public audioMaster audioMaster;

    private void Awake()
    {   
        bc = GetComponent<BoxCollider>();
        cells = GetComponentsInChildren<Rigidbody>();
        audioMaster=GameObject.FindGameObjectWithTag("audio").GetComponent<audioMaster>();
        foreach (Rigidbody rb in cells)
        {
            rb.useGravity = false;
            rb.isKinematic = true;
        }
    }

    public void Break()
    {
        audioMaster.playSound(audioMaster.bricks);
        if (broken) return;
        broken = true;
        

        // 1️⃣ scoatem obstacolul REAL
        bc.enabled = false;

        // 2️⃣ distrugere vizuală
        foreach (Rigidbody rb in cells)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
            Destroy(rb.gameObject, destroyDelay);
        }

        // optional: distrugi și parentul după
        Destroy(gameObject, destroyDelay + 1f);
    }
}

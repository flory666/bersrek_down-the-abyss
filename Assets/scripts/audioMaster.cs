using UnityEngine;

public class audioMaster : MonoBehaviour
{
    [SerializeField] AudioSource sfx;   
    public AudioClip sword;
    public AudioClip monstru1;
    public AudioClip monstru2;
    public AudioClip steps;
    public AudioClip bricks;
    public void playSound(AudioClip clip)
    {sfx.PlayOneShot(clip);}
}

using UnityEngine;

public class Rotation : MonoBehaviour
{
    //perputaran sumbu z dengan menambahkan kecepatan
    private float speed = 2f;
    [SerializeField] AudioClip obstacleSFX;
    private new AudioSource audio;

    void Start()
    {
        audio = GetComponent<AudioSource>();
    }

    void Update()
    {
        transform.Rotate(0, 0, speed);
    }

    void OnTriggerEnter2D(Collider2D collision)
    { 
        if(collision.tag == "Ball")
        {
            audio.PlayOneShot(obstacleSFX);
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallControl : MonoBehaviour
{
    //Besarnya gaya awal yang diberikan untuk mendorong bola
    [Header("Kecepatan")]
    [SerializeField] float xInitialForce;
    [SerializeField] float yInitialForce;
    //Power
    [SerializeField] float power;
    //Audio Efek
    [Header("SFX")]
    [SerializeField] AudioClip hitSFX;
    [SerializeField] AudioClip wallSFX;
    //Partikel Efek
    [Header("Particle VFX")]
    [Tooltip("Untuk partikel efek")]
    [SerializeField] GameObject ObstaclesVFX;
    [SerializeField] GameObject PaddleVFX;
    [SerializeField] GameObject WallVFX;
    
    // Titik asal lintasan bola saat ini
    private Vector2 trajectoryOrigin;
    // Rigidbody 2D bola
    private new Rigidbody2D rigidbody2D;
    //Audio
    private new AudioSource audio;
    //memberikan pantulan kecepatan;
    private Vector2 direction;

    void ResetBall()
    {
        //Reset posisi menjadi (0,0)
        transform.position = Vector2.zero;

        //Reset kecepatan menjadi (0,0)
        rigidbody2D.velocity = Vector2.zero;   
    }
    void PushBall()
    {
        // Tentukan nilai komponen y dari gaya dorong antara -yInitialForce dan yInitialForce
        float yRandomInitialForce = Random.Range(-yInitialForce, yInitialForce);
        float xRandomInitialForce = Random.Range(-xInitialForce, xInitialForce);

        // Tentukan nilai acak antara 0 (inklusif) dan 2 (eksklusif)
        float randomDirection = Random.Range(0, 2);

        // Jika nilainya di bawah 1, bola bergerak ke kiri. 
        // Jika tidak, bola bergerak ke kanan.
        if (randomDirection < 1.0f)
        {
            // Gunakan gaya untuk menggerakkan bola ini.
            rigidbody2D.AddForce(new Vector2(-xInitialForce, yRandomInitialForce) * power * Time.deltaTime);
            rigidbody2D.AddForce(new Vector2(-yInitialForce, xRandomInitialForce) * power * Time.deltaTime);
        }
        else
        {
            rigidbody2D.AddForce(new Vector2(xInitialForce, yRandomInitialForce) * power * Time.deltaTime);
            rigidbody2D.AddForce(new Vector2(yInitialForce, xRandomInitialForce) * power * Time.deltaTime);
        }
    }

    void RestartGame()
    {
        //Kembalikan bola ke posisi semula
        ResetBall();

        //Setelah 2 detik, berikan gaya ke bola
        Invoke("PushBall", 2);
    }

    void Start()
    {
        trajectoryOrigin = transform.position;
        rigidbody2D = GetComponent<Rigidbody2D>();
        audio = GetComponent<AudioSource>();
        //Mulai game
        RestartGame();
    }
    void Update()
    {
     //..
    }

    // Ketika bola beranjak dari sebuah tumbukan, rekam titik tumbukan tersebut
    private void OnCollisionExit2D(Collision2D collision)
    {
        trajectoryOrigin = transform.position;
    }

    // Untuk mengakses informasi titik asal lintasan
    public Vector2 TrajectoryOrigin
    {
        get { return trajectoryOrigin; }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Paddle")
        {
            direction.x = -direction.x;
            PlayPaddleVFX();
            audio.PlayOneShot(hitSFX);
            //Menambah kecepatan ball setiap ball tersentuh dengan paddle
            power += Time.deltaTime;
        }

        if (collision.tag == "Wall")
        {
            direction.y = -direction.y;

            //Menambah kecepatan ball setiap ball tersentuh dengan wall
            power += Time.deltaTime;
        }

        if (collision.tag == "LeftRightWall")
        {
            PlayWallVFX();
            audio.PlayOneShot(wallSFX);
        }

        if (collision.tag == "Obstacles")
        {
            PlayObstaclesVFX();
        }

    }

    void PlayPaddleVFX()
    {
        //Efek ball jika terkenal paddle
        GameObject paddleSFX = (GameObject)Instantiate(PaddleVFX);
        paddleSFX.transform.position = transform.position;
        Destroy(paddleSFX, 1f);
    }

    void PlayWallVFX()
    {
        //Efek ball jika terkenal wall
        GameObject wallSFX = (GameObject)Instantiate(WallVFX);
        wallSFX.transform.position = transform.position;
        Destroy(wallSFX, 1f);
    }

    void PlayObstaclesVFX()
    {
        //Efek ball jika terkenal obstacle
        GameObject obstaclesSFX = (GameObject)Instantiate(ObstaclesVFX);
        obstaclesSFX.transform.position = transform.position;
        Destroy(obstaclesSFX, 1f);
    }
}





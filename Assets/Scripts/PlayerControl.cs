using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    //Tombol untuk menggerakan ke atas
    [Header("Input Button")]
    [SerializeField] KeyCode upButton = KeyCode.W;
    //Tombol untuk menggerakan ke bawah
    [SerializeField] KeyCode downButton = KeyCode.S;
    [Header("Speed Paddle")]
    //Kecepatan Gerak
    [SerializeField] float speed = 10.0f;
    [Header("Bg Boundary")]
    //Batas atas dan bawah game scene ( Batas bawah menggunakan minus(-))
    [SerializeField] float yBoundary = 9.0f;

    //Rigidbody2D racket ini
    private Rigidbody2D rigidBody2D;
    //Skor pemain
    private int score;
    //Paddle Scales
    Vector2 scales;
    //kondisi Paddle
    bool activePaddle = false;
    private static float timer = 5.0f;
  

    // Titik tumbukan terakhir dengan bola, untuk menampilkan variabel-variabel fisika terkait tumbukan tersebut
    private ContactPoint2D lastContactPoint;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        SpeedRacket();
        PositionRacket();
        PaddleScales();
    }

    public void SpeedRacket()
    {
        //Dapatkan kecepatan raket sekarang
        Vector2 velocity = rigidBody2D.velocity;

        //Jika pemain menekan tombol ke atas, beri kecepatan positif ke komponen y (KeyCode atas).
        if (Input.GetKey(upButton))
        {
            velocity.y = speed;
        }
        //Jika pemain menekan tombol ke bawah, beri kecepatan negatif ke komponen y (KeyCode bawah).
        else if (Input.GetKey(downButton))
        {
            velocity.y = -speed;
        }
        //Jika pemain tidak menekan tombol apa-apa, kecepatan nol.
        else
        {
            velocity.y = 0.0f;
        }

        //Masukan kembali kecepatannya ke rigidBody2D.
        rigidBody2D.velocity = velocity;
    }

    public void PositionRacket()
    {
        //Dapatkan posisi raket sekarang
        Vector3 position = transform.position;

        //Jika posisi raket melewati batas atas (YBoundary), kembalikan ke batas atas tersebut.
        if (position.y > yBoundary)
        {
            position.y = yBoundary;
        }
        //Jika posisi raket melewati abats bawah (-YBoundary), kembalikan ke batas atas tersebut.
        else if (position.y < -yBoundary)
        {
            position.y = -yBoundary;
        }
        //Masukan kembali posisinya ke transform.
        transform.position = position;
    }

    //Menaikan skor sebanyak 1 point
    public void IncrementScore()
    {
        score++;
    }

    //Mengembalikan skor menjadi 0
    public void ResetScore()
    {
        score = 0;
    }

    //Mendapatkan nilai skor
    public int Score 
    {
        get { return score; }
    }

    // Untuk mengakses informasi titik kontak dari kelas lain
    public ContactPoint2D LastContactPoint
    {
        get { return lastContactPoint; }
    }

    // Ketika terjadi tumbukan dengan bola, rekam titik kontaknya.
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name.Equals("Ball"))
        {
            lastContactPoint = collision.GetContact(0);
        } 
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Ball")
        {
            //aktifkan scale paddle
            activePaddle = true;
            PaddleScales();
        }
        if (collision.tag != "Ball")
        {
            //waktu paddle memperbesar
            timer--;
           if (timer < 0)
           {
                activePaddle = false;
                //paddle kembali normal setelah batas delta time selesai
                transform.localScale = scales.normalized;
            }
        }
        
    }

    void PaddleScales()
    {
        if (activePaddle)
        {
            //memperbesar paddle dengan waktu delta time
            scales = transform.localScale;
            scales.x += Time.deltaTime / 1.5f;
            scales.y += Time.deltaTime / 1.5f;
            transform.localScale = scales;
        }
    }
}




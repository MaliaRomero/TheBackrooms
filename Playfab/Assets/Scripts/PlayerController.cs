using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public float speed;
    private Rigidbody rig;

    private float startTime;
    private float timeTaken;

    private int collectablesPicked;
    public int maxCollectables = 10;
    private bool isPlaying;

    public GameObject playButton;
    public TextMeshProUGUI curTimeText;

    public float rotationSpeed = 150f;

    void Awake()
    {
        rig = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (!isPlaying)
            return;

        // Rotate left when 'Q' is pressed
        if (Input.GetKey("q"))
        {
            RotatePlayer(-5);

        }
        // Rotate right when 'E' is pressed
        else if (Input.GetKey("e"))
        {
            RotatePlayer(5);
        }

        float x = Input.GetAxis("Horizontal") * speed;
        float z = Input.GetAxis("Vertical") * speed;

        // Calculate movement direction based on both input and current rotation
        Vector3 moveDirection = new Vector3(x, 0f, z).normalized;

        // Calculate the rotation amount based on the current rotation
        Quaternion rotation = Quaternion.Euler(0f, transform.eulerAngles.y, 0f);

        // Apply rotation to the movement direction
        moveDirection = rotation * moveDirection;

        // Apply movement to the rigidbody
        rig.velocity = new Vector3(moveDirection.x * speed, rig.velocity.y, moveDirection.z * speed);

        curTimeText.text = (Time.time - startTime).ToString("F2");


    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Collectable"))
        {
            collectablesPicked++;
            Destroy(other.gameObject);
            if (collectablesPicked == maxCollectables)
                End();
        }
    }

    public void Begin()
    {
        startTime = Time.time;
        isPlaying = true;
        playButton.SetActive(false);

    }

    void End()
    {
        timeTaken = Time.time - startTime;
        isPlaying = false;
        Leaderboard.instance.SetLeaderboardEntry(-Mathf.RoundToInt(timeTaken * 1000.0f));
        playButton.SetActive(true);    
    }

    void RotatePlayer(int direction)
    {
        // Calculate the rotation amount based on the direction
        float rotationAmount = direction * rotationSpeed * Time.deltaTime;

        // Apply the rotation to the player's transform
        transform.Rotate(Vector3.up, rotationAmount);
    }

}

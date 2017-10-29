using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Knife : MonoBehaviour {

    private Vector3 startSwipePos;
    private Vector3 endSwipePos;
    private float timeWhenWeStartedFlying;

    public Rigidbody rb;
    public float force = 5f;
    public float torque = 20f;

	void Start () {
		
	}

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startSwipePos = Camera.main.ScreenToViewportPoint(Input.mousePosition); //Converts from screen space to viewport space
        }
        if (Input.GetMouseButtonUp(0))
        {
            endSwipePos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
            Swipe();
        }
    }

    void Swipe()
    {
        rb.isKinematic = false;

        timeWhenWeStartedFlying = Time.time;

        Vector2 swipe = endSwipePos - startSwipePos;

        Debug.Log(swipe);

        rb.AddForce(swipe * force, ForceMode.Impulse);
        rb.AddTorque(0f, 0f, -torque, ForceMode.Impulse);

    }

    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "WoodenBlock")
        {
            rb.isKinematic = true; //freezes position and rotation
        }
        else
        {
            Restart();
        }
    }

    void OnCollisionEnter()
    {

        float timeInAir = Time.time - timeWhenWeStartedFlying;

        if (!rb.isKinematic && timeInAir >= .05f)
        {
            Restart();
        }
            
    }

    void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}

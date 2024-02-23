using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AFKPrompter : MonoBehaviour
{
    public Transform orientation;
    public Camera mainCamera;
    public float delay;

    private float timer;


    // Start is called before the first frame update
    void Start()
    {
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;


        if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
        {
            timer = 0;
        }

        if (Input.GetKeyDown(KeyCode.W) ||
            Input.GetKeyDown(KeyCode.S) ||
            Input.GetKeyDown(KeyCode.A) ||
            Input.GetKeyDown(KeyCode.D) ||
            Input.GetKeyDown(KeyCode.Space) ||
            Input.GetKeyDown(KeyCode.R) ||
            Input.GetMouseButtonDown(0))
        {
            timer = 0;
        }


        if (timer > delay)
        {
            timer = 0;
            RaycastAndMessage();
        }
    }


    private void RaycastAndMessage()
    {
        // Cast a ray from the mouse cursor position
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        LayerMask layerMask = LayerMask.GetMask("PickUp");

        // Perform the raycast
        if (Physics.Raycast(ray, out hit, 100))
        {
            //, can't wait to take you by surprise and break stuff!
            if (hit.collider.gameObject.tag == "Ground")
                Subtitles.Instance.sendMessage("I am going to stare at the ground.");
            else if (hit.collider.gameObject.tag == "Wall")
                Subtitles.Instance.sendMessage("I am going to stare at the wall.");
            else if (hit.collider.gameObject.tag == "Ceiling")
                Subtitles.Instance.sendMessage("I am going to stare at the ceiling.");
        }
    }
}

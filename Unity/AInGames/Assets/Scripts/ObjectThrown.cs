using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectThrown : MonoBehaviour
{
    public Rigidbody rb;
    public string message;
    public string pickUpPrompt;
    private void OnCollisionEnter(Collision collision)
    {
        if (rb.velocity.magnitude > 6)
        {
            Subtitles.Instance.sendMessage(message);
        }
    }
}

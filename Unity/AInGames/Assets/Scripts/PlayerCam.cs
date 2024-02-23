
using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    public float sensX;
    public float sensY;

    public Transform orientation;

    float xRot;
    float yRot;

    public Camera mainCamera;

    public Transform holder;

    private bool holding;
    private Rigidbody heldObject;

    private float diffX = 0;
    private float diffY = 0;

    private Quaternion rotation;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        float mX = (Input.GetAxisRaw("Mouse X") - diffX) * Time.deltaTime * sensX;
        float mY = (Input.GetAxisRaw("Mouse Y") - diffY) * Time.deltaTime * sensY;


        yRot += mX;
        xRot -= mY;



        if (holding && Input.GetKey(KeyCode.R))
        {
            heldObject.rotation = Quaternion.Euler(xRot, yRot, 0f);
        }
        else
        {
            xRot = Mathf.Clamp(xRot, -90f, 90f);

            transform.rotation = Quaternion.Euler(xRot, yRot, 0);
            orientation.rotation = Quaternion.Euler(0, yRot, 0);
        }

        // Check if the left mouse button is clicked
        if (Input.GetMouseButtonDown(0))
        {
            PickUp();
        }


        if (Input.GetMouseButtonUp(0))
        {
            Drop();
        }

    }


    private void FixedUpdate()
    {

        if (holding)
        {
            Vector3 diff = (holder.transform.position - heldObject.position);

            heldObject.velocity = diff * 15;
        }

    }

    private void PickUp()
    {
        // Cast a ray from the mouse cursor position
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        LayerMask layerMask = LayerMask.GetMask("PickUp");

        // Perform the raycast
        if (Physics.Raycast(ray, out hit, 2, layerMask))
        {
            holding = true;

            heldObject = hit.collider.gameObject.GetComponent<Rigidbody>();
            heldObject.freezeRotation = true;

            ObjectThrown pickUp = hit.collider.gameObject.GetComponent<ObjectThrown>();
            if (pickUp && Random.Range(0, 3) == 0)
                Subtitles.Instance.sendMessage(pickUp.pickUpPrompt);
        }
    }

    private void Drop()
    {
        if (heldObject != null)
            heldObject.freezeRotation = false;

        holding = false;
    }
}

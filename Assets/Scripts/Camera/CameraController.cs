using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private bool doMovement = true;

    [SerializeField] private float panSpeed = 30f;
    [SerializeField] private float panBorderThicknes = 10f;
    [SerializeField] private float scrollSpeed = 5f;

    [SerializeField] private float minY = 10f;
    [SerializeField] private float maxY = 80f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
            doMovement = !doMovement;

        if (!doMovement)
            return;

        if(Input.GetKey(KeyCode.W) || Input.mousePosition.y >= Screen.height - panBorderThicknes)
            transform.Translate(Vector3.forward * panSpeed * Time.deltaTime, Space.World);
        if (Input.GetKey(KeyCode.S) || Input.mousePosition.y <= panBorderThicknes)
            transform.Translate(Vector3.back * panSpeed * Time.deltaTime, Space.World);
        if (Input.GetKey(KeyCode.D) || Input.mousePosition.x >= Screen.width - panBorderThicknes)
            transform.Translate(Vector3.right * panSpeed * Time.deltaTime, Space.World);
        if (Input.GetKey(KeyCode.A) || Input.mousePosition.x <= panBorderThicknes)
            transform.Translate(Vector3.left * panSpeed * Time.deltaTime, Space.World);


        float scroll = Input.GetAxis("Mouse ScrollWheel");
        Vector3 pos = transform.position;

        pos.y -= scroll * 1000 * scrollSpeed * Time.deltaTime;
        pos.y = Mathf.Clamp(pos.y,minY,maxY);
        transform.position = pos;
    }
}
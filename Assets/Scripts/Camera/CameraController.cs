using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float panSpeed = 30f;
    [SerializeField] private float panBorderThicknes = 10f;
    [SerializeField] private float scrollSpeed = 5f;

    [SerializeField] private float minY = 10f;
    [SerializeField] private float maxY = 80f;
    private bool canMove = true;
    // Start is called before the first frame update
    void Start()
    {
        canMove = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!canMove)
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

    private void OnEnable()
    {
        GameManager.onGameOver += GameEnd;
    }
    private void OnDisable()
    {
        GameManager.onGameOver -= GameEnd;
    }

    private void GameEnd()
    {
        canMove = false;
    }
}

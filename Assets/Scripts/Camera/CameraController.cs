using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float scrollSpeed = 10f;
    [SerializeField] private Rect bound = new Rect(0,0,29,29);

    [SerializeField] private float minY = 5f;
    [SerializeField] private float maxY = 15f;

    [SerializeField] private Transform followTarget;
    [SerializeField] private BoxCollider cameraConfine;
    [SerializeField] private CinemachineVirtualCamera virtualCamera;

    private bool canMove = true;
    // Start is called before the first frame update
    void Start()
    {
        canMove = true;
    }

    public void SetCamera(Vector2Short mapSize)
    {
        bound = new Rect(0,0,mapSize.x - 1,mapSize.y - 1);
        cameraConfine.size = new Vector3(mapSize.x + 5, 40f, mapSize.y + 5);
        cameraConfine.transform.position = new Vector3(mapSize.x / 2 , 0 , mapSize.y / 3);
    }

    // Update is called once per frame
    void Update()
    {
        if (!canMove)
            return;

        if(Input.GetKey(KeyCode.W) && (followTarget.localPosition + Vector3.forward).z < bound.yMax / 1.5)
        {
            followTarget.Translate(Vector3.forward * moveSpeed * Time.deltaTime,Space.World);
        }
        if (Input.GetKey(KeyCode.A) && (followTarget.localPosition + Vector3.left).x > bound.xMin)
        {
            followTarget.Translate(Vector3.left * moveSpeed * Time.deltaTime, Space.World);
        }
        if (Input.GetKey(KeyCode.S) && (followTarget.localPosition + Vector3.back).z > bound.yMin)
        {
            followTarget.Translate(Vector3.back * moveSpeed * Time.deltaTime, Space.World);
        }
        if (Input.GetKey(KeyCode.D) && (followTarget.localPosition + Vector3.right).x < bound.xMax)
        {
            followTarget.Translate(Vector3.right * moveSpeed * Time.deltaTime, Space.World);
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        virtualCamera.m_Lens.OrthographicSize -= scroll * scrollSpeed * 25f * Time.deltaTime;
        virtualCamera.m_Lens.OrthographicSize = Mathf.Clamp(virtualCamera.m_Lens.OrthographicSize, minY,maxY);
    }

    private void OnEnable()
    {
        EventHelper.GameOverEvent += GameOverEvent;
        EventHelper.GameWonEvent += GameWinEvent;
    }

    private void OnDisable()
    {
        EventHelper.GameOverEvent -= GameOverEvent;
        EventHelper.GameWonEvent -= GameWinEvent;
    }

    private void GameOverEvent(object s, GameEvent.GameOverEvent e)
    {
        canMove = false;
    }

    private void GameWinEvent(object s, GameEvent.GameWinEvent e)
    {
        canMove = false;
    }
}

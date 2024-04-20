using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager instance;
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("¦³¦h­ÓBuildManager");
            return;
        }
        instance = this;
    }
   
    private GameObject turretToBuild;
    public GameObject TurretTobuild => turretToBuild;

    [SerializeField]private GameObject standardTurretPrefab;
    // Start is called before the first frame update
    void Start()
    {
        turretToBuild = standardTurretPrefab;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

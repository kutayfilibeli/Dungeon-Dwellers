using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public static CameraController instance;

    public float moveSpeed;

    public Transform target;

    public Camera mainCamera, mapCamera;

    private bool isMapActive;
    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(target != null)
        {

             transform.position = Vector3.MoveTowards(transform.position, new Vector3(target.position.x, target.position.y, transform.position.z), moveSpeed * Time.deltaTime);

        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            if (!isMapActive)
            {
                ActivateMap();
            }
            else
            {
                DeactivateMap();
            }
        }
        
    }

    public void ChangeTarget(Transform newTarget)
    {
        target = newTarget;
    }

    public void ActivateMap()
    {
        if (!LevelManager.instance.isPaused)
        {
            isMapActive = true;

            mapCamera.enabled = true;

            mainCamera.enabled = false;

            PlayerController.instance.canMove = false;

            Time.timeScale = 0;

            UIController.instance.mapDisplay.SetActive(false);
            UIController.instance.bigMapText.SetActive(true);
        }
        
       
    }

    public void DeactivateMap()
    {
        isMapActive = false;
        mapCamera.enabled = false;

        mainCamera.enabled = true;

        PlayerController.instance.canMove = true;

        Time.timeScale = 1;

        UIController.instance.mapDisplay.SetActive(true);
        UIController.instance.bigMapText.SetActive(false);
    }
}

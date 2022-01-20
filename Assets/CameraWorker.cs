using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraWorker : MonoBehaviour
{

    [SerializeField] GameObject followObject;
    [SerializeField] float maxZoom, minZoom;
    [SerializeField] float zoomScalar;
    Vector3 newPosition;
    float distance;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.LookAt(followObject.transform.position);
        ZoomHandler();


        
    }

    void ZoomHandler()
    {
        if (Input.mouseScrollDelta.y > 0)
        {
            newPosition = this.transform.position + transform.forward * zoomScalar;
            distance = Vector3.Distance(followObject.transform.position, newPosition);

            if (distance > maxZoom)
            {
                this.transform.position = newPosition;
            }

        }

        if (Input.mouseScrollDelta.y < 0)
        {
            newPosition = this.transform.position - transform.forward * zoomScalar;
            distance = Vector3.Distance(followObject.transform.position, newPosition);

            if (distance < minZoom)
            {
                this.transform.position = newPosition;
            }
        }
    }
}


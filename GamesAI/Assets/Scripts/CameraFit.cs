using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFit : MonoBehaviour
{
    public float width = 102f;
    public float height = 104f;

    private Camera cam;
	// Use this for initialization
	void Start ()
	{
	    cam = GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void Update () {
        float aspect = width / (height / 2);
        if (cam.aspect <= aspect)
        {
            cam.orthographicSize = width / 2 / cam.aspect;
        }
        else
        {
            cam.orthographicSize = height / 4;
        }
    }
}

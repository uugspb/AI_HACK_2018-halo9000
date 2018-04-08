using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScroll : MonoBehaviour {

    public float speed;
    private Vector2 startPos;
    private Camera cam;
    private Vector2 targetPos;
    private float wasdSpeed = 5f;

	void Start ()
    {
        cam = GetComponent<Camera>();
        targetPos = new Vector2(transform.position.x, transform.position.y);
	}
	
	// Update is called once per frame
	void Update ()
    {
	    if (Input.GetMouseButtonDown(0))
        {
            startPos = cam.ScreenToWorldPoint(Input.mousePosition);
            //transform.position = new Vector3(Mathf.Lerp(transform.position.x, targetPos.x, speed * Time.deltaTime), Mathf.Lerp(transform.position.y, targetPos.y, speed * Time.deltaTime), transform.position.z);
        }
        else if (Input.GetMouseButton(0))
        {

            float posX = cam.ScreenToWorldPoint(Input.mousePosition).x - startPos.x;
            float posY = cam.ScreenToWorldPoint(Input.mousePosition).y - startPos.y;
            targetPos = new Vector2(transform.position.x - posX, transform.position.y - posY);
            transform.position = new Vector3(Mathf.Lerp(transform.position.x, targetPos.x, speed * Time.deltaTime), Mathf.Lerp(transform.position.y, targetPos.y, speed * Time.deltaTime), transform.position.z);
        }
        

        if (Input.GetKey(KeyCode.W))      
            transform.position = new Vector3(transform.position.x, transform.position.y + speed * Time.deltaTime * wasdSpeed, transform.position.z);
        if (Input.GetKey(KeyCode.S))
            transform.position = new Vector3(transform.position.x, transform.position.y - speed * Time.deltaTime * wasdSpeed, transform.position.z);
        if (Input.GetKey(KeyCode.A))
            transform.position = new Vector3(transform.position.x - speed * Time.deltaTime * wasdSpeed, transform.position.y , transform.position.z);
        if (Input.GetKey(KeyCode.D))
            transform.position = new Vector3(transform.position.x + speed * Time.deltaTime * wasdSpeed, transform.position.y, transform.position.z);
    }
}

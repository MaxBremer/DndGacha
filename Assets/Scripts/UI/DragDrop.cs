using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragDrop : MonoBehaviour
{
    public GameObject particlesObj;
    public bool IsDragging = false;

    private ParticleSystem _myDragParticles;

    private const float DRAG_SPEED_MULT = 10f;
    private const float DRAG_LOOK_INCR = 5f;

    private float _dist;
    private Vector3 _offset;


    // Start is called before the first frame update
    void Start()
    {
        _myDragParticles = particlesObj.GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsDragging)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            Vector3 startPos = transform.position;
            Vector3 endPos = ray.GetPoint(_dist);
            endPos.z = startPos.z; // Ensure the z-value remains constant
            Vector3 endPosLook = new Vector3(endPos.x, endPos.y, endPos.z + DRAG_LOOK_INCR);

            transform.position = Vector3.Lerp(startPos, endPos, Time.deltaTime * DRAG_SPEED_MULT);

            Vector3 relPos = endPosLook - transform.position;
            transform.rotation = Quaternion.LookRotation(relPos, Vector3.up);
        }
    }

    private void OnMouseDown()
    {
        StartDrag();
    }

    private void OnMouseUp()
    {
        StopDrag();
    }

    private void StartDrag()
    {
        //Debug.Log("Starting drag");
        IsDragging = true;
        _dist = Vector3.Distance(transform.position, Camera.main.transform.position);
        _offset = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, _dist));

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        particlesObj.transform.position = ray.GetPoint(_dist);
        _myDragParticles.Play();
    }

    private void StopDrag()
    {
        //Debug.Log("Stopping drag");
        IsDragging = false;
        transform.rotation = Quaternion.LookRotation(Vector3.forward, Vector3.up);
        _myDragParticles.Stop();
    }
}

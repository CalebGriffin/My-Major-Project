using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class CineCam : MonoBehaviour
{
    public Transform target;
    private float height = 25f;
    private float radius = 45f;
    private float rotationSpeed = 0.35f;

    private float startTime;

    private bool isMoving = false;
    private bool checkPos = false;

    private Vector3 startPos;

    void Start()
    {
        transform.LookAt(target);

        Move();
    }

    void Update()
    {
        transform.LookAt(target);

        if (isMoving)
        {
            Move();

            if (checkPos && Vector3.Distance(transform.position, startPos) < 0.1f)
            {
                isMoving = false;
                checkPos = false;
            }
        }

        transform.LookAt(target);
    }

    void Move()
    {
        // Do moving stuff
        float x = Mathf.Cos((Time.time - startTime) * rotationSpeed) * radius;
        float z = Mathf.Sin((Time.time - startTime) * rotationSpeed) * radius;

        transform.position = new Vector3(target.position.x + x, target.position.y + height, target.position.z + z);
    }

    [Button]
    public void StartCineCam()
    {
        startPos = transform.position;
        isMoving = true;
        startTime = Time.time;
        StartCoroutine(Wait());
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(.1f);
        checkPos = true;
    }

}

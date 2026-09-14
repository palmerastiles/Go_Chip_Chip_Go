using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlataformaMovil : MonoBehaviour
{
    public Transform PointA;
    public Transform PointB;
    public bool GoingUp;


    // Update is called once per frame
    void Update()
    {
        Vector3 wantedposition = Vector3.zero;

        if (GoingUp)
            wantedposition = PointA.position;
        else
            wantedposition = PointB.position;

        Vector3 direction = (wantedposition - transform.position);

        transform.position += direction.normalized * Time.deltaTime;

        if (direction.magnitude < 0.1)
            GoingUp = !GoingUp;
    }
}

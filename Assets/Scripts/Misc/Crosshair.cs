using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // ocultar cursor
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        // this hace referencia al gameobject para el cual se referencia el script
        this.transform.position = Input.mousePosition;
    }
}

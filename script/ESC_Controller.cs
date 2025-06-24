using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ESC_Controller : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    }
}

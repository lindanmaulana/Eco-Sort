using UnityEngine;

public class Test: MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Layar diklik pada posisi: " + Input.mousePosition);
        }
    }
}
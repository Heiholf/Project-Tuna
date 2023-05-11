using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddConsoleEntry : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        ConsoleHandler.Instance.AddConsoleEntry(new ConsoleEntry(System.DateTime.Now, "Test"));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

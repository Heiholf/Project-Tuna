using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTraveler : MonoBehaviour
{
   
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    
}

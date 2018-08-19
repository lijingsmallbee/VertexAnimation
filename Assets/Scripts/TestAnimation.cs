using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAnimation : MonoBehaviour {

	// Use this for initialization
	void Start () 
    {
        var anim = GetComponent<VertexAnimation>();
        if (Random.Range(0, 100) < 50)
        {
            anim.PlayAnimation("Assets/Footman_Blue/Footman_Blue_Attack01.bytes",100);
        }
        else
        {
            anim.PlayAnimation("Assets/Footman_Blue/Footman_Blue_Attack02.bytes",200);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

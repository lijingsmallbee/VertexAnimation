using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAnimation : MonoBehaviour {

	// Use this for initialization
	void Start () 
    {
        var anim = GetComponent<VertexAnimation>();
        anim.PlayAnimation("Assets/Footman_Blue/Footman_Blue_Attack01.bytes");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

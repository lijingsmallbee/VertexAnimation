using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class DemoLoader 
{
    static private DemoLoader _instance = null;
    static public DemoLoader Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = new DemoLoader();
            }
            return _instance;
        }
    }
	
    public byte[] LoadBytes(string path)
    {
        var obj = Resources.Load<TextAsset>(path);
        return obj.bytes;
    }

    public Material LoadMaterial(string path)
    {
        var obj = Resources.Load<Material>(path);
        return obj;
    }
}

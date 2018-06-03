using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class AnimationDataManager
{
    Dictionary<string, VertexAnimationData> _allAnimationData = new Dictionary<string, VertexAnimationData>();
    static AnimationDataManager _Instance = null;
    static public AnimationDataManager Instance
    {
        get
        {
            if(_Instance == null)
            {
                _Instance = new AnimationDataManager();
            }
            return _Instance;
        }
    }
    // Use this for initialization
    public VertexAnimationData GetAnimationData(string animName)
    {
        VertexAnimationData data = null;
        if(!_allAnimationData.TryGetValue(animName, out data))
        {
            var bytes = DemoLoader.Instance.LoadBytes(animName);
            data = new VertexAnimationData();
            data.Init(bytes);
            _allAnimationData.Add(animName, data);
            return data;
        }
        return data;

    }
}

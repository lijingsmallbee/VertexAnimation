using UnityEngine;
using System.Collections;
//动画播放实例类，记录一次播放的运行时参数
public class VertexAnimationState
{
    public VertexAnimationState(VertexAnimationData data)
    {
        _clipData = data;
    }
    private float _startTime = 0f;
    private VertexAnimationData _clipData = null;
    public void Play()
    {
        
    }
    // Update is called once per frame
    public void Update()
    {

    }
}

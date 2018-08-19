using UnityEngine;
using System.Collections;
public class AnimPlayContext
{
    public Renderer renderer;
    public Mesh originalMesh;
}
//动画播放实例类，记录一次播放的运行时参数
public class VertexAnimationState
{
    private AnimPlayContext _playContext = null;
    private MaterialPropertyBlock _materialBlock = new MaterialPropertyBlock();
    public VertexAnimationState(VertexAnimationData data)
    {
        _clipData = data;
    }

    public void Init()
    {

    }
    private float _startTime = 0f;
    private VertexAnimationData _clipData = null;
    public void Play(AnimPlayContext context)
    {
        _playContext = context;
        _startTime = Time.time;
        if (_clipData.AnimationMode == VertexAnimationMode.texture)
        {
            var render = _playContext.renderer;
            render.GetPropertyBlock(_materialBlock);
            _materialBlock.SetFloat("_AnimLen", _clipData.Length);
            _materialBlock.SetFloat("_AnimStart", Time.time);
            _materialBlock.SetFloat("_AnimLoop", 1);
            _materialBlock.SetTexture("_AnimMap", _clipData.Texture);
            render.SetPropertyBlock(_materialBlock);
        }
        else
        {
            var mesh = _clipData.GetFrameMesh(_playContext.originalMesh,0);
            var filter = _playContext.renderer.GetComponent<MeshFilter>();
            filter.mesh = mesh;
        }
    }

    // Update is called once per frame
    public void Update()
    {
        if(_clipData.AnimationMode == VertexAnimationMode.mesh)
        {
            float passTime = Time.time - _startTime;
            var frame = Mathf.CeilToInt(passTime / _clipData.FrameTime);
            var cur = frame % _clipData.FrameCount;
            var mesh = _clipData.GetFrameMesh(_playContext.originalMesh, cur);
            var filter = _playContext.renderer.GetComponent<MeshFilter>();
            filter.mesh = mesh;
        }
        
    }

}

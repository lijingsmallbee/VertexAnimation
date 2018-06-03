using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//动画数据，相当于unity的 animationclip
public class VertexAnimationData
{
    private Texture2D _texture;
    private List<Mesh> _meshFrames = null;

    public void Init(byte[] data)
    {
        if(SystemInfo.graphicsDeviceType <= UnityEngine.Rendering.GraphicsDeviceType.OpenGLES2)
        {
            //use nesh mode
        }
        else
        {
            //use texture mode
        }
    }
}

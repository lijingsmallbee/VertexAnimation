using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
//动画数据，相当于unity的 animationclip
public enum VertexAnimationMode
{
    mesh,
    texture,
}
public class VertexAnimationData
{
    private Texture2D _texture;
    private List<Mesh> _meshFrames = null;
    private VertexAnimationMode _mode = VertexAnimationMode.texture;
    private float _animationLen;
    public void Init(byte[] data)
    {
        if(SystemInfo.graphicsDeviceType <= UnityEngine.Rendering.GraphicsDeviceType.OpenGLES2)
        {
            //use nesh mode
            _mode = VertexAnimationMode.texture;
            //use texture mode
            if (data != null)
            {
                MemoryStream memory = new MemoryStream(data);
                BinaryReader reader = new BinaryReader(memory);
                int width = reader.ReadInt32();
                int height = reader.ReadInt32();
                float animLength = reader.ReadSingle();
                var texture = new Texture2D(width, height, TextureFormat.RGBAHalf, false);
                for (int j = 0; j < height; ++j)
                {
                    for (int i = 0; i < width; ++i)
                    {
                        var shortX = reader.ReadInt16();
                        var shortY = reader.ReadInt16();
                        var shortZ = reader.ReadInt16();
                        var color = new Color(shortX / 1000f, shortY / 1000f, shortZ / 1000f, 1f);
                        texture.SetPixel(i, j, color);
                    }
                }
                texture.Apply(false, true);
                _texture = texture;
                _animationLen = animLength;
            }
        }
        else
        {
            _mode = VertexAnimationMode.texture;
            //use texture mode
            if (data != null)
            {
                MemoryStream memory = new MemoryStream(data);
                BinaryReader reader = new BinaryReader(memory);
                int width = reader.ReadInt32();
                int height = reader.ReadInt32();
                float animLength = reader.ReadSingle();
                var texture = new Texture2D(width, height, TextureFormat.RGBAHalf, false);
                for (int j = 0; j < height; ++j)
                {
                    for (int i = 0; i < width; ++i)
                    {
                        var shortX = reader.ReadInt16();
                        var shortY = reader.ReadInt16();
                        var shortZ = reader.ReadInt16();
                        var color = new Color(shortX / 1000f, shortY / 1000f, shortZ / 1000f, 1f);
                        texture.SetPixel(i, j, color);
                    }
                }
                texture.Apply(false, true);
                _texture = texture;
                _animationLen = animLength;
            }
        }
    }

    public VertexAnimationMode AnimationMode
    {
        get { return _mode; }
    }

    public Texture2D Texture
    {
        get { return _texture; }
    }

    public Mesh GetFrame(int frameIndex)
    {
        if(frameIndex >= 0 && frameIndex < _meshFrames.Count)
        {
            return _meshFrames[frameIndex];
        }
        return null;
    }

    public int FrameCount
    {
        get { return _meshFrames.Count; }
    }

    public float Length
    {
        get { return _animationLen; }
    }
}

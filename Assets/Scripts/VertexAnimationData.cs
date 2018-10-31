using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
//动画数据，相当于unity的 animationclip
public enum VertexAnimationMode
{
    mesh,
    texture,
}

public class FrameData
{
    public List<Int16> frame = null;
}
public class VertexAnimationData
{
    private Texture2D _texture;
    private List<FrameData> _vertexFrames = null;
    private List<Mesh> _meshFrames = null;
    private VertexAnimationMode _mode = VertexAnimationMode.texture;
    private float _animationLen;
    private float _frameTime;
    private int _frameCount;

    private List<Vector3> _tmpList = null;
    public void Init(byte[] data)
    {
        //如果要测试mesh模式，就把关于editor的判断去掉
   //     if(!Application.isEditor && SystemInfo.graphicsDeviceType <= UnityEngine.Rendering.GraphicsDeviceType.OpenGLES2)
        if(Application.isEditor)
        {
            //use nesh mode
            _mode = VertexAnimationMode.mesh;
            //use texture mode
            if (data != null)
            {
                MemoryStream memory = new MemoryStream(data);
                BinaryReader reader = new BinaryReader(memory);
                int width = reader.ReadInt32();
                int height = reader.ReadInt32();
                float animLength = reader.ReadSingle();
                var realHeight = height >> 1 + 1;
                _vertexFrames = new List<FrameData>(realHeight);
                _meshFrames = new List<Mesh>(realHeight);
                _tmpList = new List<Vector3>(width);
                var startJ = 0;
                for (int j = 0; j < height; ++j)
                {
                    bool needAdd = false;
                    if (j % 2 == 0)
                    {
                        needAdd = true;
                        var frameData = new FrameData();
                        _vertexFrames.Add(frameData);
                        _meshFrames.Add(null);
                        frameData.frame = new List<short>(width * 3);
                    }
                        for (int i = 0; i < width; ++i)
                    {
                        var shortX = reader.ReadInt16();
                        var shortY = reader.ReadInt16();
                        var shortZ = reader.ReadInt16();
                     //   var color = new Vector3(shortX / 1000f, shortY / 1000f, shortZ / 1000f);
                        if(needAdd)
                        {
                            _vertexFrames[startJ].frame.Add(shortX);
                            _vertexFrames[startJ].frame.Add(shortY);
                            _vertexFrames[startJ].frame.Add(shortZ);
                        }
                        
                    }
                        if(needAdd)
                    {
                        startJ++;
                    }
                    
                }
                _animationLen = animLength;
                _frameCount = _vertexFrames.Count;
                _frameTime = _animationLen / _frameCount;
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

    public Mesh GetFrameMesh(Mesh original,int index)
    {
        if(index >=0 && index < _frameCount)
        {
            if(_meshFrames[index] == null)
            {
                _meshFrames[index] = new Mesh();
                var top = original.GetTopology(0);
                _tmpList.Clear();
                for(int i=0;i<original.vertexCount;++i)
                {
                    var frame = _vertexFrames[index].frame;
                    Vector3 vec = new Vector3(frame[i * 3] / 1000f, frame[i * 3 + 1] / 1000f, frame[i * 3 + 2] / 1000f);
                    _tmpList.Add(vec);
                }
                _meshFrames[index].SetVertices(_tmpList);
                _meshFrames[index].colors = original.colors;
                _meshFrames[index].uv = original.uv;
                _meshFrames[index].SetIndices(original.GetIndices(0), top, 0);
                _meshFrames[index].UploadMeshData(true);
            }
            return _meshFrames[index];    
        }
        return null;
    }

    public VertexAnimationMode AnimationMode
    {
        get { return _mode; }
    }

    public Texture2D Texture
    {
        get { return _texture; }
    }

    public float Length
    {
        get { return _animationLen; }
    }

    public float FrameTime
    {
        get { return _frameTime; }
    }

    public int FrameCount
    {
        get { return _frameCount; }
    }
}

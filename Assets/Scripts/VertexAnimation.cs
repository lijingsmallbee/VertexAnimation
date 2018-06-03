using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class VertexAnimation : MonoBehaviour 
{
    Dictionary<string, BaseAnimationData> _allAnimation = new Dictionary<string, BaseAnimationData>();
	// Use this for initialization
	public void PlayAnimation(string animName)
    {
        var animData = AnimationDataManager.Instance.GetAnimationData(animName);
        if(animData != null)
        {
            var state = new VertexAnimationState(animData);
        }
        var bytes = DemoLoader.Instance.LoadBytes(animName);
        if(bytes != null)
        {
            MemoryStream memory = new MemoryStream(bytes);
            BinaryReader reader = new BinaryReader(memory);
            int width = reader.ReadInt32();
            int height = reader.ReadInt32();
            float animLength = reader.ReadSingle();
            var texture = new Texture2D(width, height, TextureFormat.RGBAHalf, false);
            for (int j = 0; j < height;++j)
            {
                for (int i = 0; i < width;++i)
                {
                    var shortX = reader.ReadInt16();
                    var shortY = reader.ReadInt16();
                    var shortZ = reader.ReadInt16();
                    var color = new Color(shortX / 1000f, shortY / 1000f, shortZ / 1000f,1f);
                    texture.SetPixel(i, j, color);
                }
            }
            texture.Apply(false, true);
            var render = GetComponent<Renderer>();
            render.material.SetTexture("_AnimMap",texture);
            render.material.SetFloat("_AnimLen",animLength);
        }

    }
}

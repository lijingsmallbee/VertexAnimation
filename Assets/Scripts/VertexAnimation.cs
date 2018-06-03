using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class VertexAnimation : MonoBehaviour 
{
    MeshRenderer _renderer = null;
    Dictionary<string, VertexAnimationState> _allStates = new Dictionary<string, VertexAnimationState>(4);
	// Use this for initialization
	public void PlayAnimation(string animName)
    {
        if(_renderer == null)
        {
            _renderer = GetComponentInChildren<MeshRenderer>(true);
        }
        VertexAnimationState state = null;
        if(_allStates.TryGetValue(animName,out state))
        {
            state.Play(_renderer);
        }
        else
        {
            var animData = AnimationDataManager.Instance.GetAnimationData(animName);
            if (animData != null)
            {
                state = new VertexAnimationState(animData);
                _allStates.Add(animName,state);
                state.Play(_renderer);
            }
        }

        if(state == null)
        {
            Debug.LogFormat("play animation {0} failed", animName);
        }
    }
}

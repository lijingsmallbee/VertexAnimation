using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class VertexAnimation : MonoBehaviour 
{
    AnimPlayContext _context = null;
    Dictionary<string, VertexAnimationState> _allStates = new Dictionary<string, VertexAnimationState>(4);
    VertexAnimationState _currentState = null;
	// Use this for initialization
	public void PlayAnimation(string animName)
    {
        if(_context == null)
        {
            _context = new AnimPlayContext();
            _context.renderer = GetComponentInChildren<Renderer>(true);
            _context.originalMesh = _context.renderer.GetComponent<MeshFilter>().mesh;
        }
        VertexAnimationState state = null;
        if(_allStates.TryGetValue(animName,out state))
        {
            state.Play(_context);
            _currentState = state;
        }
        else
        {
            var animData = AnimationDataManager.Instance.GetAnimationData(animName);
            if (animData != null)
            {
                state = new VertexAnimationState(animData);
                _allStates.Add(animName,state);
                state.Play(_context);
                _currentState = state;
            }
        }

        if(state == null)
        {
            Debug.LogFormat("play animation {0} failed", animName);
        }
    }

    private void Update()
    {
        if(_currentState != null)
        {
            _currentState.Update();
        }
    }
}

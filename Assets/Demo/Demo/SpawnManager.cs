﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// demo使用，分配大量的活动的角色
/// </summary>
public class SpawnManager : MonoBehaviour {

    #region 字段

    public GameObject spawnPrefab;
    public int gridWidth;
    public int gridHeight;

    #endregion


    #region 方法


    void Start()
    {
        MaterialPropertyBlock block = new MaterialPropertyBlock();
        for(int i = 0; i < gridWidth; i++)
        {
            for(int j = 0; j < gridHeight; j++)
            {
                var obj = Instantiate<GameObject>(spawnPrefab, new Vector3(i * 2, 0, j * 2), Quaternion.identity);
                var render = obj.GetComponentInChildren<Renderer>(true);
                if(render)
                {
                    block.SetFloat("_AnimStart", Random.Range(0, 100));
                    render.SetPropertyBlock(block);
                }
            }
        }
    }


    #endregion

}

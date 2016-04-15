// VacuumShaders 2014
// https://www.facebook.com/VacuumShaders

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using VacuumShaders.TheAmazingWireframeShader;


[AddComponentMenu("VacuumShaders/The Amazing Wireframe Generator")]
public class TheAmazingWireframeGenerator : MonoBehaviour
{
    //////////////////////////////////////////////////////////////////////////////
    //                                                                          // 
    //Variables                                                                 //                
    //                                                                          //               
    //////////////////////////////////////////////////////////////////////////////
    public WIRE_INSIDE useBuffer;

    //////////////////////////////////////////////////////////////////////////////
    //                                                                          // 
    //Unity Functions                                                           //                
    //                                                                          //               
    //////////////////////////////////////////////////////////////////////////////
    void Awake()
    { 
        Generate(useBuffer);
    }
        
    //////////////////////////////////////////////////////////////////////////////
    //                                                                          // 
    //Custom Functions                                                          //                
    //                                                                          //               
    //////////////////////////////////////////////////////////////////////////////
    public void Generate(WIRE_INSIDE _wireInside)
    {
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        if (meshFilter != null)
        {
            meshFilter.mesh = WireframeManager.GetWire(meshFilter.sharedMesh, _wireInside);
        }
        else
        {
            SkinnedMeshRenderer skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();
            if (skinnedMeshRenderer != null)
            {
                skinnedMeshRenderer.sharedMesh = WireframeManager.GetWire(skinnedMeshRenderer.sharedMesh, _wireInside);
            }                        
        }
    }
}

//Note
//Script generates wireframe by using WireframeManager.GetWire() function. 
//WireframeManager generates and saves instance ID of the meshes whose wireframes has been generated, to avoid wireframe calculation for meshes with the same ID.
//WireframeManager.RemoveMesh() - removes mesh ID from the WireframeManager

//For direct wireframe calculation use - WireframeGenerator.Generate()




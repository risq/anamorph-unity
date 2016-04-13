using UnityEngine;
using System.Collections;

using VacuumShaders.MeshMaterializer;

public class Runtime_Materializer_Example : MonoBehaviour 
{
    //////////////////////////////////////////////////////////////////////////////
    //                                                                          // 
    //Variables                                                                 //                
    //                                                                          //               
    //////////////////////////////////////////////////////////////////////////////

    
    
    //////////////////////////////////////////////////////////////////////////////
    //                                                                          // 
    //Unity Functions                                                           //                
    //                                                                          //               
    //////////////////////////////////////////////////////////////////////////////
	// Use this for initialization
	void Start () 
    {
        //Build info
         MM_INFO[] buidInfo = new MM_INFO[]{MM_INFO.Ok};
         string[] buildInfoFull = new string[] { "" };

        //MMdata
        MMData_SurfaceInfo meshInfo = new MMData_SurfaceInfo(MM_SURFACE_TYPE.Original, true, true, Color.green);
        MMData_MeshMainTexture mainTexture = new MMData_MeshMainTexture(true, "_MainTex", MM_TEXTURE_SAMPLING_TYPE.FlatSmooth);
        MMData_MeshDisplace dispalce = new MMData_MeshDisplace(true, "_MainTex", MM_DISPLACE_READ_CHANNEL.Grayscale, 1.0f, 2.0f, MM_DISPLACE_SAVE_TYPE.DisplaceVertex, MM_SAVE_CHANNEL.RGB);
        MMData_AmbientOcclusion ao = new MMData_AmbientOcclusion(true, MM_COLOR_SAMPLING_TYPE.Smooth, MM_SAVE_CHANNEL.RGB, Color.black, 64, 1.0f, 1.0f, 1.0f);


        //Generating flat mesh with displace and AO
        Mesh flatMesh = MMGenerator.MaterializeMesh(GetComponent<Renderer>(), ref buidInfo, ref buildInfoFull, meshInfo, mainTexture, dispalce, ao);

        //Print build info
        foreach (var str in buildInfoFull)
        {
            Debug.Log(str);
        }

        //Assign new mesh
        if(flatMesh != null)
            GetComponent<MeshFilter>().mesh = flatMesh;

        //Disabling texture inside material, rendering only vertex colors
        GetComponent<Renderer>().material.DisableKeyword("V_MM_TEXTURE_AND_COLOR_ON");
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}

    //////////////////////////////////////////////////////////////////////////////
    //                                                                          // 
    //Custom Functions                                                          //                
    //                                                                          //               
    //////////////////////////////////////////////////////////////////////////////

}

#ifndef VACUUM_FRAGMENTUM_CG_MACROS_INCLUDED
#define VACUUM_FRAGMENTUM_CG_MACROS_INCLUDED


//************************************************************************
//Macros
//************************************************************************
#ifdef SHADER_API_D3D11
	#define V_FR_INDEX (round(v.texcoord1.x * V_FR_Devider))
	#define V_FR_INDEX_LM (round(v.texcoord1.y * V_FR_Devider))
#endif

#define _2PI 6.28318

#ifndef TRANSFORM_TEX
#define TRANSFORM_TEX(tex,name) (tex.xy * name##_ST.xy + name##_ST.zw)
#endif

#endif
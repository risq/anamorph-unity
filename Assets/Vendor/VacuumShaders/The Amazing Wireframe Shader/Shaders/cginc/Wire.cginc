#ifndef VACUUM_WIRE_CGINC
#define VACUUM_WIRE_CGINC

#include "../cginc/Wire_Variables.cginc"
#include "../cginc/Wire_Functions.cginc"

#ifdef V_WIRE_SURFACE
#include "../cginc/Wire_Surface.cginc"
#else
#include "../cginc/Wire_Vertex.cginc"
#endif

#endif	//cginc
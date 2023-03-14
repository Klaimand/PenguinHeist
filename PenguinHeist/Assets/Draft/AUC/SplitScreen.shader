Shader "Mask/SplitScreen" 
{
	//Simple depthmask shader 
	SubShader {
	    Tags {"Queue" = "AlphaTest" "IgnoreProjector" = "True" "RenderType" = "TransparentCutout"}
	    ColorMask 0
		//ZWrite On
        Pass {}
	}
}
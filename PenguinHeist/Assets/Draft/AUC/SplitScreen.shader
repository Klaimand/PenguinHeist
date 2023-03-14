Shader "Mask/SplitScreen" 
{
	//Simple depthmask shader 
	SubShader {
	    Tags { "Queue" = "Background"}
	    ColorMask 0
		//ZWrite On
        Pass {}
	}
}
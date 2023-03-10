Shader "Custom/Mask"
{
	SubShader
	{
		Tags{"Queue" = "Transparent+1"}
		/*ColorMask 0
		ZWrite On*/
		Pass{
	Blend Zero one
	}
	}
}

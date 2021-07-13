Shader "Custom/SingleColor"
{
	Properties
	{
	  _Color("Color", Color) = (1,1,1)
	}
	SubShader {
		Tags { "Queue" = "Geometry+1" }

		ZTest Always
		Color[_Color]
		ZWrite On

		// Во время прохода ничего не делаем 
		Pass {}

	}

}

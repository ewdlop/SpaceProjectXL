Shader "Custom/Wrap"
{
	Properties
	{
		_MainTex("Sprite Texture", 2D) = "white" {}
	_Color("Tint", Color) = (1,1,1,1)
		[MaterialToggle] PixelSnap("Pixel snap", Float) = 0
		_RendererColor("RendererColor", Color) = (1,1,1,1)
		_Flip("Flip", Vector) = (1,1,1,1)
		_AlphaTex("External Alpha", 2D) = "white" {}
	_EnableExternalAlpha("Enable External Alpha", Float) = 0
		_a("a",Range(0, 10)) = 1
		_b("b",Range(0, 10)) = 1
		_shipX("shipX",Float) = 0
		_shipY("shipY",Float) = 0
		_holeX("holeX",Float) = 0
		_holeY("holeY",Float) = 0
		_shipSizeX("shipSizeX",Float) = 0
		_shipSizeY("shipSizeY",Float) = 0


	}

		SubShader
	{
		Tags
	{
		"Queue" = "Transparent"
		"IgnoreProjector" = "True"
		"RenderType" = "Transparent"
		"PreviewType" = "Plane"
		"CanUseSpriteAtlas" = "True"
	}

		Cull Off
		Lighting Off
		ZWrite Off
		Blend One OneMinusSrcAlpha

		CGPROGRAM
#pragma surface surf Lambert vertex:vert nofog nolightmap nodynlightmap keepalpha noinstancing
#pragma multi_compile _ PIXELSNAP_ON
#pragma multi_compile _ ETC1_EXTERNAL_ALPHA
#include "UnitySprites.cginc"

		struct Input
	{
		float2 uv_MainTex;
		fixed4 color;
	};
	float _a;
	float _b;
	float _shipX;
	float _shipY;
	float _holeX;
	float _holeY;
	float _shipSizeX;
	float _shipSizeY;

	void vert(inout appdata_full v, out Input o)
	{

		v.vertex.xy *= _Flip.xy;
		//float r = sqrt(pow(v.vertex.x, 2) + pow(v.vertex.y, 2));
		//float angle = atan2((_shipSizeY*v.vertex.y+_shipY)-_holeY, (_shipSizeX*v.vertex.x + _shipX) - _holeX);
		float angle = atan2((_shipSizeY*v.vertex.y + _shipY) - _holeY, (_shipSizeX*v.vertex.x + _shipX) - _holeX);
		if (angle < 0) {

			angle += 2 * 3.14159265358979323846264338327950;
		}
		//angle += 20/180* 3.14159265358979323846264338327950;
		//float r = sqrt(pow((_shipSizeY*v.vertex.y + _shipY) - _holeY,2) + pow((_shipSizeX*v.vertex.x + _shipX) - _holeX, 2));
		float r = _a;
		//float r = sqrt(pow((v.vertex.y + _shipY) - _holeY, 2) + pow((v.vertex.x + _shipX) - _holeX, 2));
		//float finalangle = _a*exp(-r / 5 + angle);

		//v.vertex = float4(-1*(_shipSizeX*v.vertex.x + _shipX - _holeX)+r*cos(finalangle), -1*(_shipSizeY*v.vertex.y + _shipY)+r*sin(finalangle), 0, 0);
		//v.vertex = float4(-1*(_shipX - _holeX) + r*cos(finalangle), -1*(_shipY-_holeY) + r*sin(finalangle), 0, 0);
		//float r = _a;
		v.vertex = float4(-1 * (_shipSizeX*v.vertex.x + _shipX - _holeX) + r*cos(angle), -1 * (_shipSizeY*v.vertex.y + _shipY - _holeY) + r*sin(angle),4,0);

#if defined(PIXELSNAP_ON)
		v.vertex = UnityPixelSnap(v.vertex);
#endif

		UNITY_INITIALIZE_OUTPUT(Input, o);
		o.color = v.color * _Color * _RendererColor;
	}

	void surf(Input IN, inout SurfaceOutput o)
	{
		fixed4 c = SampleSpriteTexture(IN.uv_MainTex) * IN.color;
		o.Albedo = c.rgb * c.a;
		o.Alpha = c.a;
	}
	ENDCG
	}

		Fallback "Transparent/VertexLit"
}

Shader "Custom/GreenToTransparent"
{
Properties{
_MainTex("Base", 2D) = "white" {}
   _Threshold("Threshold", Range(0, 1)) = 0
_R("R",Range(0, 1))=0
_G("G",Range(0, 1)) = 0
_B("B",Range(0, 1)) = 0
}
SubShader{
Pass{
Tags{ "Queue" = "Transparent" "RenderType" = "Transparent" }
Blend SrcAlpha OneMinusSrcAlpha
Cull Off
CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#include "UnityCG.cginc"
struct v2f {
float4 pos : SV_POSITION;
float2 uv1 : TEXCOORD0;
};
sampler2D _MainTex;
float4 _MainTex_ST;
v2f vert(appdata_base v) {
v2f o;
o.pos = UnityObjectToClipPos(v.vertex);
o.uv1 = TRANSFORM_TEX(v.texcoord, _MainTex);
return o;
}
float _Threshold;
float _R;
float _G;
float _B;
fixed4 frag(v2f i) : COLOR{
fixed4 col1 = tex2D(_MainTex, i.uv1);
if (col1.g >_G&&col1.b<_B&&col1.r<_R) {
     col1.a = 0;//材质的绿色大到一定程度，并且蓝色和红色小到一定程度，就把该部分的材质的透明度设置为0  
}
//fixed4 val = ceil(saturate(col1.g - col1.r - _Threshold)) * ceil(saturate(col1.g - col1.b - _Threshold));
return col1;
}
ENDCG
}
}
}
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Demo/SpriteShader"
 {  
     Properties
     {
 	   	[PerRendererData]_MainTex ("Sprite Texture", 2D) = "white" {}    //   当前的Sprite图（添加[PerRendererData]后在属性面板中不可见） 
        _Color ("Alpha Color Key", Color) = (0,0,0,1)                               // 用于比较的基色(想过滤掉什么颜色，这个颜色就设置为那种颜色)
        _Range("Range",Range (0, 1.01))=0.1          							// 决定抠图范围的域                                
        [MaterialToggle] PixelSnap ("Pixel snap", Float) = 0                     // 在属性面板中以按钮形式显示（在此Shader中没用到）
     }
     SubShader
     {
       //Sprite图一般均为透明贴图，需要做以下处理
         Tags 
         { 
         	"Queue"="Transparent" 
         	"IgnoreProjector"="True" 
         	"RenderType"="Transparent" 
         	"PreviewType"="Plane"
         	"CanUseSpriteAtlas"="True"
         }
 
         Pass
         {
           //Sprite图一般均为透明贴图，需要做以下处理
         	Cull Off
     		Lighting Off
     		ZWrite Off
     		Fog { Mode Off } 
     		Blend SrcAlpha OneMinusSrcAlpha    //Sprite图一般均为透明贴图，需要做以下处理
 
             CGPROGRAM
             #pragma vertex vert
             #pragma fragment frag
 
             sampler2D _MainTex;
             float4 _Color;
             half _Range;
             struct Vertex
             {
                 float4 vertex : POSITION;
                 float2 uv_MainTex : TEXCOORD0;
                 float2 uv2 : TEXCOORD1;
             };
 
             struct Fragment
             {
                 float4 vertex : POSITION;
                 float2 uv_MainTex : TEXCOORD0;
             };
 
             Fragment vert(Vertex v)
             {
                 Fragment o;
 
                 o.vertex = UnityObjectToClipPos(v.vertex);
                 o.uv_MainTex = v.uv_MainTex;
 
                 return o;
             }
 
             float4 frag(Fragment IN) : COLOR
             {
                 float4 o = float4(1, 1, 1, 1);
 
                 half4 c = tex2D (_MainTex, IN.uv_MainTex);
                 o.rgb = c.rgb;
                 //使用当前像素颜色与基色相减，然后与域相比较以决定是否将其显示出来
                 if(abs(c.r-_Color.r)<_Range && abs(c.g-_Color.g)<_Range && abs(c.b-_Color.b)<_Range)
                 {
                     o.a = 0;
                 }
                 else
                 {
                     o.a = 1;
                 }
 
                 return o;
             }
 
             ENDCG
         }
     }
 }
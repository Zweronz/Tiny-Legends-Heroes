Shader "Triniti/Scene/COL_LM_2S" {
  Properties {
   _Color ("Main Color", Color) = (1,1,1,1)
   _MainTex ("MainTex", 2D) = "" {}
   _LightMap ("Lightmap (RGB)", 2D) = "white" {}
  }
  SubShader {
      LOD 200
      Tags { "RenderType" = "Opaque" }
  CGPROGRAM
  #pragma surface surf Lambert nodynlightmap
  struct Input {
    float2 uv_MainTex;
    float2 uv2_LightMap;
  };
  sampler2D _MainTex;
  sampler2D _LightMap;
  fixed4 _Color;
  void surf (Input IN, inout SurfaceOutput o)
  {
    o.Albedo = tex2D (_MainTex, IN.uv_MainTex).rgb * (_Color * 0.05);
    half4 lm = tex2D (_LightMap, IN.uv2_LightMap) * 20;
    o.Emission = lm.rgb*o.Albedo.rgb;
    o.Alpha = lm.a * _Color.a;
  }
  ENDCG
  }
  }
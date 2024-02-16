Shader "Triniti/Character/COL_OFFEST_ON" {
Properties {
 _Color ("Main Color", Color) = (1,1,1,1)
 _MainTex ("MainTex(RGB)", 2D) = "" {}
}
SubShader { 
 Tags { "QUEUE"="Geometry" }
 Pass {
  Tags { "QUEUE"="Geometry" }
  Color [_Color]
  Offset -1, -1
  SetTexture [_MainTex] { combine texture * primary }
 }
}
}
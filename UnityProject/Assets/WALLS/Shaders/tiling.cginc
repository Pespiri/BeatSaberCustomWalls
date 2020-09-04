float2 tileUV(float2 inputUV, float _ScaleMult, float3 normal, float4x4 unity_ObjectToWorld){
	float2 returnedUV;

	float3 scale = float3(half3(
		length(unity_ObjectToWorld._m00_m10_m20),
		length(unity_ObjectToWorld._m01_m11_m21),
		length(unity_ObjectToWorld._m02_m12_m22)
	));

	scale *= _ScaleMult * 1.73;

	float zoffset = (scale.z - floor(scale.z))/2;
	float yoffset = (scale.y - floor(scale.y))/2;

	float xfactor = dot(normal,float3(1,0,0));
	float yfactor = dot(normal,float3(0,1,0));
	float zfactor = dot(normal,float3(0,0,1));

	float2 flippedUV = abs(1 - inputUV);
	returnedUV = flippedUV * scale.xz;
	returnedUV = lerp(returnedUV, inputUV * scale.zy,abs(xfactor));
	returnedUV = lerp(returnedUV, flippedUV * scale.xy,abs(zfactor));
	returnedUV.x += (zoffset - (sign(xfactor) * zoffset)) * sign(xfactor);
	returnedUV.y += (zoffset - (sign(yfactor) * zoffset)) * sign(yfactor) -
	(yoffset + (sign(zfactor) * yoffset)) * sign(zfactor);

	return returnedUV;
}
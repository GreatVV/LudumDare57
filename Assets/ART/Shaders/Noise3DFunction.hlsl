#include "BitangentNoise_v0.hlsl"

void Noise3D_float(float3 In, float Pow, out float Out)
{
    Out = BitangentNoise3D(In) * Pow + (Pow * 3);
}
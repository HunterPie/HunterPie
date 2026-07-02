// ==========================================================
// WPF Constants (Registers)
// ==========================================================
sampler2D implicitInput : register(s0);
float Time : register(c0);
float4 BaseColor : register(c1);
float4 CoreColor : register(c2);

// ==========================================================
// Procedural Noise Generation
// ==========================================================

// Pseudo-random number generator for ps_3_0
float rand(float2 co)
{
	return frac(sin(dot(co, float2(12.9898, 78.233))) * 43758.5453);
}

// Smooth noise interpolation
float smoothNoise(float2 p)
{
	float2 i = floor(p);
	float2 f = frac(p);
    
    // Quintic interpolation curve for smooth transitions
	float2 u = f * f * f * (f * (f * 6.0 - 15.0) + 10.0);

    // Bilinear interpolation of random corner values
	float a = rand(i);
	float b = rand(i + float2(1.0, 0.0));
	float c = rand(i + float2(0.0, 1.0));
	float d = rand(i + float2(1.0, 1.0));

	return lerp(lerp(a, b, u.x), lerp(c, d, u.x), u.y);
}

// Fractal Brownian Motion (fBm) - Stacks noise for "wispy" texture
float getProceduralPlasma(float2 uv)
{
	float scale = 20; // Global scale of the texture
	float val = 0.0;
    
    // Octave 1: Main flow
	val += 0.5000 * smoothNoise(uv * scale + float2(Time * -1.5, Time * 0.5));
    
    // Octave 2: Mid-level texture detail
	val += 0.500 * smoothNoise(uv * scale * 2.1 + float2(Time * -2.5, Time * 1.0));
    
    // Octave 3: Fine wisps and chaos
	val += 0.1250 * smoothNoise(uv * scale * 4.3 + float2(Time * -3.5, Time * 1.5));
    
    // Shift from [0, 1] range to roughly [-0.5, 0.5] so it centers on the bar
	return val - 0.4375;
}

// ==========================================================
// Main Pixel Shader
// ==========================================================

float4 main(float2 uv : TEXCOORD) : COLOR
{
    // 2. Generate textured displacement
    // The multiplier (0.3) controls how violently the plasma waves up and down
	float noiseOffset = getProceduralPlasma(uv) * 0.3;
    
    // 3. Calculate vertical distance from the erratic moving center line
	float dist = abs(uv.y - 0.5 + noiseOffset);

    // 4. The Core (Pure White) with non-linear bloom falloff
	float core = 0.1 / (dist + 0.005);
	core = saturate(pow(core, 1.5));

    // 5. The Outer Glow (Base Color) with a wider, softer spread
	float glow = 0.3 / (dist + 0.2);
	glow = saturate(glow);

    // 6. Left-to-Right highlight sweep
	float sweep = saturate(sin(uv.x * 4.0 - Time * 3.0));
	core = saturate(core + sweep * 0.15);

    // 7. Color Blending
	float4 finalColor = BaseColor * glow; // Aura
	finalColor = lerp(finalColor, CoreColor, core); // Blend in the hot core

    // 8. Alpha Handling (For glassy transparency and rounded WPF edges)
	float4 texColor = tex2D(implicitInput, uv);
    
    // Fade out top and bottom edges smoothly, bounded by the WPF element's alpha
	finalColor.a = saturate(glow + core) * texColor.a * BaseColor.a;

    // WPF expects pre-multiplied alpha for proper blending over other UI elements
	finalColor.rgb *= finalColor.a;

	return finalColor;
}
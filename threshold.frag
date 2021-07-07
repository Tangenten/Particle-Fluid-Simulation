uniform sampler2D texture;
uniform float threshold;

void main(void)
{

	vec4 texColor = texture2D(texture, gl_TexCoord[0].xy).rgba;
	float bright = 0.33333 * (texColor.r + texColor.g + texColor.b);
	if (bright > threshold) {
		float r = mix(0., 1, texColor.r * bright) * 6;
		float g = mix(0., 1, texColor.g * bright) * 6;
		float b = mix(0., 1, texColor.b * bright) * 6;
		float a = texColor.a;
		gl_FragColor = vec4(r, g, b, a);
	} else {
		gl_FragColor = vec4(0.0, 0.0, 0.0, 1.0);
	}


	/*
	vec4 texColor = texture2D(texture, gl_TexCoord[0].xy).rgba;
	float bright = 0.33333 * ((texColor.r + texColor.g + texColor.b) * texColor.a);
	float b = mix(0.0, 1.0, step(threshold, bright));
	gl_FragColor = vec4(vec3(b), col.a);
	*/
}

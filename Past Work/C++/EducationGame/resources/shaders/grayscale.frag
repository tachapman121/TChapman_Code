precision mediump float;
uniform sampler2D tex;

void main()
{
    vec4 texelColor = texture2D(tex, gl_TexCoord[0].xy);
    vec4 scaledColor = texelColor * vec4(0.3, 0.59, 0.11, 1.0);
    float luminance = scaledColor.r + scaledColor.g + scaledColor.b ;
    gl_FragColor = vec4( luminance, luminance, luminance, texelColor.a);
}

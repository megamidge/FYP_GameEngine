#version 330

uniform vec4 Colour;
uniform sampler2D sTexture;
uniform int textureSample = 0;

in vec3 v_FragPos;
in vec2 v_FragTexCoord;

out vec4 Color;

void main(){
	if(textureSample == 1){
		vec4 textureColour = texture2D(sTexture, v_FragTexCoord);
		Color = textureColour;
	}
	else{
		Color = Colour;
	}
}
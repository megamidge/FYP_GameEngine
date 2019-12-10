#version 330

uniform vec4 Colour;

in vec3 v_FragPos;
in vec3 v_FragNormal;

out vec4 Color;

void main(){
	Color = vec4(v_FragNormal, 1);
}
#version 330

uniform vec4 Colour;

in vec3 v_FragPos;
in vec4 Normal;

out vec4 Color;

void main(){
	Color = Colour;
}
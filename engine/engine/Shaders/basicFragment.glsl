#version 330

uniform vec4 Colour;

in vec3 v_FragPos;

out vec4 Color;

void main(){
	Color = Colour;
}
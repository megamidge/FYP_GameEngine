#version 330
layout (location = 0) in vec3 vPosition;

uniform mat4 ModelViewProjectionMat;
uniform mat4 ModelMat;

out vec3 v_FragPos;
void main() {
	gl_Position = ModelViewProjectionMat * vec4(vPosition, 1.0);
	v_FragPos = vec3(ModelMat * vec4(vPosition, 1.0));
}
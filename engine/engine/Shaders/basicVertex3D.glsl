#version 330
layout (location = 0) in vec3 vPosition;
layout (location = 1) in vec3 vNormal;
uniform mat4 ModelViewProjectionMat;
uniform mat4 ModelMat;

out vec3 v_FragPos;
out vec3 v_FragNormal;
void main() {
	gl_Position = ModelViewProjectionMat * vec4(vPosition, 1.0);
	v_FragPos = vec3(ModelMat * vec4(vPosition, 1.0));
	v_FragNormal = vec3(ModelMat * vec4(vNormal, 1.0));;
}
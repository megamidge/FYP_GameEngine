#version 330
layout (location = 0) in vec3 vPosition;
layout (location = 1) in vec2 vTexture;
uniform mat4 ModelViewProjectionMat;
uniform mat4 ModelMat;

out vec2 v_FragTexCoord;
out vec3 v_FragPos;
void main() {
	gl_Position = ModelViewProjectionMat * vec4(vPosition, 1.0);
	v_FragPos = vec3(ModelMat * vec4(vPosition, 1.0));
	v_FragTexCoord = vTexture;
}
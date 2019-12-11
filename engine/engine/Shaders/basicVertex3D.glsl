#version 330
layout (location = 0) in vec3 vPosition;
layout (location = 1) in vec3 vNormal;
layout (location = 2) in vec2 vTexture;

uniform mat4 ModelViewProjectionMat;
uniform mat4 ModelMat;

out vec2 v_FragTexCoord;
out vec3 v_FragPos;
out vec3 v_FragNormal;
void main() {
	gl_Position = ModelViewProjectionMat * vec4(vPosition, 1.0);
	v_FragPos = vec3(ModelMat * vec4(vPosition, 1.0));
	mat3 modelMat2 = mat3(ModelMat);
	v_FragNormal = modelMat2 * vNormal;
	v_FragTexCoord = vTexture;
}
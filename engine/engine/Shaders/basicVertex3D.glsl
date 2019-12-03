#version 330
layout (location = 0) in vec3 vPosition;
layout (location = 1) in vec3 vNormal;

uniform mat4 ProjectionMat;
uniform mat4 ModelMat;
uniform mat4 ViewMat;

out vec3 v_FragPos;
out vec4 Normal;
void main() {
	gl_Position = ModelMat * ViewMat * ProjectionMat * vec4(vPosition, 1.0);

	v_FragPos = vec3(ModelMat * vec4(vPosition, 1.0));

	Normal = vec4(normalize(vNormal * mat3(transpose(inverse(ModelMat * ViewMat)))), 1);
}
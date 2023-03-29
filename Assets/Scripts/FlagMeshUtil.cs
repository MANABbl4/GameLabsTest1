using UnityEngine;

namespace Assets.Scripts
{
    public static class FlagMeshUtil
    {
        public static Mesh Generate(uint width, uint height, float vertexDistance, string name)
        {
            if (width < 2 || height < 2)
                return null;

            var mesh = new Mesh
            {
                name = name
            };

            var positionOffset = new Vector2(-width * vertexDistance / 2, -height * vertexDistance / 2);

            var vertices = new Vector3[width * height];
            var triangles = new int[6 * (width - 1) * (height - 1)];
            var normals = new Vector3[width * height];
            var uv = new Vector2[width * height];
            var tangents = new Vector4[width * height];

            uint vertexNum = 0;
            uint quadNum = 0;

            for (uint i = 0; i < height; ++i)
            {
                for (uint j = 0; j < width; ++j)
                {
                    vertices[vertexNum] = new Vector3(j * vertexDistance + positionOffset.x, i * vertexDistance + positionOffset.y);

                    if (i < height - 1 && j < width - 1)
                    {
                        triangles[6 * quadNum + 0] = (int)vertexNum;
                        triangles[6 * quadNum + 1] = (int)(vertexNum + width);
                        triangles[6 * quadNum + 2] = (int)(vertexNum + width + 1);
                        triangles[6 * quadNum + 3] = (int)vertexNum;
                        triangles[6 * quadNum + 4] = (int)(vertexNum + width + 1);
                        triangles[6 * quadNum + 5] = (int)(vertexNum + 1);

                        ++quadNum;
                    }

                    normals[vertexNum] = Vector3.back;

                    uv[vertexNum] = new Vector2(j / (float)(width - 1), i / (float)(height - 1));

                    tangents[vertexNum] = new Vector4(1f, 0f, 0f, -1f);

                    ++vertexNum;
                }
            }

            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.normals = normals;
            mesh.uv = uv;
            mesh.tangents = tangents;

            return mesh;
        }
    }
}

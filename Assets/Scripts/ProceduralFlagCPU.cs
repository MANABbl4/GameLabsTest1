using System;
using UnityEngine;

namespace Assets.Scripts
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class ProceduralFlagCPU : MonoBehaviour
    {
        [SerializeField]
        private float _vertexDistance = 0.5f;
        [SerializeField]
        private uint _width = 0;
        [SerializeField]
        private uint _height = 0;
        [SerializeField]
        private Texture _texture;
        [SerializeField]
        private float _textureSpeed = 0.5f;
        [SerializeField]
        private float _waveSpeed = 1f;
        [SerializeField]
        private float _waveFrequency = 0.5f;
        [SerializeField]
        private float _waveAmplitude = 0.5f;

        private Mesh _mesh = null;

        // Start is called before the first frame update
        void Start()
        {
            _mesh = FlagMeshUtil.Generate(_width, _height, _vertexDistance, "Flag CPU");

            if (_mesh == null)
            {
                if (_width < 2)
                {
                    Debug.LogError($"Invalid width parameter {_width}. It should be 2 or more.");
                }

                if (_height < 2)
                {
                    Debug.LogError($"Invalid height parameter {_height}. It should be 2 or more.");
                }
            }
            else
            {
                GetComponent<MeshFilter>().mesh = _mesh;
                GetComponent<MeshRenderer>().material.mainTexture = _texture;
            }
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if (_mesh != null)
            {
                UpdateUV();

                UpdateVertices();
            }
        }

        private void UpdateUV()
        {
            var uv = _mesh.uv;

            for (int i = 0; i < uv.Length; i++)
            {
                uv[i].x += _textureSpeed * Time.fixedDeltaTime;
            }

            _mesh.uv = uv;
        }

        private void UpdateVertices()
        {
            var vertices = _mesh.vertices;

            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i].z = MathF.Sin(vertices[i].x * _waveFrequency + Time.realtimeSinceStartup * _waveSpeed) * _waveAmplitude;
            }

            _mesh.vertices = vertices;
            _mesh.RecalculateNormals();
        }
    }
}
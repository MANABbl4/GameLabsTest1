using System;
using UnityEngine;

namespace Assets.Scripts
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class ProceduralFlagGPU : MonoBehaviour
    {
        [SerializeField]
        private float _vertexDistance = 0.5f;
        [SerializeField]
        private uint _width = 0;
        [SerializeField]
        private uint _height = 0;
        [SerializeField]
        private float _textureSpeed = 0.5f;
        [SerializeField]
        private float _waveSpeed = 1f;
        [SerializeField]
        private float _waveFrequency = 0.5f;
        [SerializeField]
        private float _waveAmplitude = 0.5f;

        private Mesh _mesh = null;
        private Material _mat = null;

        // Start is called before the first frame update
        void Start()
        {
            _mesh = FlagMeshUtil.Generate(_width, _height, _vertexDistance, "Flag GPU");

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

                _mat = GetComponent<MeshRenderer>().materials[0];
            }
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if (_mesh != null && _mat != null)
            {
                _mat.SetFloat("_TextureSpeed", _textureSpeed);
                _mat.SetFloat("_WaveSpeed", _waveSpeed);
                _mat.SetFloat("_WaveFrequency", _waveFrequency);
                _mat.SetFloat("_WaveAmplitude", _waveAmplitude);
            }
        }
    }
}
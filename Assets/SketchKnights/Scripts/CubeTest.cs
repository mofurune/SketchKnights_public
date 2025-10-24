using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class CubeTest: MonoBehaviour
    {    
        private MeshRenderer meshRenderer;
        private Material material;

        private void Start()
        {
            meshRenderer = GetComponent<MeshRenderer>();
            material = meshRenderer.material;
        }

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("Enter"+ other.gameObject.name);
            if (other.gameObject.CompareTag("Sword"))
            {
                material.color = Color.red;
                meshRenderer.material = material;
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.CompareTag("Sword"))
            {
                material.color = Color.white;
                meshRenderer.material = material;
            }
        }
    }
}
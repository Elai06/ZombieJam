using Sirenix.OdinInspector;
using UnityEngine;

namespace Utils
{
    public class SwitchCemeteryMaterial : MonoBehaviour
    {
        [SerializeField] private Material _lightMaterial;
        [SerializeField] private Material _darkMaterial;

        [Button("SwitchOnLightMaterial")]
        public void SwitchLightMaterial()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                var child = transform.GetChild(i);
                var mesh = child.GetComponent<MeshRenderer>();
                if (mesh == null)
                {
                    for (int j = 0; j < child.childCount; j++)
                    {
                        var meshChildChild = child.GetChild(j).GetComponent<MeshRenderer>();
                        if(meshChildChild == null) continue;
                        meshChildChild.material = _lightMaterial;
                    }
                    
                    continue;
                }
                
                mesh.material = _lightMaterial;
            }
        }
        
        [Button("SwitchOnDarkMaterial")]
        public void SwitchDarkMaterial()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                var child = transform.GetChild(i);
                var mesh = child.GetComponent<MeshRenderer>();
                if (mesh == null)
                {
                    for (int j = 0; j < child.childCount; j++)
                    {
                        var meshChildChild = child.GetChild(j).GetComponent<MeshRenderer>();
                        if(meshChildChild == null) continue;
                        meshChildChild.material = _darkMaterial;
                    }
                    
                    continue;
                }
                
                mesh.material = _darkMaterial;
            }
        }
    }
}
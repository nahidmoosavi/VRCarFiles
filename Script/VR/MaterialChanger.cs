using UnityEngine;

namespace VRCar
{
    /// <summary>
    /// Simple Script to change Steering wheel handle materials 
    /// </summary>
    public class MaterialChanger : MonoBehaviour
    {
        [SerializeField] MeshRenderer mesh;
        [SerializeField] Material transparent;
        [SerializeField] Material highlight;
        [SerializeField] Material halfTransparent;
        // Start is called before the first frame update

        public void Transparent()
        {
            mesh.material = transparent;
        }

        public void HalfTarnsparent()
        {
            mesh.material = halfTransparent;
        }

        public void Highlight()
        {
            mesh.material = highlight;
        }

    }
}

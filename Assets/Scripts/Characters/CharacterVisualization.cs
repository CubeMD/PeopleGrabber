using UnityEngine;

namespace Characters
{
    public class CharacterVisualization : MonoBehaviour
    {
        private readonly static int BaseColor = Shader.PropertyToID("_BaseColor");
        
        [SerializeField]
        private Renderer agentRenderer;

        public void SetCharacterColor(Color color)
        {
            agentRenderer.material.SetColor(BaseColor, color);
        }
    }
}
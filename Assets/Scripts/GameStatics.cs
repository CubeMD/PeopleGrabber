using UnityEngine;

namespace DefaultNamespace
{
    public static class GameStatics
    {
        public readonly static int CharacterBaseColorID = Shader.PropertyToID("_Color");
        public readonly static int CharacterOutlineColorID = Shader.PropertyToID("_OutlineColor");
    }
}
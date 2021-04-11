using UnityEngine;

namespace Areas.NetCode
{
    public enum ZNetType { Local, Client, Server }

    public static class ZNetExtensions
    {

        public static ZNetType GetInstanceType(this ZNet znet)
        {

            bool isServer = znet.IsServer();
            bool isDedicated = znet.IsDedicated();

            if (isServer && !isDedicated) return ZNetType.Local;
            if (!isServer && !isDedicated) return ZNetType.Client;
            if (isServer && isDedicated) return ZNetType.Server;

            return ZNetType.Server;

        }

    }

}

namespace Areas
{

    public static class MaterialExtensions
    {

        public static void ToOpaqueMode(this Material material)
        {

            material.SetOverrideTag("RenderType", "");
            material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
            material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
            material.SetInt("_ZWrite", 1);
            material.DisableKeyword("_ALPHATEST_ON");
            material.DisableKeyword("_ALPHABLEND_ON");
            material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            material.renderQueue = -1;

        }

        public static void ToFadeMode(this Material material)
        {
            material.SetOverrideTag("RenderType", "Transparent");
            material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            material.SetInt("_ZWrite", 0);
            material.DisableKeyword("_ALPHATEST_ON");
            material.EnableKeyword("_ALPHABLEND_ON");
            material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;

        }

    }

}
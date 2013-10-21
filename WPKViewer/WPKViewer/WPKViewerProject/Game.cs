#region Using Statements
using System;
using System.IO;
using WaveEngine.Common;
using WaveEngine.Common.Graphics;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Services;
using WPKViewerProject.LoaderScenes;
#endregion

namespace WPKViewerProject
{
    public class Game : WaveEngine.Framework.Game
    {
        public override void Initialize(IApplication application)
        {
            base.Initialize(application);

            ScreenLayers screenLayers = WaveServices.ScreenLayers;
            screenLayers.AddScene<MyScene>();
            screenLayers.Apply();
        }

        public void LoadAsset(string fileName)
        {
            var assetType = this.ReadAssetType(fileName);
            Type loaderScene = null;

            if (assetType == typeof(Texture2D))
            {
                loaderScene = typeof(Texture2DLoaderScene);
            }

            if (assetType != null)
            {
                var currentScene = (BaseLoaderScene)WaveServices.ScreenLayers.FindScene(loaderScene);

                if (currentScene == null)
                {
                    var scene = (BaseLoaderScene)Activator.CreateInstance(loaderScene, new object[] { fileName });
                    WaveServices.ScreenLayers.AddScene(scene);
                    WaveServices.ScreenLayers.Apply();
                }
                else
                {
                    currentScene.LoadAsset(fileName);
                }
            }
        }

        /// <summary>
        /// Reads the type of the asset.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns>An ILoadable type, or null if the file isn't a valid WPK, or the header is unkown.</returns>
        private Type ReadAssetType(string fileName)
        {
            var contentRelativePath = "Content/" + fileName;
            var stream = new FileStream(contentRelativePath, FileMode.Open);
            Type type = null;

            using (var reader = new BinaryReader(stream))
            {
                int magicNumber = reader.ReadInt32();

                if (magicNumber != (('\0' << 24) | ('K' << 16) | ('P' << 8) | 'W'))
                {
                    // The file isn't a valid WPK
                    return null;
                }

                // Sample: "WaveEngine.Framework.Graphics.Texture2D v1.0.0.0"
                var header = reader.ReadString();
                var tokens = header.Split(new char[1] { ' ' });
                // This is a trick to ref. the assembly which defines every ILoadable,
                // as I already know Entity is within the same one
                var assembly = typeof(Entity).Assembly;
                type = assembly.GetType(tokens[0]);

                //var isCompressed = reader.ReadBoolean();
            }

            return type;
        }

        ///// <summary>
        ///// Gathers the type of the asset.
        ///// </summary>
        ///// <param name="fileName">Name of the file.</param>
        ///// <returns>The type of the asset. null if the type isn't supported, or can't be detected.</returns>
        //private Type GatherAssetType(string fileName)
        //{
        //    var contentRelativePath = "Content/" + fileName;
        //    Type assetType = null; 

        //    // Let's suppose first it's a texture 2d
        //    try
        //    {
        //        var texture2D = WaveServices.Assets.Global.LoadAsset<Texture2D>(contentRelativePath);
        //        assetType = typeof(Texture2D);
        //    }
        //    catch (Exception)
        //    {
        //        assetType = null;
        //    }

        //    return assetType;
        //}
    }
}

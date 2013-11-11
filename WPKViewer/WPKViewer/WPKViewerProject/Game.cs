#region Using Statements
using System;
using System.IO;
using WaveAssetsViewerProject.LoaderScenes;
using WaveEngine.Common;
using WaveEngine.Common.Graphics;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Services;
using WaveEngine.Framework.Sound;
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
            screenLayers.AddScene<LandingScene>();
            screenLayers.Apply();
        }

        public void LoadAsset(string fileName)
        {
            var assetInfo = this.ReadAssetInfo(fileName);

            if (assetInfo == null)
            {
                return;
            }

            var assetType = assetInfo.Type;
            Type loaderScene = null;

            // Every ILoadable supported by Wave:
            // InternalSkinnedModel
            // InternalStaticModel
            // InternalAnimation
            // SpriteFont
            // OK Texture2D
            // TextureAtlas
            // TextureCube
            // OK SoundEffect

            if (assetType == typeof(Texture2D))
            {
                loaderScene = typeof(Texture2DLoaderScene);
            }
            else if (assetType == typeof(SoundEffect))
            {
                loaderScene = typeof(SoundEffectLoaderScene);
            }
            else if (assetType == typeof(TextureAtlas))
            {
                loaderScene = typeof(TextureAtlasLoaderScene);
            }

            if (loaderScene != null)
            {
                var currentScene = (BaseLoaderScene)WaveServices.ScreenLayers.FindScene(loaderScene);

                // It's needed to recycle the same scene type, screen layers doesn't allow
                // a new one of the same type
                if (currentScene == null)
                {
                    var scene = (BaseLoaderScene)Activator.CreateInstance(loaderScene, new object[] { assetInfo });
                    WaveServices.ScreenLayers.AddScene(scene);
                    WaveServices.ScreenLayers.Apply();
                }
                else
                {
                    currentScene.LoadAsset(assetInfo);
                }
            }
        }

        /// <summary>
        /// Reads the asset info.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns>An AssetInfo, or null if the file isn't a valid WPK, or the header is unkown.</returns>
        private AssetInfo ReadAssetInfo(string fileName)
        {
            var contentRelativePath = "Content/" + fileName;
            var stream = new FileStream(contentRelativePath, FileMode.Open);
            AssetInfo assetInfo;

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
                var type = assembly.GetType(tokens[0]);

                var isCompressed = reader.ReadBoolean();

                assetInfo = new AssetInfo
                {
                    FileName = fileName,
                    Type = type,
                    Compressed = isCompressed
                };
            }

            return assetInfo;
        }
    }
}

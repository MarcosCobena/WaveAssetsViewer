using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WaveEngine.Common.Math;
using WaveEngine.Components.Animation;
using WaveEngine.Components.Graphics2D;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Services;
using WPKViewerProject;
using WPKViewerProject.LoaderScenes;

namespace WaveAssetsViewerProject.LoaderScenes
{
    /// <summary>
    /// TextureAtlas loader scene.
    /// </summary>
    class TextureAtlasLoaderScene : BaseLoaderScene
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TextureAtlasLoaderScene" /> class.
        /// </summary>
        /// <param name="assetInfo">The asset info.</param>
        public TextureAtlasLoaderScene(AssetInfo assetInfo)
            : base(assetInfo)
        { }

        protected override void CreateScene()
        {
            base.CreateScene();

            var assetPath = "Content/" + this.assetInfo.FileName;

            this.entity = new Entity()
                // How does "Frame0" behaves with "custom" sprite sheets?
                .AddComponent(new SpriteAtlas(assetPath, "Frame0"))
                .AddComponent(new SpriteAtlasRenderer(DefaultLayers.Alpha))
                .AddComponent(new Transform2D()
                {
                    Origin = Vector2.One / 2,
                    X = WaveServices.Platform.ScreenWidth / 2,
                    Y = WaveServices.Platform.ScreenHeight / 2
                })
                .AddComponent(new Animation2D());
            this.EntityManager.Add(this.entity);

            this.entity.FindComponent<Animation2D>().Play();
        }
    }
}

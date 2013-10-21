using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WaveEngine.Common.Math;
using WaveEngine.Components.Graphics2D;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Services;

namespace WPKViewerProject.LoaderScenes
{
    class Texture2DLoaderScene : BaseLoaderScene
    {
        public Texture2DLoaderScene(string fileName)
        {
            this.entity = new Entity()
                .AddComponent(new Sprite("Content/" + fileName))
                .AddComponent(new SpriteRenderer(DefaultLayers.Alpha))
                .AddComponent(new Transform2D()
                {
                    Origin = Vector2.One / 2,
                    X = WaveServices.Platform.ScreenWidth / 2,
                    Y = WaveServices.Platform.ScreenHeight / 2
                });
        }

        protected override void CreateScene()
        {
            this.EntityManager.Add(this.entity);
        }

        internal override void LoadAsset(string fileName)
        {
            if (this.entity != null)
            {
                this.EntityManager.Remove(this.entity);
            }

            this.entity = new Entity()
                .AddComponent(new Sprite("Content/" + fileName))
                .AddComponent(new SpriteRenderer(DefaultLayers.Alpha))
                .AddComponent(new Transform2D()
                {
                    Origin = Vector2.One / 2,
                    X = WaveServices.Platform.ScreenWidth / 2,
                    Y = WaveServices.Platform.ScreenHeight / 2
                });
            this.EntityManager.Add(this.entity);
        }
    }
}

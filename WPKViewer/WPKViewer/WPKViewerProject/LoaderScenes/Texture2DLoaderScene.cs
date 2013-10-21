using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WaveEngine.Common.Graphics;
using WaveEngine.Common.Math;
using WaveEngine.Components.Graphics2D;
using WaveEngine.Components.UI;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Services;
using WaveEngine.Framework.UI;

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
            this.RenderManager.BackgroundColor = Color.White;

            this.EntityManager.Add(this.entity);

            this.CreateUI();
        }

        /// <summary>
        /// Creates the UI.
        /// </summary>
        private void CreateUI()
        {
            var tbBackground = new ToggleSwitch()
            {
                OnText = "Background color",
                OffText = "Background color",
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Bottom,
                Margin = new Thickness(5),
                IsOn = true,
                // Hackish: how little I like this...
                Width = 250,
                TextColor = new Color(153, 153, 153)
            };
            this.EntityManager.Add(tbBackground);

            tbBackground.Toggled += (o, e) =>
                this.RenderManager.BackgroundColor = tbBackground.IsOn ?
                    Color.White :
                    Color.Black;
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

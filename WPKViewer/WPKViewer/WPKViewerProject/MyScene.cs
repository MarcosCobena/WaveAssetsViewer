#region Using Statements
using System;
using WaveEngine.Common;
using WaveEngine.Common.Graphics;
using WaveEngine.Common.Math;
using WaveEngine.Components.Graphics2D;
using WaveEngine.Components.UI;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Services;
using WaveEngine.Framework.UI;
#endregion

namespace WPKViewerProject
{
    public class MyScene : Scene
    {
        private Entity currentAsset;

        protected override void CreateScene()
        {
            RenderManager.BackgroundColor = Color.White;

            var help = new TextBlock() 
            { 
                Text = "Help: Drop a WPK file into this window",
                Foreground = Color.Black,
                Margin = new Thickness(5)
            };
            this.EntityManager.Add(help);
        }

        internal void LoadAsset(string fileName)
        {
            if (this.currentAsset != null)
            {
                this.EntityManager.Remove(this.currentAsset);
            }

            this.currentAsset = new Entity()
                .AddComponent(new Sprite("Content/" + fileName))
                .AddComponent(new SpriteRenderer(DefaultLayers.Alpha))
                .AddComponent(new Transform2D()
                {
                    Origin = Vector2.One / 2,
                    X = WaveServices.Platform.ScreenWidth / 2,
                    Y = WaveServices.Platform.ScreenHeight / 2
                });
            this.EntityManager.Add(this.currentAsset);
        }
    }
}

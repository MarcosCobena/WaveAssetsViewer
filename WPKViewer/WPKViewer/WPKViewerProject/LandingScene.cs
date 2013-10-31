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
    /// <summary>
    /// Defines the scene first viewed when the app is executed. It
    /// shows basic help.
    /// </summary>
    public class LandingScene : Scene
    {
        protected override void CreateScene()
        {
            RenderManager.BackgroundColor = Color.White;

            var help = new TextBlock() 
            { 
                Text = "Drop a WPK here",
                Foreground = Color.Black,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };
            this.EntityManager.Add(help);
        }
    }
}

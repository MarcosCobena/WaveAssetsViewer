#region Using Statements
using System;
using WaveEngine.Common;
using WaveEngine.Common.Graphics;
using WaveEngine.Framework;
using WaveEngine.Framework.Services;
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
            var currentScene = WaveServices.ScreenLayers.FindScene<MyScene>();

            if (currentScene != null)
            {
                currentScene.LoadAsset(fileName);
            }
        }
    }
}

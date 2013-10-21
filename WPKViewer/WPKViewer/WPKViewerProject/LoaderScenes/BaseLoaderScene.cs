using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WaveEngine.Framework;

namespace WPKViewerProject.LoaderScenes
{
    public abstract class BaseLoaderScene : Scene
    {
        protected Entity entity;

        protected override void CreateScene()
        {
            throw new NotImplementedException();
        }

        internal abstract void LoadAsset(string fileName);
    }
}

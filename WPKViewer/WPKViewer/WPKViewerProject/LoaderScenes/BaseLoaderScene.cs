using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WaveEngine.Common.Graphics;
using WaveEngine.Components.UI;
using WaveEngine.Framework;
using WaveEngine.Framework.Services;
using WaveEngine.Framework.UI;

namespace WPKViewerProject.LoaderScenes
{
    /// <summary>
    /// Defines the base scene which every type inherits. It provides
    /// info about the current WPK file.
    /// </summary>
    public abstract class BaseLoaderScene : Scene
    {
        /// <summary>
        /// The bug list URL
        /// </summary>
        private const string BugListUrl = "https://github.com/marcoscm/WaveAssetsViewer/issues";

        /// <summary>
        /// The entity which handles the WPK
        /// </summary>
        protected Entity entity;

        /// <summary>
        /// The asset info
        /// </summary>
        private AssetInfo assetInfo;

        /// <summary>
        /// The TextBlock file info
        /// </summary>
        private TextBlock tbFileInfo;

        /// <summary>
        /// The UI color
        /// </summary>
        protected Color uiColor = Color.Black;

        /// <summary>
        /// The font path
        /// </summary>
        protected string fontPath = null; // "Content/Consolas.wpk";

        /// <summary>
        /// The custom UI panel
        /// </summary>
        protected StackPanel customUIPanel;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseLoaderScene" /> class.
        /// </summary>
        /// <param name="assetInfo">The asset info.</param>
        public BaseLoaderScene(AssetInfo assetInfo)
        {
            this.assetInfo = assetInfo;
        }

        /// <summary>
        /// Creates the scene.
        /// </summary>
        /// <remarks>
        /// This method is called before all <see cref="T:WaveEngine.Framework.Entity" /> instances in this instance are initialized.
        /// </remarks>
        protected override void CreateScene()
        {
            tbFileInfo = new TextBlock()
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(5),
                Foreground = this.uiColor,
                //FontPath = this.fontPath,
                // BUG: Modifying this doesn't affect
                //LineSpacing = 50
            };
            this.EntityManager.Add(tbFileInfo);
            this.UpdateAssetInfoText();

            var btnFileBug = new Button()
            {
                Text = "File a bug",
                Foreground = this.uiColor,
                BorderColor = this.uiColor,
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Bottom,
                Margin = new Thickness(5),
                //FontPath = this.fontPath
            };
            this.EntityManager.Add(btnFileBug);

            btnFileBug.Click += (o, e) =>
                WaveServices.Platform.ShowWebBrowser(new Uri(BugListUrl));

            this.CreateUI();
        }

        /// <summary>
        /// Loads the asset. This method is needed to recycle
        /// current scene if the type of the incoming asset is
        /// the same as the current one, as current screen layers
        /// doesn't allow to load a new scene of the same type.
        /// </summary>
        /// <param name="assetInfo">Asset info.</param>
        public virtual void LoadAsset(AssetInfo assetInfo)
        {
            this.assetInfo = assetInfo;
            this.UpdateAssetInfoText();
        }

        /// <summary>
        /// Updates the asset info text.
        /// </summary>
        private void UpdateAssetInfoText()
        {
            this.tbFileInfo.Text = string.Format(
                "File name: {0}\n" +
                "Type: {1}\n" +
                "Compressed?: {2}",
                assetInfo.FileName, assetInfo.Type, assetInfo.Compressed ? "Yes" : "No");
        }

        /// <summary>
        /// Creates the UI. It allows to add custom UI per asset type.
        /// </summary>
        protected virtual void CreateUI()
        {
            // Panel
            this.customUIPanel = new StackPanel()
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Bottom,
                Margin = new Thickness(5)
            };
            this.EntityManager.Add(customUIPanel);
        }
    }
}

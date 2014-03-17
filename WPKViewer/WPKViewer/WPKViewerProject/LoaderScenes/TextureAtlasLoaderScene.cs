#region Using Statements
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using WaveAssetsViewerProject.Components;
using WaveEngine.Common.Graphics;
using WaveEngine.Common.Math;
using WaveEngine.Components.Gestures;
using WaveEngine.Components.Graphics2D;
using WaveEngine.Components.UI;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Physics2D;
using WaveEngine.Framework.Services;
using WaveEngine.Framework.UI;
using WPKViewerProject.Components;
#endregion

namespace WPKViewerProject.LoaderScenes
{
    class TextureAtlasLoaderScene : BaseLoaderScene
    {
        /// <summary>
        /// The minimum slider scale value
        /// </summary>
        private int MinimumSliderScaleValue = 1;

        /// <summary>
        /// The default slider scale value
        /// </summary>
        private const int DefaultSliderScaleValue = 10;

        /// <summary>
        /// The maximum slider scale value
        /// </summary>
        private int MaximumSliderScaleValue = 30;

        /// <summary>
        /// The scale slider
        /// </summary>
        private Slider sldScale;

        /// <summary>
        /// The size text block
        /// </summary>
        private TextBlock tbSize;

        public TextureAtlasLoaderScene(AssetInfo assetInfo)
            : base(assetInfo)
        {
        }

        protected override void CreateScene()
        {
            base.CreateScene();

            // This color is the one used on the sprite sheets 
            // to identify the background
            this.RenderManager.BackgroundColor = new Color(1, 0, 1);

            this.entity = new Entity()
                .AddComponent(new Transform2D()
                {
                    Origin = Vector2.One / 2,
                    X = WaveServices.Platform.ScreenWidth / 2,
                    Y = WaveServices.Platform.ScreenHeight / 2
                })
                .AddComponent(new TouchGestures()
                {
                    EnabledGestures = SupportedGesture.Translation
                })
                .AddComponent(new RectangleCollider())
                .AddComponent(new BorderDrawable2D())
                .AddComponent(new SpritesheetRectanglesRenderer());

            this.EntityManager.Add(this.entity);

            this.LoadAsset(this.assetInfo);
        }

        protected override void Start()
        {
            base.Start();

            this.UpdateSizeText();
        }

        /// <summary>
        /// Updates the size text.
        /// </summary>
        private void UpdateSizeText()
        {
            var trans2D = this.entity.FindComponent<Transform2D>();
            this.tbSize.Text = string.Format("Size (px): {0} x {1}",
                trans2D.Rectangle.Width,
                trans2D.Rectangle.Height);
        }

        /// <summary>
        /// Creates the UI.
        /// </summary>
        protected override void CreateUI()
        {
            base.CreateUI();

            // Size text
            this.tbSize = new TextBlock()
            {
                Foreground = this.uiColor,
                //FontPath = this.fontPath
            };
            customUIPanel.Add(tbSize);

            // Scale text
            var tbScale = new TextBlock()
            {
                Foreground = this.uiColor,
                Text = "Scale (1):",
                //FontPath = this.fontPath
            };
            customUIPanel.Add(tbScale);

            // Scale slider
            this.sldScale = new Slider()
            {
                Minimum = MinimumSliderScaleValue,
                Maximum = MaximumSliderScaleValue,
                Value = DefaultSliderScaleValue,
                TextColor = Color.Transparent,
                //FontPath = this.fontPath
            };
            customUIPanel.Add(sldScale);

            sldScale.RealTimeValueChanged += (o, e) =>
                {
                    var trans2D = this.entity.FindComponent<Transform2D>();
                    var finalScale = sldScale.Value / 10f;
                    trans2D.XScale = trans2D.YScale = finalScale;

                    tbScale.Text = string.Format("Scale ({0}):", finalScale);
                };

            var tsShowBorder = new ToggleSwitch()
            {
                OnText = "Show border?",
                OffText = "Show border?",
                IsOn = true,
                Width = 300,
                TextColor = this.uiColor,
                //FontPath = this.fontPath
            };
            customUIPanel.Add(tsShowBorder);

            tsShowBorder.Toggled += (o, e) =>
                {
                    var border = this.entity.FindComponent<BorderDrawable2D>();
                    border.IsVisible = !border.IsVisible;
                };

            var tsShowRectangles = new ToggleSwitch()
            {
                OnText = "Show Rectangles?",
                OffText = "Show Rectangles?",
                IsOn = true,
                Width = 300,
                TextColor = this.uiColor,
                //FontPath = this.fontPath
            };
            customUIPanel.Add(tsShowRectangles);

            tsShowRectangles.Toggled += (o, e) =>
            {
                var renderer = this.entity.FindComponent<SpritesheetRectanglesRenderer>();
                renderer.IsVisible = !renderer.IsVisible;
            };
        }

        public override void LoadAsset(AssetInfo assetInfo)
        {
            base.LoadAsset(assetInfo);

            this.assetInfo = assetInfo;

            var ta = this.Assets.LoadAsset<TextureAtlas>("Content/" + this.assetInfo.FileName);

            if (this.entity.FindComponent<Sprite>() == null)
            {
                this.entity.AddComponent(new SpriteRenderer(DefaultLayers.Alpha));
            }
            else
            {
                this.entity.RemoveComponent<Sprite>();
            }

            this.entity.AddComponent(new Sprite(ta.Texture));

            this.entity.FindComponent<SpritesheetRectanglesRenderer>().Rectangles = ta.SpriteRectangles.Values;

            this.entity.RefreshDependencies();

            this.Reset();
        }

        /// <summary>
        /// Resets this instance.
        /// </summary>
        private void Reset()
        {
            var trans2D = this.entity.FindComponent<Transform2D>();
            // Scale to 1, its default value
            trans2D.XScale = trans2D.YScale = 1;
            // Translate to the center
            trans2D.X = WaveServices.Platform.ScreenWidth / 2;
            trans2D.Y = WaveServices.Platform.ScreenHeight / 2;

            // Scale slider to its default value
            this.sldScale.Value = DefaultSliderScaleValue;

            this.UpdateSizeText();
        }
    }
}

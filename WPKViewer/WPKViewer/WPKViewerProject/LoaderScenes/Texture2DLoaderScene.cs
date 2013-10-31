using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
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

namespace WPKViewerProject.LoaderScenes
{
    class Texture2DLoaderScene : BaseLoaderScene
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

        public Texture2DLoaderScene(AssetInfo assetInfo)
            : base(assetInfo)
        {
            this.entity = new Entity()
                .AddComponent(new Sprite("Content/" + assetInfo.FileName))
                .AddComponent(new SpriteRenderer(DefaultLayers.Alpha))
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
                .AddComponent(new BorderDrawable2D());
        }

        protected override void CreateScene()
        {
            base.CreateScene();

            // This color is the one used on the sprite sheets 
            // to identify the background
            this.RenderManager.BackgroundColor = new Color(1, 0, 1);

            this.EntityManager.Add(this.entity);

            this.CreateUI();
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
        private void CreateUI()
        {
            // Panel
            var panel = new StackPanel()
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Bottom,
                Margin = new Thickness(5)
            };
            this.EntityManager.Add(panel);

            // Size text
            this.tbSize = new TextBlock()
            {
                Foreground = Color.Black
            };
            panel.Add(tbSize);

            // Scale text
            var tbScale = new TextBlock()
            {
                Foreground = Color.Black,
                Text = "Scale (1):"
            };
            panel.Add(tbScale);

            // Scale slider
            this.sldScale = new Slider()
            {
                Minimum = MinimumSliderScaleValue,
                Maximum = MaximumSliderScaleValue,
                Value = DefaultSliderScaleValue,
                TextColor = Color.Transparent
            };
            panel.Add(sldScale);

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
                TextColor = Color.Black
            };
            panel.Add(tsShowBorder);

            tsShowBorder.Toggled += (o, e) =>
                {
                    var border = this.entity.FindComponent<BorderDrawable2D>();
                    border.IsVisible = !border.IsVisible;
                };
        }

        public override void LoadAsset(AssetInfo assetInfo)
        {
            base.LoadAsset(assetInfo);

            this.entity.RemoveComponent<Sprite>();
            this.entity.AddComponent(new Sprite("Content/" + assetInfo.FileName));
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

using System;
using System.IO;
using System.Reflection;
using WaveEngine.Common.Graphics;
using WaveEngine.Common.Input;
using WaveEngine.Common.Math;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Services;

namespace WPKViewer
{
    public class App : WaveEngine.Adapter.Application
    {
        WPKViewerProject.Game game;
        SpriteBatch spriteBatch;
        Texture2D splashScreen;
        bool splashState = true;
        TimeSpan time;
        Vector2 position;
        Color backgroundSplashColor;

        public App()
        {
            this.Width = 800;
            this.Height = 600;
            this.FullScreen = false;
            this.WindowTitle = "Wave Assets Viewer";
        }

        public override void Initialize()
        {
            this.game = new WPKViewerProject.Game();
            this.game.Initialize(this);

            // We know renderForm is a private member so we get it through reflection
            var form = typeof(WaveEngine.Adapter.Application)
                .GetField("renderForm", BindingFlags.Instance | BindingFlags.NonPublic)
                .GetValue(this)
                as System.Windows.Forms.Form;
            // By default dropping is disabled, so we enable it
            form.AllowDrop = true;
            // Here we wait for file dropping
            form.DragDrop += (o, e) =>
                {
                    if (e.Data.GetDataPresent(System.Windows.Forms.DataFormats.FileDrop))
                    {
                        var filePaths = (string[])e.Data.GetData(System.Windows.Forms.DataFormats.FileDrop);

                        if (filePaths.Length > 0)
                        {
                            var filePath = filePaths[0];
                            this.CopyFileToContent(filePath);

                            var fileName = Path.GetFileName(filePath);
                            this.game.LoadAsset(fileName);
                        }
                    }
                };
            // When we drag a file we stablish it'll act as a copy
            form.DragEnter += (o, e) =>
                {
                    if (e.Data.GetDataPresent(System.Windows.Forms.DataFormats.FileDrop))
                    {
                        e.Effect = System.Windows.Forms.DragDropEffects.Copy;
                    }
                };

            #region WAVE SOFTWARE LICENSE AGREEMENT
            this.backgroundSplashColor = new Color(32, 32, 32, 255);
            this.spriteBatch = new SpriteBatch(WaveServices.GraphicsDevice);

            var resourceNames = Assembly.GetExecutingAssembly().GetManifestResourceNames();
            string name = string.Empty;

            foreach (string item in resourceNames)
            {
                if (item.Contains("SplashScreen.wpk"))
                {
                    name = item;
                    break;
                }
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new InvalidProgramException("License terms not agreed.");
            }

            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(name))
            {
                this.splashScreen = WaveServices.Assets.Global.LoadAsset<Texture2D>(name, stream);
            }

            position = new Vector2();
            position.X = (this.Width / 2.0f) - (this.splashScreen.Width / 2.0f);
            position.Y = (this.Height / 2.0f) - (this.splashScreen.Height / 2.0f);
            #endregion
        }

        private void CopyFileToContent(string filePath)
        {
            if (File.Exists(filePath))
            {
                var currentAssemblyPath = Directory.GetCurrentDirectory();
                var fileName = Path.GetFileName(filePath);
                var contentPath = string.Format(@"{0}\Content", currentAssemblyPath);

                if (!Directory.Exists(contentPath))
                {
                    Directory.CreateDirectory(contentPath);
                }

                var destPath = string.Format(@"{0}\{1}", contentPath, fileName);
                File.Copy(filePath, destPath, true);
            }
        }

        public override void Update(TimeSpan elapsedTime)
        {
            if (this.game != null && !this.game.HasExited)
            {
                if (WaveServices.Input.KeyboardState.F10 == ButtonState.Pressed)
                {
                    this.FullScreen = !this.FullScreen;
                }

                if (this.splashState)
                {
                    #region WAVE SOFTWARE LICENSE AGREEMENT
                    this.time += elapsedTime;
                    if (time > TimeSpan.FromSeconds(2))
                    {
                        this.splashState = false;
                    }
                    #endregion
                }
                else
                {
                    if (WaveServices.Input.KeyboardState.Escape == ButtonState.Pressed)
                    {
                        WaveServices.Platform.Exit();
                    }
                    else
                    {
                        this.game.UpdateFrame(elapsedTime);
                    }
                }
            }
        }

        public override void Draw(TimeSpan elapsedTime)
        {
            if (this.game != null && !this.game.HasExited)
            {
                if (this.splashState)
                {
                    #region WAVE SOFTWARE LICENSE AGREEMENT
                    WaveServices.GraphicsDevice.RenderTargets.SetRenderTarget(null);
                    WaveServices.GraphicsDevice.Clear(ref this.backgroundSplashColor, ClearFlags.Target, 1);
                    this.spriteBatch.Begin();
                    this.spriteBatch.Draw(this.splashScreen, this.position, Color.White);
                    this.spriteBatch.End();
                    #endregion
                }
                else
                {
                    this.game.DrawFrame(elapsedTime);
                }
            }
        }
    }
}


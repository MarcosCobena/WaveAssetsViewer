using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WaveEngine.Common.Graphics;
using WaveEngine.Common.Math;
using WaveEngine.Common.Media;
using WaveEngine.Components.UI;
using WaveEngine.Framework.Services;
using WaveEngine.Framework.Sound;
using WaveEngine.Framework.UI;

namespace WPKViewerProject.LoaderScenes
{
    class SoundEffectLoaderScene : BaseLoaderScene
    {
        private SoundInfo sound;
        private SoundBank bank;
        private SoundInstance soundInstance;

        /// <summary>
        /// The sound info TextBlock
        /// </summary>
        private TextBlock tbSoundInfo;

        public SoundEffectLoaderScene(AssetInfo assetInfo)
            : base(assetInfo)
        {
            var path = "Content/" + assetInfo.FileName;
            this.sound = new SoundInfo(path);
            this.uiColor = Color.White;
        }

        protected override void CreateScene()
        {
            base.CreateScene();

            this.RenderManager.BackgroundColor = Color.Black;

            ConfigureSound();
        }

        protected override void CreateUI()
        {
            base.CreateUI();

            var btnPlay = new Button()
            {
                Text = "Play",
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Foreground = this.uiColor,
                BorderColor = this.uiColor
            };
            this.EntityManager.Add(btnPlay);

            btnPlay.Click += (o, e) =>
            {
                if (this.soundInstance != null)
                {
                    this.soundInstance.Stop();
                }

                this.soundInstance = WaveServices.SoundPlayer.Play(this.sound, 1, false);
            };

            this.tbSoundInfo = new TextBlock()
            {
                Foreground = this.uiColor,
                Text = "Loading...\nLoading...",
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Bottom
            };
            this.customUIPanel.Add(tbSoundInfo);
        }

        /// <summary>
        /// Allows to perform custom code when this instance is started.
        /// </summary>
        /// <remarks>
        /// This base method perfoms a layout pass.
        /// </remarks>
        protected override void Start()
        {
            base.Start();

            this.soundInstance = WaveServices.SoundPlayer.Play(this.sound, 1, false);
        }

        /// <summary>
        /// Configures the sound.
        /// </summary>
        private void ConfigureSound()
        {
            this.bank = new SoundBank(this.Assets);
            WaveServices.SoundPlayer.RegisterSoundBank(this.bank);
            this.bank.Add(this.sound);

            this.UpdateSoundInfoTextBlock();
        }

        /// <summary>
        /// Updates the sound info text block.
        /// </summary>
        private void UpdateSoundInfoTextBlock()
        {
            var channels = this.sound.SoundEffect.Channels == 1 ?
                "mono" :
                "stereo";
            this.tbSoundInfo.Text = string.Format(
                "Sample rate (Hz): {3}\n" +
                "Channels: {1} ({2})\n" +
                "Bit rate (bits/s): {0}",
                this.sound.SoundEffect.BitsPerSample, this.sound.SoundEffect.Channels,
                channels, this.sound.SoundEffect.SampleRate);
        }

        public override void LoadAsset(AssetInfo assetInfo)
        {
            base.LoadAsset(assetInfo);

            this.soundInstance.Stop();
            this.soundInstance = null;
            this.bank.Remove(this.sound);

            var path = "Content/" + assetInfo.FileName;

            if ((this.sound != null) && (this.sound.SoundEffect != null))
            {
                this.sound.SoundEffect.Unload();
                WaveServices.Assets.Global.UnloadAsset(path);
            }

            this.sound = new SoundInfo(path);
            this.bank.Add(this.sound);

            this.UpdateSoundInfoTextBlock();

            this.soundInstance = WaveServices.SoundPlayer.Play(this.sound, 1, false);
        }
    }
}

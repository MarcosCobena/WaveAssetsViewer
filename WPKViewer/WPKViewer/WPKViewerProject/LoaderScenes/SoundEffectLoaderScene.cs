using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WaveEngine.Common.Graphics;
using WaveEngine.Common.Media;
using WaveEngine.Framework.Services;
using WaveEngine.Framework.Sound;

namespace WPKViewerProject.LoaderScenes
{
    class SoundEffectLoaderScene : BaseLoaderScene
    {
        private SoundInfo sound;
        private SoundBank bank;
        private SoundInstance soundInstance;

        public SoundEffectLoaderScene(AssetInfo assetInfo)
            : base(assetInfo)
        {
            var path = "Content/" + assetInfo.FileName;
            this.sound = new SoundInfo(path);
        }

        protected override void CreateScene()
        {
            base.CreateScene();

            this.RenderManager.BackgroundColor = Color.White;

            this.bank = new SoundBank(this.Assets);
            WaveServices.SoundPlayer.RegisterSoundBank(this.bank);
            this.bank.Add(this.sound);

            this.soundInstance = WaveServices.SoundPlayer.Play(this.sound, 1, true);
        }

        public override void LoadAsset(AssetInfo assetInfo)
        {
            base.LoadAsset(assetInfo);

            this.soundInstance.Stop();

            var path = "Content/" + assetInfo.FileName;

            if ((this.sound != null) && (this.sound.SoundEffect != null))
            {
                this.sound.SoundEffect.Unload();
                WaveServices.Assets.Global.UnloadAsset(path);
            }

            this.sound = new SoundInfo(path);
            this.bank.Add(this.sound);
            //var stream = WaveServices.Storage.OpenStorageFile(path, WaveEngine.Common.IO.FileMode.Open);
            //this.sound.SoundEffect = WaveServices.Assets.Global.LoadAsset<SoundEffect>(path, stream);
            //this.bank.AddWithouthLoad(this.sound);

            this.soundInstance = WaveServices.SoundPlayer.Play(this.sound, 1, true);
        }
    }
}

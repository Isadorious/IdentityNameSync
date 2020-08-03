using System;
using System.IO;
using Torch;
using Torch.API;
using Torch.API.Managers;
using Torch.API.Session;
using Torch.Session;
using Sandbox.Game.World;
using NLog;
using System.Windows.Controls;
using Torch.API.Plugins;

namespace IdentityNameSync {
    public class IdentityNameSyncPlugin : TorchPluginBase, IWpfPlugin {
        public static readonly Logger Log = LogManager.GetCurrentClassLogger();
        private TorchSessionManager _sessionManager;
        private IMultiplayerManagerBase _multibase;
        private Persistent<IdentityNameSyncConfig> _config;
        public IdentityNameSyncConfig Config => _config.Data;
        private IdentityNameSyncControl _control;
        public UserControl GetControl() => _control ?? (_control = new IdentityNameSyncControl(this));
        public override void Init(ITorchBase torch) {
            base.Init(torch);
            SetupConfig();
            LoadIdentitySync();
        }

        private void SessionChanged(ITorchSession session, TorchSessionState state) {
            switch (state) {
                case TorchSessionState.Loaded:
                    LoadIdentitySync();
                    break;
                case TorchSessionState.Unloaded:
                    UnloadIdentitySync();
                    break;
            }
        }
        public void UnloadIdentitySync() {
            Dispose();
        }
        public void LoadIdentitySync() {
            if (_sessionManager == null) {
                _sessionManager = Torch.Managers.GetManager<TorchSessionManager>();
                if (_sessionManager == null) Log.Warn("Session manager not loaded");
                else _sessionManager.SessionStateChanged += SessionChanged;
            }
            if (Torch.CurrentSession != null) {
                if (_multibase == null) {
                    _multibase = Torch.CurrentSession.Managers.GetManager<IMultiplayerManagerBase>();
                    if (_multibase == null) Log.Warn("Unable to load Multiplayer Manager");
                    else _multibase.PlayerJoined += _multibase_PlayerJoined;
                }
            }
        }
        private async void _multibase_PlayerJoined(IPlayer obj)
        {
            if(Config.SyncEnabled)
            {
                try {
                    long identityID = MySession.Static.Players.TryGetIdentityId(obj.SteamId);
                    if (!MySession.Static.Players.TryGetIdentity(identityID).DisplayName.Equals(obj.Name)) {
                        MySession.Static.Players.TryGetIdentity(identityID).SetDisplayName(obj.Name);
                    }
                }
                catch (Exception ex) {
                    Log.Warn($"Identity for player {obj.Name} does not exist. They are likely a new player");
                }
            }
        }
        private void SetupConfig() {
            var configFile = Path.Combine(StoragePath, "IdentitySync.cfg");
            try {
                _config = Persistent<IdentityNameSyncConfig>.Load(configFile);
            } catch (Exception e) {
                Log.Warn(e);
            }

            if(_config?.Data == null) {
                Log.Info("Created default config, as none was found");
                _config = new Persistent<IdentityNameSyncConfig>(configFile, new IdentityNameSyncConfig());
                _config.Save();
            }
        }

        public void Save() {
            try {
                _config.Save();
                Log.Info("Config saved");
            } catch(IOException e) {
                Log.Warn(e, "Failed to save config");
            }
        }
    }
}
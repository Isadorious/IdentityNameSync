using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Torch;

namespace IdentityNameSync {
    public class IdentityNameSyncConfig : ViewModel {
        
        // Plugin properties
        private bool _SyncEnabled = true; 
        public bool SyncEnabled { get => _SyncEnabled; set => SetValue(ref _SyncEnabled, value); }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NLog;
using Sandbox.Game.World;
using Torch;
using Torch.Commands;
using Torch.Commands.Permissions;
using Torch.Mod;
using Torch.Mod.Messages;
using VRage.Game.ModAPI;

namespace IdentityNameSync
{
    public class FactionInfoCommands : CommandModule
    {
        public static readonly Logger Log = LogManager.GetCurrentClassLogger();
        public IdentityNameSyncPlugin Plugin => (IdentityNameSyncPlugin)Context.Plugin;

        [Command("identityID", "Get the identity ID for a player with the given name")]
        [Permission(MyPromoteLevel.Admin)]
        public void GetIdentityByName(string name) {
                MyPlayer player = MySession.Static?.Players?.GetPlayerByName(name);
                Context.Respond($"Identity ID for player {name} is: {player.Identity.IdentityId}");
        }

        [Command("identityID steam", "Get the identity associated with the given Steam64 ID")]
        [Permission(MyPromoteLevel.Admin)]
        public void GetIdentityBySteam64(ulong steam64ID) {
            MyPlayer player = MySession.Static?.Players?.TryGetPlayerBySteamId(steam64ID);
            Context.Respond($"Identity ID for player {player.Identity.DisplayName} ({steam64ID}) is: {player.Identity.IdentityId}");
        }

        [Command("factionID", "Get the ID for a faction with the given tag")]
        [Permission(MyPromoteLevel.Admin)]
        public void GetFactionID(string tag){
            var faction = MySession.Static?.Factions?.TryGetFactionByTag(tag);
            Context.Respond($"ID for faction {tag} is: {faction.FactionId}");
        }
        
        [Command("steamID", "Get the Steam64ID for a given player name")]
        [Permission(MyPromoteLevel.Moderator)]
        public void GetSteam64ID(string name) {
            MyPlayer player = MySession.Static?.Players?.GetPlayerByName(name);
            ulong? steamID = MySession.Static?.Players?.TryGetSteamId(player.Identity.IdentityId);
            Context.Respond($"Steam64ID for player {name} is: {steamID}");
        }

    }
}

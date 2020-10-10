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

        // From https://github.com/TorchAPI/Essentials
        public static IMyIdentity GetIdentityByNameOrIds(string playerNameOrIds)
        {
            foreach (var identity in MySession.Static.Players.GetAllIdentities())
            {
                if (identity.DisplayName == playerNameOrIds)
                    return identity;

                if (long.TryParse(playerNameOrIds, out long identityId))
                    if (identity.IdentityId == identityId)
                        return identity;

                if (ulong.TryParse(playerNameOrIds, out ulong steamId))
                {
                    ulong id = MySession.Static.Players.TryGetSteamId(identity.IdentityId);
                    if (id == steamId)
                        return identity;
                }
            }

            return null;
        }

        [Command("identityID", "Get the identity ID for a player with the given name or steam ID")]
        [Permission(MyPromoteLevel.Admin)]
        public void GetIdentityByNameOrIDs(string nameOrSteamID) {
                IMyIdentity ident = GetIdentityByNameOrIds(nameOrSteamID);
                Context.Respond($"Identity ID for player {ident.DisplayName} is: {ident.IdentityId}");
        }

        [Command("factionID", "Get the ID for a faction with the given tag")]
        [Permission(MyPromoteLevel.Admin)]
        public void GetFactionID(string tag){
            var faction = MySession.Static?.Factions?.TryGetFactionByTag(tag);
            Context.Respond($"ID for faction {tag} is: {faction.FactionId}");
        }
    }
}

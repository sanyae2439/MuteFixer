using System.Collections.Generic;
using MEC;
using Smod2;
using Smod2.Attributes;
using Smod2.EventHandlers;
using Smod2.Events;

namespace MuteFixer
{
    [PluginDetails(
    author = "sanyae2439",
    name = "MuteFixer",
    description = "Fix persistant mute",
    id = "sanyae2439.MuteFixer",
    version = "2.0",
    SmodMajor = 3,
    SmodMinor = 5,
    SmodRevision = 0
    )]
    public class MuteFixer : Plugin
    {
        public override void OnDisable()
        {
            this.Info("MuteFixer Disabled...");
        }

        public override void OnEnable()
        {
            this.Info("MuteFixer Enabled!");
        }

        public override void Register()
        {
            this.AddEventHandlers(new JoinHandler());
        }
    }

    public class JoinHandler : IEventHandlerPlayerJoin
    {
        public void OnPlayerJoin(PlayerJoinEvent ev)
        {
            if(MuteHandler.QueryPersistantMute(ev.Player.SteamId))
            {
                (ev.Player.GetGameObject() as UnityEngine.GameObject).GetComponent<CharacterClassManager>().NetworkMuted = true;
            }
            Timing.RunCoroutine(this._DelayedForceSyncMute(), Segment.FixedUpdate);
        }

        public IEnumerator<float> _DelayedForceSyncMute()
        {
            yield return Timing.WaitForSeconds(0.25f);
            foreach(var ccm in UnityEngine.Object.FindObjectsOfType<CharacterClassManager>())
            {
                if(ccm.Muted) ccm.SetDirtyBit(1u);
            }
            yield break;
        }
    }
}

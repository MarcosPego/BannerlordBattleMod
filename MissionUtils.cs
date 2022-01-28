using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.Core;
using TaleWorlds.InputSystem;
using TaleWorlds.MountAndBlade;

namespace BannerlordBattleMod
{
    public class MissionUtils
    {
        public static void ManageInputKeys(Mission mission)
        {
            if (mission != null)
            {
                if (Input.IsKeyDown(InputKey.LeftControl))
                {
                    if (Input.IsKeyPressed(InputKey.F10))
                    {
                        //InformationManager.DisplayMessage(new InformationMessage("Heroes in roster: "));

                        KillMainHero();
                    }
                  
                }
               
            }
        }

        public static void KillMainHero()
        {
            Blow b = new Blow(Mission.Current.MainAgent.Index);
            b.DamageType = DamageTypes.Blunt;
            b.BaseMagnitude = 1E+09f;
            b.Position = Mission.Current.MainAgent.Position;
            Mission.Current.MainAgent.Die(b, Agent.KillInfo.Backstabbed);
        }
    }
}

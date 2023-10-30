using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public sealed class SageAdviceAbility : TouchRangeFriendlyAbility
{
    public SageAdviceAbility()
    {
        Name = "SageAdvice";
        DisplayName = "Sage Advice";
        Description = "Target a creature in range 1. Give its stats +1 and trigger a random one of its active or \"On Death\" abilities if it has any.";
        MaxCooldown = 2;
    }

    public override void Trigger(object sender, EventArgs e)
    {
        if (ChoicesNeeded.Where(x => x.Caption == "Target").FirstOrDefault() is CreatureTargetChoice creatChoice && creatChoice.ChoiceMade)
        {
            creatChoice.TargetCreature.StatsChange(1, 1, 1, 0);

            var activeAbilsList = new List<Ability>();
            activeAbilsList.AddRange(creatChoice.TargetCreature.Abilities.Where(x => x is ActiveAbility activeAbil && ChoiceManager.ValidChoicesExist(activeAbil.ChoicesNeeded, activeAbil)));
            var deathAbilsList = new List<Ability>();
            deathAbilsList.AddRange(creatChoice.TargetCreature.Abilities.Where(x => x is AfterMyDeathPassive deathAbil));
            var allAbilsList = new List<Ability>();
            allAbilsList.AddRange(activeAbilsList);
            allAbilsList.AddRange(deathAbilsList);

            if(allAbilsList.Count > 0)
            {
                var r = new Random();
                var targetAbil = allAbilsList[r.Next(0, allAbilsList.Count)];
                if (activeAbilsList.Contains(targetAbil))
                {
                    if(Owner.MyGame.CurrentPlayerIndex == Owner.Controller.MyPlayerIndex)
                    {
                        (targetAbil as ActiveAbility).TrueActivation();
                    }
                    else
                    {
                        targetAbil.ExternalTrigger(this, new EventArgs());
                    }
                }
                else
                {
                    targetAbil.ExternalTrigger(this, new CreatureDiesArgs() { CreatureDied = creatChoice.TargetCreature, WhereItDied = creatChoice.TargetCreature.MySpace});
                }
            }
        }
    }
}

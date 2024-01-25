using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public sealed class LightningOfTheBeholderAbility : RangedTargetEnemyAbility
{
    public LightningOfTheBeholderAbility()
    {
        Name = "LightningOfTheBeholder";
        DisplayName = "Lightning of the Beholder";
        MaxCooldown = 2;
        Range = 3;
    }

    public override void Trigger(object sender, EventArgs e)
    {
        if(ChoicesNeeded.Where(x => x.Caption == "Target").FirstOrDefault() is CreatureTargetChoice creatChoice && creatChoice.ChoiceMade)
        {
            Owner.AttackTarget(creatChoice.TargetCreature, true);
            var r = new Random();
            var choice = r.Next(0, 3);
            switch (choice)
            {
                case 0:
                    creatChoice.TargetCreature.StatsChange(AtkChg: -2);
                    break;
                case 1:
                    creatChoice.TargetCreature.GainTag(CreatureTag.CANT_MOVE);
                    creatChoice.TargetCreature.GainHiddenAbility(new RemoveCantMoveEndOfTurn());
                    break;
                case 2:
                    creatChoice.TargetCreature.TakeDamage(4, Owner);
                    break;
                default:
                    break;
            }
        }
    }

    public override void UpdateDescription()
    {
        Description = "Ranged Attack: " + Range + ". The character hit by this attack has an equal chance of either: losing 2 attack, being unable to move next turn, or taking an additional 4 damage.";
    }
}

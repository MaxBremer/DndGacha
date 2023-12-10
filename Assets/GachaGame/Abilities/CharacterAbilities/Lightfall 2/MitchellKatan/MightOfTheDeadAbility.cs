using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public sealed class MightOfTheDeadAbility : FriendlyNonselfCreatureSummonedPassive
{
    private bool _affectingHealth = true;
    private int _rotationCount = 0;

    public MightOfTheDeadAbility()
    {
        Name = "MightOfTheDead";
        DisplayName = "Might of the Dead";
    }

    public override void Trigger(object sender, EventArgs e)
    {
        if(AbilityRank == 0)
        {
            if (_affectingHealth)
            {
                Owner.StatsChange(HealthChg: Owner.Health);
            }
            else
            {
                Owner.StatsChange(AtkChg: Owner.Attack);
            }
            _affectingHealth = !_affectingHealth;
        }
        else if(AbilityRank == 1)
        {
            if(_rotationCount == 0)
            {
                Owner.StatsChange(HealthChg: Owner.Health);
            }
            else if(_rotationCount == 1)
            {
                Owner.StatsChange(AtkChg: Owner.Attack);
            }
            else
            {
                Owner.StatsChange(AtkChg: Owner.Attack, HealthChg: Owner.Health);
            }

            _rotationCount = (_rotationCount + 1) % 3;
        }
        else
        {
            Owner.StatsChange(AtkChg: Owner.Attack, HealthChg: Owner.Health);
        }

    }

    public override void UpdateDescription()
    {
        if(AbilityRank < 2)
        {
            Description = "When you summon a creature, double this characters health. Swap this ability to effect attack (" + (AbilityRank == 0 ? "Swaps between attack and health on each use. " : "Swaps between attack, health, and BOTH on each use. ") + GetEndTag() + ").";
        }
        else
        {
            Description = "When you summon a creature, double this characters attack and health.";
        }
    }

    private string GetEndTag()
    {
        if(AbilityRank == 0)
        {
            return _affectingHealth ? "Health is next!" : "Attack is next!";
        }
        else
        {
            var stri = "";
            switch (_rotationCount)
            {
                case 0:
                    stri = "Health is next!";
                    break;
                case 1:
                    stri = "Attack is next!";
                    break;
                case 2:
                    stri = "Both are next!";
                    break;
            }
            return stri;
        }
    }
}

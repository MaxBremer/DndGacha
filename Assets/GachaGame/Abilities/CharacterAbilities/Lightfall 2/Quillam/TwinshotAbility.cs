using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public sealed class TwinshotAbility : BeforeMyAbilityActivatesAbility
{
    private List<RangedActiveAbility> AbilitiesAffected = new List<RangedActiveAbility>();
    private int _numActs = 2;

    public TwinshotAbility()
    {
        Name = "Twinshot";
        DisplayName = "Twinshot";
        Priority = -1;
    }

    public override void Trigger(object sender, EventArgs e)
    {
        if (e is AbilityActivateArgs abilArgs)
        {
            abilArgs.NumActivations = Math.Max(abilArgs.NumActivations, _numActs);
        }
    }

    public override void UpdateDescription()
    {
        Description = "This characters active abilities activate " + (AbilityRank < 2 ? "twice" : "thrice") + (AbilityRank < 1 ? "" : " and have 1 greater range") + " if possible.";
    }

    public override void RankUpToOne()
    {
    }

    public override void RankUpToTwo()
    {
        _numActs = 3;
    }

    public override void OnGained()
    {
        base.OnGained();
        if(AbilityRank < 1)
        {
            return;
        }

        foreach (var abil in Owner.Abilities)
        {
            if(abil is RangedActiveAbility rangeAbil)
            {
                AddToAffected(rangeAbil);
            }
        }

        EventManager.StartListening(GachaEventType.CreatureGainedAbility, AbilGainedTrips, Priority);
    }

    public override void OnLost()
    {
        base.OnLost();

        if (AbilityRank < 1)
        {
            return;
        }

        foreach (var abil in AbilitiesAffected)
        {
            abil.Range--;
        }

        AbilitiesAffected.Clear();
        EventManager.StopListening(GachaEventType.CreatureGainedAbility, AbilGainedTrips, Priority);
    }

    private void AbilGainedTrips(object sender, EventArgs e)
    {
        if(e is AbilityChangeArgs abilArgs && abilArgs.AbilityChanged.Owner == Owner && abilArgs.AbilityChanged is RangedActiveAbility rangeAbil)
        {
            AddToAffected(rangeAbil);
        }
    }

    private void AddToAffected(RangedActiveAbility rangeAbil)
    {
        if (!AbilitiesAffected.Contains(rangeAbil))
        {
            rangeAbil.Range++;
            AbilitiesAffected.Add(rangeAbil);
        }
    }
}

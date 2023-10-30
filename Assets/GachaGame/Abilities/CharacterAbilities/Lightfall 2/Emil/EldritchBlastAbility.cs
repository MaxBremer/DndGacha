using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public sealed class EldritchBlastAbility : RangedAttackAbility
{
    private const int HEX_RANGE = 5;

    public EldritchBlastAbility()
    {
        Name = "EldritchBlast";
        DisplayName = "Eldritch Blast";
        Description = "Ranged Attack: 3. If the target is under my Hexblades Curse, this is instead Ranged Attack: 5.";
        MaxCooldown = 1;
        Range = 3;
    }

    public override void InitAbility()
    {
        base.InitAbility();
        ChoicesNeeded.Clear();
        Func<Creature, bool> isValid = x => x != Owner && (GachaGrid.IsInRange(Owner, x, Range) || (x.WhereTag(CreatureTag.HEXBLADES_CURSE).Where(x => x.CreatureData == Owner).Any() && GachaGrid.IsInRange(Owner, x, HEX_RANGE)));
        ChoicesNeeded.Add(new CreatureTargetChoice() { IsValidCreature = isValid, Caption = "Target" });
    }
}

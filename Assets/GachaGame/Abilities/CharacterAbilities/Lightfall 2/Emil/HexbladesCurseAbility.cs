using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HexbladesCurseAbility : TargetSingleEnemyAbility
{
    private const int DAMAGE_AMOUNT = 3;

    public HexbladesCurseAbility()
    {
        Name = "HexbladesCurse";
        DisplayName = "Hexblade's Curse";
        Description = "Select a character. It takes " + DAMAGE_AMOUNT + " extra damage when damaged by this character. This can only curse one character at a time.";
        MaxCooldown = 0;
    }

    // TODO: Possibly add event listener to listen for lost tags, and if its Hexblades Curse and there are no more creats cursed by me remove the event listener.

    public override void Trigger(object sender, EventArgs e)
    {
        if(ChoicesNeeded.Where(x => x.Caption == "Target").FirstOrDefault() is CreatureTargetChoice creatChoice && creatChoice.ChoiceMade)
        {
            // Get creatures currently cursed by me
            var currentCursed = Owner.MyGame.AllCreatures.Where(c => c.WhereTag(CreatureTag.HEXBLADES_CURSE).Where(t => t.CreatureData == Owner).Any());

            // If no existing curses, start the event listener to add damage.
            if (!currentCursed.Any())
            {
                EventManager.StartListening("BeforeDamage", ExtraDamage);
            }

            // Remove curses from creatures currently cursed by me
            foreach (var creat in currentCursed)
            {
                var toRemoveTags = new List<Tag>();
                toRemoveTags.AddRange(creat.Tags.Where(x => x.TagType == CreatureTag.HEXBLADES_CURSE && x.CreatureData == Owner));
                foreach (var tag in toRemoveTags)
                {
                    creat.LoseTag(tag);
                }
            }

            // Apply new curse
            creatChoice.TargetCreature.GainTag(new Tag(CreatureTag.HEXBLADES_CURSE) { CreatureData = Owner });

        }
    }

    private void ExtraDamage(object sender, EventArgs e)
    {
        if(e is TakingDamageArgs dmgArgs && dmgArgs.DamageDealer == Owner && sender is Creature c && c.Tags.Where(x => x.TagType == CreatureTag.HEXBLADES_CURSE && x.CreatureData == Owner).Any())
        {
            dmgArgs.DamageAmount += DAMAGE_AMOUNT;
        }
    }
}

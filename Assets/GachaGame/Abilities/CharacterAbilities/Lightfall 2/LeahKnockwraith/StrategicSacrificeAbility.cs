using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public sealed class StrategicSacrificeAbility : TargetNonselfFriendlyAbility
{
    private int StatIncreaseAmount = 15;

    public StrategicSacrificeAbility()
    {
        Name = "StrategicSacrifice";
        DisplayName = "Strategic Sacrifice";
        MaxCooldown = 0;
    }

    public override void InitAbility()
    {
        base.InitAbility();

        // Init based on abil rank, 

        OptionSelectChoice optSel = null;

        if(AbilityRank == 0)
        {
            optSel = new OptionSelectChoice() { Caption = "Choice", Options = (new string[] { "Destroy creature", "Give control of creature to Opponent", }).ToList() };
        }else if(AbilityRank == 2)
        {
            optSel = new OptionSelectChoice() { Caption = "Choice", Options = (new string[] { "Destroy creature and give opponent a double agent", "Destroy this and buff its stats by " + StatIncreaseAmount, }).ToList() };
        }
        
        if(optSel != null)
        {
            ChoicesNeeded.Add(optSel);
        }
    }

    public override void UpdateDescription()
    {
        if (AbilityRank == 0)
        {
            Description = "Choose a friendly creature. Choose one: Destroy it, or give control of it to your opponent.";
        }else if (AbilityRank == 1)
        {
            Description = "Choose a friendly creature. Destroy it and give your opponent a 1/1/1 Double Agent where it was.";
        }
        else
        {
            Description = "Choose a friendly creature. Choose one: Destroy it and give your opponent a 1/1/1 Double Agent, or destroy this and give its stats +" + StatIncreaseAmount + ".";
        }
    }

    public override void Trigger(object sender, EventArgs e)
    {
        if (ChoicesNeeded.Where(x => x.Caption == "Target").FirstOrDefault() is CreatureTargetChoice creatChoice && creatChoice.ChoiceMade)
        {
            var opp = Owner.MyGame.Players.Where(x => x != Owner.Controller).FirstOrDefault();
            if ((AbilityRank == 0 || AbilityRank == 2) && ChoicesNeeded.Where(x => x.Caption == "Choice").FirstOrDefault() is OptionSelectChoice optChoice && optChoice.ChoiceMade)
            {
                if (AbilityRank == 0)
                {
                    if (optChoice.ChosenOption == "Destroy creature")
                    {
                        creatChoice.TargetCreature.Die();
                    }
                    else
                    {
                        if (opp != null)
                        {
                            creatChoice.TargetCreature.SetController(opp);
                        }
                    }
                }
                else
                {
                    if (optChoice.ChosenOption == "Destroy creature and give opponent a double agent")
                    {
                        //target dies, summon double agent, give control to opponent.
                        DestroyAndSummonAgent(creatChoice.TargetCreature, opp);

                    }
                    else
                    {
                        Owner.Die();
                        creatChoice.TargetCreature.StatsChange(StatIncreaseAmount, StatIncreaseAmount, StatIncreaseAmount);
                    }
                }
            }
            else
            {
                //target dies, summon double agent, give control to opponent.
                DestroyAndSummonAgent(creatChoice.TargetCreature, opp);
            }

        }
    }

    public override void RankUpToOne()
    {
        // INIT COMES AFTER RANK UP
        /*var oldOptSel = ChoicesNeeded.Where(x => x.Caption == "Choice").FirstOrDefault();

        if (oldOptSel != null)
        {
            ChoicesNeeded.Remove(oldOptSel);
        }*/
    }

    public override void RankUpToTwo()
    {
        /*var optSel = new OptionSelectChoice() { Caption = "Choice", Options = (new string[] { "Destroy creature and give opponent a double agent", "Destroy this and buff its stats by 10", }).ToList() };


        var oldOptSel = ChoicesNeeded.Where(x => x.Caption == "Choice").FirstOrDefault();

        if (oldOptSel != null)
        {
            ChoicesNeeded.Remove(oldOptSel);
        }

        ChoicesNeeded.Add(optSel);*/
    }

    private Creature GetDoubleAgent(Player newOwner)
    {
        var cbase = new CreatureGameBase()
        {
            Initiative = 1,
            Health = 1,
            Attack = 1,
            Speed = 1,
            DisplayName = "Double Agent",
        };

        var creat = new Creature(cbase);
        creat.InitFromBase();
        creat.SetController(newOwner);

        return creat;
    }

    private void DestroyAndSummonAgent(Creature targetCreat, Player opp)
    {
        var targetSquare = targetCreat.MySpace;
        targetCreat.Die();
        Owner.MyGame.SummonCreature(GetDoubleAgent(opp), targetSquare);
    }
}

using System.Collections.Generic;

public abstract class CharacterRollDecorator : ICharacterRoll
{
    protected ICharacterRoll _decoratedRoll;

    public CharacterRollDecorator(ICharacterRoll decoratedRoll)
    {
        _decoratedRoll = decoratedRoll;
    }

    public virtual Dictionary<Rarity, int> GetRarityWeights()
    {
        return _decoratedRoll.GetRarityWeights();
    }

    public virtual Dictionary<CosmeticType, int> GetCosmeticWeights()
    {
        return _decoratedRoll.GetCosmeticWeights();
    }

    public virtual int[] GetRankWeights()
    {
        return _decoratedRoll.GetRankWeights();
    }

    public virtual int[] GetLevelWeights()
    {
        return _decoratedRoll.GetLevelWeights();
    }

    public virtual Dictionary<Rarity, List<ScriptableCharacterBase>> GetRollableCharacters()
    {
        return _decoratedRoll.GetRollableCharacters();
    }

    public virtual CollectionCharacter MakeRoll(ICharacterRoll roll)
    {
        return _decoratedRoll.MakeRoll(roll);
    }

    public virtual (ScriptableCharacterBase cha, int weight)[] GetCharacterWeights(IEnumerable<ScriptableCharacterBase> Characters)
    {
        return _decoratedRoll.GetCharacterWeights(Characters);
    }
}


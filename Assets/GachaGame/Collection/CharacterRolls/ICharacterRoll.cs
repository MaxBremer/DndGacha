using System.Collections;
using System.Collections.Generic;

public interface ICharacterRoll
{
    Dictionary<Rarity, int> GetRarityWeights();

    Dictionary<CosmeticType, int> GetCosmeticWeights();

    int[] GetRankWeights();

    int[] GetLevelWeights();

    (ScriptableCharacterBase cha, int weight)[] GetCharacterWeights(IEnumerable<ScriptableCharacterBase> Characters);

    Dictionary<Rarity, List<ScriptableCharacterBase>> GetRollableCharacters();

    CollectionCharacter MakeRoll(ICharacterRoll getStatssFrom);
}
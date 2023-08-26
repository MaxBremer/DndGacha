using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tag
{
    public Tag(CreatureTag tag)
    {
        TagType = tag;
    }

    public CreatureTag TagType;

    public Creature CreatureData = null;

    public int IntData = -1;

    public Tag Copy()
    {
        var retTag = new Tag(TagType) { IntData = IntData, CreatureData = CreatureData };
        return retTag;
    }
}

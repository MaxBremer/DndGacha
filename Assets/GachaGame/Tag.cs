using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tag
{
    public Tag(CreatureTag tag)
    {
        TagType = tag;
    }

    public Tag(CreatureTag tag, object data)
    {
        TagType = tag;
        Data = data;
    }

    public object Data;

    public CreatureTag TagType;

    public Tag Copy()
    {
        return new Tag(TagType, Data);
    }
}

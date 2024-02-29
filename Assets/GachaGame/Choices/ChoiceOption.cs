using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ChoiceOption
{
    public string OptionName { get; set; }
    public object AssociatedObject { get; set; }
    public Func<Ability, bool> ConditionOfPresentation { get; set; } = x => true;

    public static bool operator ==(ChoiceOption opt, string str)
    {
        return opt.OptionName == str;
    }

    public static bool operator !=(ChoiceOption opt, string str)
    {
        return opt.OptionName != str;
    }

    public override bool Equals(object obj)
    {
        if (obj is string s)
        {
            return OptionName == s;
        }
        else
        {
            return base.Equals(obj);
        }
    }

    public override string ToString()
    {
        return OptionName;
    }

    public override int GetHashCode()
    {
        int hashCode = -1996282033;
        hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(OptionName);
        hashCode = hashCode * -1521134295 + EqualityComparer<object>.Default.GetHashCode(AssociatedObject);
        return hashCode;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISkill
{
    public void UseSkill();
    public bool IsSkillCanUse();
    public void UnlockSkill();
    public bool IsSkillUnlocked();
}

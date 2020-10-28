using System.Collections;
using System.Collections.Generic;

namespace RPG.Stats
{
    interface IModifierProvider
    {
        int GetAdditiveModifiers();
        int GetPercentageModifiers();
    }
}
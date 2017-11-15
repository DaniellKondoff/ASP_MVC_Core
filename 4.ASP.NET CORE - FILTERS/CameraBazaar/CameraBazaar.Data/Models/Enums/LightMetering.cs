using System;

namespace CameraBazaar.Data.Models.Enums
{
    [Flags]
    public enum LightMetering
    {
        Spot = 1,
        CenterWeighted = 2,
        Evaluative = 4
    }
}

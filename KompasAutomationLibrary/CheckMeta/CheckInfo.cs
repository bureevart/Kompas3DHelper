using System;
using System.ComponentModel;

namespace KompasAutomationLibrary.CheckMeta
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public record CheckInfo(string Display, Enum FlagValue, DocKind Kind, bool Implemented = true);
    
}
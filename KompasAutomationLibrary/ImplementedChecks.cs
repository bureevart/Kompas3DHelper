using System.Collections.Generic;
using Kompas3DAutomation.Checks.AssemblyChecks;
using Kompas3DAutomation.Checks.DrawingChecks;
using Kompas3DAutomation.Checks.Part3DChecks;
using KompasAutomationLibrary.CheckMeta;

namespace KompasAutomationLibrary
{
    public class ImplementedChecks
    {
        public static readonly List<CheckInfo> AllChecks = new()
        {
            new("Скрытые объекты",  CheckDrawing.DrawingChecks.NoHiddenObjects,          DocKind.Drawing2D),
            new("Ручные размеры",   CheckDrawing.DrawingChecks.ManualTextDimensionChanges, DocKind.Drawing2D),

            new("Скрытые компоненты", CheckAssembly.AssemblyChecks.HiddenObjectsPresent, DocKind.Assembly),

            new("Скрытые объекты (3D)", CheckPart3D.Part3DChecks.HiddenObjectsPresent,   DocKind.Part3D),
            new("Самопересечение граней", CheckPart3D.Part3DChecks.SelfIntersectionOfFaces, DocKind.Part3D)
        };
        
        public static readonly Dictionary<DocKind, string> KindDisplay = new()
        {
            { DocKind.Drawing2D, "2D-чертёж" },
            { DocKind.Assembly, "Сборка" },
            { DocKind.Part3D, "3D-деталь" }
            
        };
    }
}
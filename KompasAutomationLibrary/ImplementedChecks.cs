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
            new("Проверка на отсутствие скрытых объектов в поле чертежа", CheckDrawing.DrawingChecks.NoHiddenObjects, DocKind.Drawing2D),
            new("Проверка размеров на ручное внесение текстовых изменений", CheckDrawing.DrawingChecks.ManualTextDimensionChanges, DocKind.Drawing2D),
            new("Проверка расположения объектов на заданных слоях", CheckDrawing.DrawingChecks.LayerObjectsPosition, DocKind.Drawing2D),


            new("Проверка на наличие скрытых компонентов", CheckAssembly.AssemblyChecks.HiddenObjectsPresent, DocKind.Assembly),

            new("Наличие скрытых объектов", CheckPart3D.Part3DChecks.HiddenObjectsPresent, DocKind.Part3D),
            new("Проверка на наличие более одного твердого тела", CheckPart3D.Part3DChecks.SingleSolidBody, DocKind.Part3D)
        };
        
        public static readonly Dictionary<DocKind, string> KindDisplay = new()
        {
            { DocKind.Drawing2D, "2D-чертёж" },
            { DocKind.Assembly, "Сборка" },
            { DocKind.Part3D, "3D-деталь" }
            
        };
    }
}
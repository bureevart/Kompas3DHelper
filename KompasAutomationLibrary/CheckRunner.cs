using System;
using System.Linq;
using Kompas3DAutomation;
using Kompas3DAutomation.Checks.AssemblyChecks;
using Kompas3DAutomation.Checks.DrawingChecks;
using Kompas3DAutomation.Checks.Part3DChecks;
using Kompas3DAutomation.Results;
using KompasAutomationLibrary.CheckMeta;

namespace KompasAutomationLibrary;

/// <summary>Результат запуска: отчёт и действие «очистить подсветку».</summary>
public readonly record struct CheckRunResult(CheckReport Report, Action Clear);

public static class CheckRunner
{
    public static CheckRunResult Run(KompasConnectionObject conn,
        DocKind kind,
        long bits)
        => kind switch
        {
            DocKind.Drawing2D => RunChecks(new CheckDrawing(conn),
                (CheckDrawing.DrawingChecks)bits),

            DocKind.Assembly => RunChecks(new CheckAssembly(conn),
                (CheckAssembly.AssemblyChecks)bits),
            
            DocKind.Part3D => RunChecks(new CheckPart3D(conn),
                (CheckPart3D.Part3DChecks)bits),

            _ => throw new ArgumentOutOfRangeException(nameof(kind))
        };

    /// <summary>
    /// Возвращает битовую маску всех проверок, реализованных для данного вида документа.
    /// </summary>
    public static long GetValidMask(DocKind kind)
    {
        return kind switch
        {
            DocKind.Drawing2D => Enum.GetValues(typeof(CheckDrawing.DrawingChecks))
                .Cast<CheckDrawing.DrawingChecks>()
                .Aggregate(0L, (acc, f) => acc | Convert.ToInt64(f)),

            DocKind.Part3D => Enum.GetValues(typeof(CheckPart3D.Part3DChecks))
                .Cast<CheckPart3D.Part3DChecks>()
                .Aggregate(0L, (acc, f) => acc | Convert.ToInt64(f)),

            DocKind.Assembly => Enum.GetValues(typeof(CheckAssembly.AssemblyChecks))
                .Cast<CheckAssembly.AssemblyChecks>()
                .Aggregate(0L, (acc, f) => acc | Convert.ToInt64(f)),

            _ => 0L
        };
    }
    
    private static CheckRunResult RunChecks<TEnum>(dynamic checker, TEnum flags)
        where TEnum : struct
    {
        var rpt = checker.CheckForActiveDocument(flags);
        Action clear = () => checker.ClearHighlightForActiveDocument();
        return new CheckRunResult(rpt, clear);
    }
}
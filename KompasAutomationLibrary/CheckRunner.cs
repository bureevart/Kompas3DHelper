using System;
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

    /* универсальный хелпер */
    private static CheckRunResult RunChecks<TEnum>(dynamic checker, TEnum flags)
        where TEnum : struct
    {
        var rpt = checker.CheckForActiveDocument(flags);
        Action clear = () => checker.ClearHighlightForActiveDocument();
        return new CheckRunResult(rpt, clear);
    }
}
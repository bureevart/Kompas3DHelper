using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Kompas3DAutomation.Results
{
    /// Один конкретный дефект
    [EditorBrowsable(EditorBrowsableState.Never)]
    public record CheckViolation
    (
         string CheckName,          // «NoHiddenObjects», «ViewIntegrity»…
         string Message,            // Текст для отчёта
         object TargetObject,       // Любой API‑объект Kompas, нужен для подсветки
         Action Highlighter = null  // Как подсветить (опционально)
    );

    /// Итог по файлу / документу
    public class CheckReport
    {
        public List<CheckViolation> Violations { get; } = new();
        public bool HasErrors => Violations.Count > 0;

        public override string ToString() => HasErrors
            ? string.Join(Environment.NewLine, Violations.Select(v => $"{v.CheckName}: {v.Message}"))
            : "Ошибок не обнаружено";

        public static CheckReport ConnectionError() => new CheckReport
        {
            Violations = { new CheckViolation("Connection", "Ошибка подключения к Kompas3D", null, null) }
        };
    }

}

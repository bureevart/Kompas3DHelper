using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kompas3DAutomation.Checks.DrawingChecks;
using Kompas3DAutomation.Results;
using Kompas6API5;
using KompasAPI7;

namespace Kompas3DAutomation.Checks.AssemblyChecks
{
    /// <summary>
    /// Проверка отсутствия скрытых компонентов в сборке.
    /// Логика рекурсии из вашего статического кода, только bool → yield return.
    /// </summary>
    internal sealed class HiddenObjectsChecker : IChecker
    {
        private readonly KompasObject _kompas;
        private readonly IAssemblyDocument _asmDoc;

        public HiddenObjectsChecker(KompasObject kompas, IAssemblyDocument asmDoc)
        {
            _kompas = kompas;
            _asmDoc = asmDoc;
        }

        public IEnumerable<CheckViolation> Run()
        {
            // ChooseManager есть только в 3D‑API7
            var chooser = _asmDoc.ChooseManager;

            // Собираем всё дерево деталей/сборок
            foreach (var part in GetAllParts(_asmDoc.TopPart))
            {
                if (part.Hidden)
                {
                    yield return new CheckViolation(
                        CheckName: $"{nameof(CheckAssembly.AssemblyChecks.HiddenObjectsPresent)}",
                        Message: $"Компонент «{part.Name}» скрыт",
                        TargetObject: part,
                        Highlighter: () => chooser.Choose(part)
                    );
                }
            }
        }

        /// <summary>
        /// Рекурсивный перебор всех IPart7: текущая + вложенные сборки.
        /// </summary>
        private static IEnumerable<IPart7> GetAllParts(IPart7 root)
        {
            yield return root;

            foreach (IPart7 child in root.Parts)
            {
                yield return child;
                // если это сама сборка, забираем её детей
                if (!child.Detail)
                    foreach (var desc in GetAllParts(child))
                        yield return desc;
            }
        }

        #region Obsolete
        [Obsolete]
        private static bool CheckHiddenObjects(KompasObject kompasObject, IPart7 part)
        {
            var details = GetDetailsAndAssembliesRecursive(part);

            foreach (var detail in details)
            {
                Console.WriteLine($"{detail.Name}");
                if(detail.Hidden)
                    return false;
            }

            return true;
        }

        [Obsolete]
        private static List<IPart7> GetDetailsAndAssembliesRecursive(IPart7 part)
        {
            var parts = new List<IPart7>
            {
                part
            };

            foreach(IPart7 item in part.Parts)
            {
                if (item.Detail == true) parts.Add(item);
                if (item.Detail == false)
                    parts.AddRange(GetDetailsAndAssembliesRecursive(item));
            }

            return parts;

        }
        #endregion
    }
}

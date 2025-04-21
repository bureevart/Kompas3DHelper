﻿using Kompas3DAutomation.Checks;
using Kompas3DAutomation.Checks.DrawingChecks;
using Kompas3DAutomation.Checks.Part3DChecks;
using Kompas3DAutomation.Results;
using Kompas6Constants;
using KompasAPI7;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kompas3DAutomation.Checks.AssemblyChecks
{
    public class CheckAssembly : CheckBase
    {
        public CheckAssembly(KompasConnectionObject kompasConnectionObject)
            : base(kompasConnectionObject) { }

        /// <summary>
        /// Проверяет сборку по пути (открывает/закрывает документ).
        /// </summary>
        public CheckReport Check(string path, AssemblyChecks checks)
        {
            if (!_kompasObject.IsConnected)
                return CheckReport.ConnectionError();

            var app = (IApplication)_kompasObject.Kompas.ksGetApplication7();
            app.Documents.Open(path, true, true);
            var asmDoc = (IAssemblyDocument)app.ActiveDocument;

            try
            {
                return RunChecks(asmDoc, checks);
            }
            finally
            {
                asmDoc.Close(DocumentCloseOptions.kdDoNotSaveChanges);
            }
        }

        /// <summary>
        /// Проверяет уже открытый активный документ‑сборку.
        /// </summary>
        public CheckReport CheckForActiveDocument(AssemblyChecks checks)
        {
            if (!_kompasObject.IsConnected)
                return CheckReport.ConnectionError();

            var app = (IApplication)_kompasObject.Kompas.ksGetApplication7();
            var active = app.ActiveDocument as IAssemblyDocument;
            if (active == null)
                throw new InvalidOperationException("Нет активного 3D‑документа сборки.");

            return RunChecks(active, checks);
        }

        /// <summary>
        /// Бежим по всем чекерам‑модулям.
        /// </summary>
        private CheckReport RunChecks(IAssemblyDocument asmDoc, AssemblyChecks checks)
        {
            var report = new CheckReport();

            // локальный добавлятор для любого IChecker
            void Add(IChecker ch) => report.Violations.AddRange(ch.Run());

            //if (checks.HasFlag(AssemblyChecks.PartInterference))
            //    Add(new PartInterferenceChecker(_kompasObject.Kompas, asmDoc));

            if (checks.HasFlag(AssemblyChecks.HiddenObjectsPresent))
                Add(new HiddenObjectsChecker(_kompasObject.Kompas, asmDoc));

            return report;
        }

        #region CheckTypes
        /// <summary>
        /// Проверки для моделей СЕ (сборочной единицы).
        /// </summary>
        [Flags]
        public enum AssemblyChecks
        {
            None = 0,
            /// <summary>
            /// Врезание деталей не допускается (анализ зазоров по сопряженным поверхностям).
            /// </summary>
            PartInterference = 1 << 0,
            /// <summary>
            /// Наличие скрытых объектов (компонентов).
            /// </summary>
            HiddenObjectsPresent = 1 << 1
        }
        #endregion
    }
}

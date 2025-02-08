using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kompas3DAutomation.Results
{
    public static class CheckResultExtensions
    {
        public static string GetMessage(this CheckResults checkResults)
        {
            switch (checkResults)
            {
                case CheckResults.NoErrors:
                    return "Ошибок нет";
                case CheckResults.ConnectionError:
                    return "Ошибка подключения к Kompas3D";
                case CheckResults.Error:
                    return "Ошибка проверки";
                default:
                    return string.Empty;
            }
        }
    }
}

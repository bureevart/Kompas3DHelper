using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Kompas3DAutomation.Results;

namespace KompasAutomationLibrary
{
    // Создаём один раз и кешируем чтобы не плодить окна
    public class CheckReportForm : Form
    {
        private readonly DataGridView _grid = new DataGridView();

        public CheckReportForm()
        {
            Text = "Результаты проверки";
            Width = 600; Height = 400;

            _grid.Dock = DockStyle.Fill;
            _grid.AutoGenerateColumns = false;
            _grid.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Message",
                HeaderText = "Сообщение",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });
            Controls.Add(_grid);
        }

        public void Bind(CheckReport report)
        {
            _grid.DataSource = report.Violations;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            _grid.CellDoubleClick += (_, ev) =>
            {
                if (ev.RowIndex >= 0)
                {
                    var vio = (CheckViolation)_grid.Rows[ev.RowIndex].DataBoundItem;
                    vio.Highlighter?.Invoke();
                }
            };
        }
    }

}

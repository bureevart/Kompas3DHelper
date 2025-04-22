using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Kompas3DAutomation.Results;

namespace KompasAutomationLibrary
{
    public sealed class CheckReportForm : Form
    {
        private readonly ListView _list = new ListView();
        private readonly Button _btnClear = new Button();
        private readonly Button _btnSave = new Button();
        private int _hoverIndex = -1;
        private CheckReport _currentReport;

        public Action ClearHighlightAction { get; set; }

        public CheckReportForm()
        {
            Text = "Результаты проверки";
            MinimumSize = new Size(600, 400);
            StartPosition = FormStartPosition.CenterParent;

            /* ---------- Панель кнопок снизу ---------- */
            var panel = new Panel { Dock = DockStyle.Bottom, Height = 34 };
            Controls.Add(panel);

            _btnClear.Text = "Очистить подсветку";
            _btnClear.AutoSize = true;
            _btnClear.Anchor = AnchorStyles.Left | AnchorStyles.Top;
            _btnClear.Click += (_, __) => ClearHighlightAction?.Invoke();
            panel.Controls.Add(_btnClear);

            _btnSave.Text = "Сохранить в Excel";
            _btnSave.AutoSize = true;
            _btnSave.Left = _btnClear.Right + 10;
            _btnSave.Click += SaveToExcel_Click;
            panel.Controls.Add(_btnSave);

            /* ---------- ListView ---------- */
            _list.Dock = DockStyle.Fill;
            _list.View = View.Details;
            _list.FullRowSelect = true;
            _list.GridLines = false;
            _list.HeaderStyle = ColumnHeaderStyle.Nonclickable;
            _list.Columns.Add("Сообщение", -2);
            _list.OwnerDraw = true;
            _list.DrawColumnHeader += (s, e) => e.DrawDefault = true;
            _list.DrawItem += DrawItem;
            _list.MouseMove += List_MouseMove;
            _list.MouseLeave += (_, __) => { _hoverIndex = -1; _list.Invalidate(); };
            _list.ItemActivate += (_, __) =>
            {
                if (_list.SelectedItems.Count != 1) return;
                if (_list.SelectedItems[0].Tag is CheckViolation v)
                    v.Highlighter?.Invoke();
            };
            Controls.Add(_list);
        }

        /* ---------- Публичное связывание данных ---------- */
        public void Bind(CheckReport rep)
        {
            _currentReport = rep;
            _list.BeginUpdate();
            _list.Items.Clear();
            _list.Groups.Clear();

            foreach (var g in rep.Violations.GroupBy(v => v.CheckName).OrderBy(g => g.Key))
            {
                var grp = new ListViewGroup(g.Key) { Header = g.Key };
                _list.Groups.Add(grp);

                foreach (var v in g)
                    _list.Items.Add(new ListViewItem(v.Message, grp) { Tag = v });
            }
            _list.AutoResizeColumn(0, ColumnHeaderAutoResizeStyle.ColumnContent);
            _list.EndUpdate();
        }

        /* ---------- Отрисовка строки и разделителей ---------- */
        private void DrawItem(object _, DrawListViewItemEventArgs e)
        {
            bool hover = e.ItemIndex == _hoverIndex;
            var br = new SolidBrush(hover ? Color.LightBlue : Color.White);
            e.Graphics.FillRectangle(br, e.Bounds);
            e.DrawText(TextFormatFlags.Left | TextFormatFlags.VerticalCenter);
            bool lastInGroup = e.Item.Index == _list.Items.Count - 1 ||
                               e.Item.Group != _list.Items[e.Item.Index + 1].Group;
            if (lastInGroup)
            {
                int y = e.Bounds.Bottom - 1;
                var pen = new Pen(Color.LightGray);
                e.Graphics.DrawLine(pen, e.Bounds.Left, y, e.Bounds.Right, y);
                pen.Dispose();
            }
            br.Dispose();
        }
        private void List_MouseMove(object _, MouseEventArgs e)
        {
            int idx = _list.HitTest(e.Location).Item?.Index ?? -1;
            if (idx != _hoverIndex) { _hoverIndex = idx; _list.Invalidate(); }
        }

        /* ---------- Сохранить в CSV (Excel) ---------- */
        private void SaveToExcel_Click(object sender, EventArgs e)
        {
            if (_currentReport == null || _currentReport.Violations.Count == 0)
            {
                MessageBox.Show(this, "Нет данных для экспорта.", "Сохранение", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            var dlg = new SaveFileDialog
            {
                Filter = "CSV файл|*.csv",
                FileName = "KompasReport.csv"
            };
            if (dlg.ShowDialog(this) != DialogResult.OK) return;

            var sb = new StringBuilder();
            sb.AppendLine("Тип проверки;Сообщение");
            foreach (var v in _currentReport.Violations)
                sb.AppendLine($"{v.CheckName};{v.Message.Replace(';', ',')}");

            File.WriteAllText(dlg.FileName, sb.ToString(), Encoding.UTF8);
            MessageBox.Show(this, "Отчёт сохранён.", "Сохранение", MessageBoxButtons.OK, MessageBoxIcon.Information);
            dlg.Dispose();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            e.Cancel = true; Hide();
        }
    }
}

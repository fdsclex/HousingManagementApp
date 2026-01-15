using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;           // ← обязательно для SelectionChangedEventArgs
using System.Data.Entity;                // ← для Include

namespace HousingManagementApp
{
    public partial class HistoryWindow : Window
    {
        private readonly Entities _context;  // предполагаем, что это ваш DbContext
        private readonly bool _isByEmployee;

        public HistoryWindow(Entities context, bool isByEmployee)
        {
            InitializeComponent();
            _context = context;
            _isByEmployee = isByEmployee;

            TitleText.Text = _isByEmployee
                ? "История по Сотрудникам"
                : "История по Адресам";

            LoadFilter();
        }

        private void LoadFilter()
        {
            if (_isByEmployee)
            {
                FilterCombo.ItemsSource = _context.Employee.ToList();
                FilterCombo.DisplayMemberPath = "FullName";
                FilterCombo.SelectedValuePath = "Id";
            }
            else
            {
                FilterCombo.ItemsSource = _context.Building.ToList();
                FilterCombo.DisplayMemberPath = "Address";
                FilterCombo.SelectedValuePath = "Id";
            }
        }

        private void FilterCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (FilterCombo.SelectedValue == null) return;

            var filterId = (int)FilterCombo.SelectedValue;

            // В EF6 самый надёжный способ для нескольких уровней — строковый Include
            var query = _context.RequestExecution
                .Include("Request.Building")                     // Request → Building
                .Include("WorkingGroups.Employee")               // RequestExecution → WorkingGroups → Employee
                .Include("Request")                              // на всякий случай сам Request
                .AsQueryable();

            if (_isByEmployee)
            {
                query = query.Where(re =>
                    re.WorkingGroup.Any(wg => wg.EmployeeId == filterId));
            }
            else
            {
                query = query.Where(re =>
                    re.Request != null && re.Request.BuildingId == filterId);
            }

            // Материализация результата
            var result = query.ToList();

            HistoryGrid.ItemsSource = result;
        }
    }
}
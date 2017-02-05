using System.Windows;

namespace KadrWPF.Table
{
    /// <summary>
    /// Логика взаимодействия для Hours_By_Period.xaml
    /// </summary>
    public partial class Hours_By_Period : Window
    {
        public Hours_By_Period(OracleDataTable dtHoursPeriod)
        {
            InitializeComponent();
            dgHoursCalc.DataContext = dtHoursPeriod.DefaultView;
        }
    }
}

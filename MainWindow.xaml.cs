using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ModbusConverter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void TbValue_TextChanged(object sender, TextChangedEventArgs e)
        {
            EvalValues();
        }

        private void EvalValues()
        {
            if (TbLong == null)
                return;

            if (TbHighValue ==  null || !int.TryParse(TbHighValue.Text, out var highValue))
                highValue = 0;

            if (TbLowValue == null || !int.TryParse(TbLowValue.Text, out var lowValue))
                lowValue = 0;

            // Long result
            var merge = unchecked(BitConverter.IsLittleEndian
                ? (ushort)highValue << 16 | (ushort)lowValue
                : (ushort)lowValue << 16 | (ushort)highValue);

            TbLong.Text = merge.ToString();
            TbULong.Text = unchecked(((uint)merge).ToString());

            // ASCII result
            var lowAscii = unchecked(BitConverter.GetBytes((ushort)lowValue));
            var highAscii = unchecked(BitConverter.GetBytes((ushort)highValue));

            TbAscii.Text = System.Text.Encoding.ASCII.GetString(lowAscii) + " - " + System.Text.Encoding.ASCII.GetString(highAscii);
        }
    }
}

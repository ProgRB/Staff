﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Threading;

namespace Tabel
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            bool oneonly = true;
            // указываем имя своей программы. 
            Mutex m = new Mutex(true, "FormMenu", out oneonly);
            if (oneonly)
            {
                Elegant.Ui.RibbonLicenser.LicenseKey = "5DB0-D11C-A582-4F1E-6178-7F38-AFFB-8021";
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
            }
            else
            {
                MessageBox.Show("Это приложение уже запущено.", "АРМ \"Учет рабочего времени\"", 
                    MessageBoxButtons.OK);
            }
        }
    }
}
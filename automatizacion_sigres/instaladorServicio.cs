using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.ServiceProcess;
using System.Threading.Tasks;

namespace automatizacion_sigres
{
    [RunInstaller(true)]
    public partial class instaladorServicio : System.Configuration.Install.Installer
    {
        public instaladorServicio()
        {
            InitializeComponent();
        }
        private void ServiceInstaller_AfterInstall(object sender, System.Configuration.Install.InstallEventArgs e)
        {
            ServiceController sc = new ServiceController("Servicio Automatizacion SIGRES");
            sc.Start();
        }
    }
}

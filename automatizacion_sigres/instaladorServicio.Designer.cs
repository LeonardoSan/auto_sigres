
using System.ServiceProcess;

namespace automatizacion_sigres
{
    partial class instaladorServicio
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            this.serviceInstaller = new System.ServiceProcess.ServiceInstaller();
            this.serviceProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();

            this.serviceProcessInstaller.Account = ServiceAccount.LocalSystem;

            this.serviceInstaller.ServiceName = "Telefonica Autoremediación Clientes No Navega (OLT)";
            this.serviceInstaller.Description = "Este servicio permite identificar los clientes con afectación en el servicio de BA-FTTH que no reportan MAC en OLT's con estado working";
            this.serviceInstaller.StartType = System.ServiceProcess.ServiceStartMode.Manual;
            this.serviceInstaller.AfterInstall += ServiceInstaller_AfterInstall;

            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.serviceProcessInstaller,
            this.serviceInstaller});
        }

        #endregion

        private System.ServiceProcess.ServiceInstaller serviceInstaller;
        private System.ServiceProcess.ServiceProcessInstaller serviceProcessInstaller;
    }
}
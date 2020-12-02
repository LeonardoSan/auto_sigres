using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace automatizacion_sigres
{
    public partial class auto_sigres : ServiceBase
    {
        public auto_sigres()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            System.Threading.Thread.Sleep(10000);
            ProgramarProximaEjecucion();

        }

        protected override void OnStop()
        {
            if (timer_inicio_servicio != null)
            {
                timer_inicio_servicio.Stop();
                timer_inicio_servicio.Close();
                timer_inicio_servicio.Dispose();
                timer_inicio_servicio = null;
            }

            if (countdownInsumo != null)
            {
                countdownInsumo.Dispose();
                countdownInsumo = null;
            }
        }

        private void ProgramarProximaEjecucion()
        {
            if (timer_inicio_servicio != null)
            {
                timer_inicio_servicio.Stop();
                timer_inicio_servicio.Close();
                timer_inicio_servicio.Dispose();
            }

            timer_inicio_servicio = new System.Timers.Timer(CalcularProximaEjecucion("13:00:00"));//PARAMETRIZAR BD
            timer_inicio_servicio.AutoReset = false;
            timer_inicio_servicio.Elapsed += Timer_ejecucion_Elapsed;
            timer_inicio_servicio.Start();
        }

        protected double CalcularProximaEjecucion(string timer_min_hour)
        {
            DateTime FechaHoraActual = DateTime.Now;
            DateTime FechaHoraMin = FechaHoraActual.AddTicks(FechaHoraActual.TimeOfDay.Ticks * -1);

            FechaHoraMin = FechaHoraMin.AddHours(Convert.ToDouble(timer_min_hour.Substring(0, 2)));
            FechaHoraMin = FechaHoraMin.AddMinutes(Convert.ToDouble(timer_min_hour.Substring(3, 2)));
            FechaHoraMin = FechaHoraMin.AddSeconds(Convert.ToDouble(timer_min_hour.Substring(6, 2)));

            if (FechaHoraActual > FechaHoraMin)
            {
                FechaHoraMin = FechaHoraMin.AddDays(1);
            }
            return (FechaHoraMin - FechaHoraActual).TotalMilliseconds;
        }
        private void Timer_ejecucion_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            Ejecutar_Logica();
        }

        public void Ejecutar_Logica()
        {
            //Aquí va tu logica


            ProgramarProximaEjecucion();//Volver a programar proxima ejecución
        }
    }
}

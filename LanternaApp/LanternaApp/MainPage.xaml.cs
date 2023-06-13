using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Essentials;
using Plugin.Battery.Abstractions;
using Plugin.Battery;
using System.Linq.Expressions;

namespace LanternaApp
{
    public partial class MainPage : ContentPage
    {
        bool lanterna_ligada = false;

        public MainPage()
        {
            InitializeComponent();


            btnOnOff.Source = ImageSource.FromResource(
                "btoff");

            Carrega_Info_Bateria();
        }

        private void btnOnOff_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (lanterna_ligada == false)
                {
                    Flashlight.TurnOnAsync();
                    btnOnOff.Source = ("bton");
                    lanterna_ligada = true;
                }
                else
                {
                    Flashlight.TurnOffAsync();
                    btnOnOff.Source = ("btoff");
                    lanterna_ligada = false;
                }

            }
            catch (Exception ex)
            {
                DisplayAlert("Ops", "Não foi possível acessar a lanterna", "OK :(");
            }
            finally
            {
                Vibration.Vibrate(TimeSpan.FromMilliseconds(500));
            }

        }
        private void Mudanca_Status_Bateria(object sender, BatteryChangedEventArgs e)
        {
            try
            {
                //Carga Restante
                lbl_carga_restante.Text = e.RemainingChargePercent.ToString() + "%";

                //Status da Bateria (carregando, etec)
                switch (e.Status)
                {

                    case BatteryStatus.Charging:
                        lbl_status_bateria.Text = "Carregando";
                        break;

                    case BatteryStatus.Discharging:
                        lbl_status_bateria.Text = "Descarregando";
                        break;

                    case BatteryStatus.Full:
                        lbl_status_bateria.Text = "Carga Completa";
                        break;

                    case BatteryStatus.NotCharging:
                        lbl_status_bateria.Text = "Sem carregar";
                        break;

                    case BatteryStatus.Unknown:
                        lbl_status_bateria.Text = "Desconhecido";
                        break;

                }


                // Fonte de energia do dispositivo
                switch (e.PowerSource)
                {
                    case PowerSource.Ac:
                        lbl_status_bateria.Text = "Carregador";
                        break;

                    case PowerSource.Battery:
                        lbl_fonte_energia.Text = "Bateria";
                        break;

                    case PowerSource.Other:
                        lbl_fonte_energia.Text = "USB";
                        break;

                    case PowerSource.Wireless:
                        lbl_fonte_energia.Text = "Sem fio";
                        break;
                }
            }
            catch (Exception ex)
            {
                DisplayAlert("Ops", "Não foi possível obter dados da bateria", "OK");
            }

        }
        private async void Carrega_Info_Bateria()
        {
            try
            {
                if (CrossBattery.IsSupported)
                {
                    CrossBattery.Current.BatteryChanged -= Mudanca_Status_Bateria;
                    CrossBattery.Current.BatteryChanged += Mudanca_Status_Bateria;
                }
                else
                {
                    throw new Exception("Não há suporte ao plugin de bateria");
                }
            }
            catch (Exception ex)
            {
                DisplayAlert("Ops", "Não foi possível obter dados da bateria", "OK");

            }
        }
    }
}

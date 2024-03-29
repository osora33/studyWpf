﻿using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WpfSmartHomeMonitoringApp.Helpers;

namespace WpfSmartHomeMonitoringApp.ViewModels
{
    class MainViewModel : Conductor<object>
    {
        public MainViewModel()
        {
            DisplayName = "SmartHome Monitoring v2.0";
        }

        protected override Task OnDeactivateAsync(bool close, CancellationToken cancellationToken)
        {
            if (Commons.MQTT_CLIENT.IsConnected)
            {
                Commons.MQTT_CLIENT.Disconnect();
                Commons.MQTT_CLIENT = null;
                //비활성화 처리
            }
            return base.OnDeactivateAsync(close, cancellationToken);
        }

        public void LoadDataBaseView()
        {
            ActivateItemAsync(new DataBaseViewModel());
        }

        public void LoadRealTimeView()
        {
            ActivateItemAsync(new RealTimeViewModel());
        }

        public void LoadHistoryView()
        {
            ActivateItemAsync(new HistoryViewModel());
        }

        public void ExitProgram()
        {
            Environment.Exit(0);
        }

        public void ExitToolBar()
        {
            Environment.Exit(0);
        }

        //Start 메뉴, 아이콘 눌렀을때 처리할 이벤트
        public void PopInfoDialog()
        {
            TaskPopup();
        }

        public void StartSubscribe()
        {
            TaskPopup();
        }

        private void TaskPopup()
        {
            //CustomPopupView
            var winManager = new WindowManager();
            var result = winManager.ShowDialogAsync(new CustomPopupViewModel("New Broker"));

            if (result.Result == true)
            {
                ActivateItemAsync(new DataBaseViewModel());    //화면전환
            }
        }

        public void PopInfoView()
        {
            var winManager = new WindowManager();
            winManager.ShowDialogAsync(new CustomInfoViewModel("About"));
        }
    }
}

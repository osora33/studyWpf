using Caliburn.Micro;
using OxyPlot;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfSmartHomeMonitoringApp.Helpers;
using WpfSmartHomeMonitoringApp.Models;

namespace WpfSmartHomeMonitoringApp.ViewModels
{
    public class HistoryViewModel : Screen
    {
        private BindableCollection<DivisionModel> divisions;
        private DivisionModel selectedDivision;
        private string startDate;
        private string initStartDate;
        private string endDate;
        private string initEndDate;
        private int totalCount;
        private PlotModel smartHomeModel;

        public HistoryViewModel()
        {
            Commons.CONNSTRING = "Data Source=localhost;Initial Catalog=OpenApiLab;Integrated Security=True";
            initControl();
        }

        private void initControl()
        {
            Divisions = new BindableCollection<DivisionModel>
            {
                new DivisionModel{KeyVal = 0, DivisionVal = "-- Select --" },
                new DivisionModel{KeyVal = 1, DivisionVal = "DINING" },
                new DivisionModel{KeyVal = 2, DivisionVal = "LIVING" },
                new DivisionModel{KeyVal = 3, DivisionVal = "BED" },
                new DivisionModel{KeyVal = 4, DivisionVal = "BATH" }
            };

            //Select를 선택해서 초기화
            SelectedDivision = Divisions.Where(v => v.DivisionVal.Contains("Select")).FirstOrDefault();

            InitStartDate = DateTime.Now.AddDays(-1).ToShortDateString();
            InitEndDate = DateTime.Now.AddDays(1).ToShortTimeString();
        }

        public BindableCollection<DivisionModel> Divisions
        {
            get => divisions; set
            {
                divisions = value;
                NotifyOfPropertyChange(() => Divisions);
            }
        }
        public DivisionModel SelectedDivision
        {
            get => selectedDivision; set
            {
                selectedDivision = value;
                NotifyOfPropertyChange(() => SelectedDivision);
            }
        }
        public string StartDate
        {
            get => startDate; set
            {
                startDate = value;
                NotifyOfPropertyChange(() => StartDate);
            }
        }
        public string InitStartDate
        {
            get => initStartDate; set
            {
                initStartDate = value;
                NotifyOfPropertyChange(() => InitStartDate);
            }
        }
        public string EndDate
        {
            get => endDate; set
            {
                endDate = value;
                NotifyOfPropertyChange(() => EndDate);
            }
        }
        public string InitEndDate
        {
            get => initEndDate; set
            {
                initEndDate = value;
                NotifyOfPropertyChange(() => InitEndDate);
            }
        }
        public int TotalCount
        {
            get => totalCount; set
            {
                totalCount = value;
                NotifyOfPropertyChange(() => TotalCount);
            }
        }
        public PlotModel SmartHomeModel
        {
            get => smartHomeModel; set
            {
                smartHomeModel = value;
                NotifyOfPropertyChange(() => SmartHomeModel);
            }
        }

        //검색 메서드
        public void SearchIoTData()
        {
            //Validation check
            if(SelectedDivision.KeyVal == 0)
            {
                MessageBox.Show("검색할 방을 선택하세요");
                return;
            }

            if(DateTime.Parse(StartDate) > DateTime.Parse(EndDate))
            {
                MessageBox.Show("시작일이 종료일보다 최신일 수 없습니다"); ;
                return;
            }

            TotalCount = 0;
            using (SqlConnection conn = new SqlConnection(Commons.CONNSTRING))
            {
                string strQuery = "select Id, CurrTime, Temp, Humid from TblSmartHome "
                    + "where DevId = @DevId and CurrTime between @StartDate and @EndDate "
                    + "order by Id";

                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(strQuery, conn);
                    SqlParameter parmDevId = new SqlParameter("@DevId", SelectedDivision.DivisionVal);
                    cmd.Parameters.Add(parmDevId);
                    SqlParameter parmStartDate = new SqlParameter("@StartDate", StartDate);
                    cmd.Parameters.Add(parmStartDate);
                    SqlParameter parmEndDate = new SqlParameter("@EndDate", EndDate);
                    cmd.Parameters.Add(parmEndDate);

                    SqlDataReader reader = cmd.ExecuteReader();

                    var i = 0;
                    while (reader.Read())
                    {
                        var temp = reader["Temp"];
                        //Temp, Humid 차트데이터를 생성

                        i++;
                    }
                    TotalCount = i;
                }
                catch (Exception ex)
                {

                    throw;
                }
            }
        }
    }
}

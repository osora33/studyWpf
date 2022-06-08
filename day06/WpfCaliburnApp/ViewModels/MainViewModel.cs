using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfCaliburnApp.Models;

namespace WpfCaliburnApp.ViewModels
{
    public class MainViewModel : Screen
    {
        //private EmployeesModel employee;
        private BindableCollection<EmployeesModel> listEmployees;
        public BindableCollection<EmployeesModel> ListEmployees 
        {
            get { return listEmployees; }
            set
            {
                listEmployees = value;
                NotifyOfPropertyChange(() => ListEmployees);
            }
        }
        string connString = "Data Source=localhost;Initial Catalog=OpenApiLab;Integrated Security=True";

        public MainViewModel()
        {
            GetEmployees();
        }

        public void GetEmployees()
        {
            //using 사용시 커넥션 자동으로 close해줌
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                string strQuery = "select * from TblEmployees";
                SqlCommand cmd = new SqlCommand(strQuery, conn);
                SqlDataReader reader = cmd.ExecuteReader();
                ListEmployees = new BindableCollection<EmployeesModel>();

                while (reader.Read())
                {
                    var temp = new EmployeesModel
                    {
                        Id = (int)reader["Id"],
                        EmpName = reader["EmpName"].ToString(),
                        Salary = (decimal)reader["Salary"],
                        DeptName = reader["DeptName"].ToString(),
                        Destination = reader["Destination"].ToString()
                    };
                    ListEmployees.Add(temp);
                }
            }

        }

        int id;
        public int Id
        {
            get { return id; }
            set
            {
                id = value;
                NotifyOfPropertyChange(() => Id);
                NotifyOfPropertyChange(() => CanDelEmployee);
            }
        }

        string empName;
        public string EmpName
        {
            get { return empName; }
            set 
            { 
                empName = value;
                NotifyOfPropertyChange(() => EmpName);
                NotifyOfPropertyChange(() => CanSaveEmployee);
            }
        }

        decimal salary;
        public decimal Salary
        {
            get { return salary; }
            set
            {
                salary = value;
                NotifyOfPropertyChange(() => Salary);
                NotifyOfPropertyChange(() => CanSaveEmployee);
            }
        }

        string deptName;
        public string DeptName
        {
            get { return deptName; }
            set
            {
                deptName = value;
                NotifyOfPropertyChange(() => DeptName);
                NotifyOfPropertyChange(() => CanSaveEmployee);
            }
        }

        string destination;
        public string Destination
        {
            get { return destination; }
            set
            {
                destination = value;
                NotifyOfPropertyChange(() => Destination);
                NotifyOfPropertyChange(() => CanSaveEmployee);
            }
        }

        public EmployeesModel selectedEmployee;
        public EmployeesModel SelectedEmployee
        {
            get { return selectedEmployee; }
            set
            {
                Id = value.Id;
                EmpName = value.EmpName;
                Salary = value.Salary;
                DeptName = value.DeptName;
                Destination = value.Destination;
                NotifyOfPropertyChange(() => SelectedEmployee);
            }
        }

        public void NewEmployee()
        {
            Id = 0;
            EmpName = string.Empty;
            Salary = 0;
            DeptName = Destination = string.Empty;
        }
        
        //버튼 활성/비활성화 속성
        public bool CanSaveEmployee
        {
            get
            {
                return !string.IsNullOrEmpty(EmpName) && !string.IsNullOrEmpty(DeptName)
                    && !string.IsNullOrEmpty(Destination) && Salary != 0; 
            }
        }

        public void SaveEmployee()
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                if(Id == 0)     //INSERT
                {
                    cmd.CommandText = @"INSERT INTO TblEmployees
                                               (EmpName
                                               , Salary
                                               , DeptName
                                               , Destination)
                                         VALUES
                                               (@EmpName
                                               , @Salary
                                               , @DeptName
                                               , @Destination)";
                }
                else            //UPDATE
                {
                    cmd.CommandText = @"UPDATE TblEmployees
                                           SET EmpName = @EmpName
                                              , Salary = @Salary
                                              , DeptName = @DeptName
                                              , Destination = @Destination
                                         WHERE Id = @Id";
                }
                SqlParameter parmEmpName = new SqlParameter("@EmpName", EmpName);
                SqlParameter parmSalary = new SqlParameter("@Salary", Salary);
                SqlParameter parmDeptName = new SqlParameter("@DeptName", DeptName);
                SqlParameter parmDestination = new SqlParameter("@Destination", Destination);

                cmd.Parameters.Add(parmEmpName);
                cmd.Parameters.Add(parmSalary);
                cmd.Parameters.Add(parmDeptName);
                cmd.Parameters.Add(parmDestination);

                if(Id != 0)
                {
                    SqlParameter parmId = new SqlParameter("@Id", Id);
                    cmd.Parameters.Add(parmId);
                }

                cmd.ExecuteNonQuery();
            }

            //입력창 전부 초기화
            NewEmployee();
            //데이터 다시 조회
            GetEmployees();
        }

        public bool CanDelEmployee
        {
            get 
            {
                return (Id != 0);
            }
        }
        public void DelEmployee()
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                string strQuery = "delete from TblEmployees where Id = @Id";
                SqlCommand cmd = new SqlCommand(strQuery, conn);
                SqlParameter parmId = new SqlParameter("@Id", Id);
                cmd.Parameters.Add(parmId);
                cmd.ExecuteNonQuery();

                //입력창 전부 초기화
                NewEmployee();
                //데이터 다시 조회
                GetEmployees();
            }
        }
    }
}

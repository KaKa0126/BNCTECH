using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace TESTCSS
{
    public partial class Form2 : Form
    {
        // Form2의 생성자를 수정하여 Form1의 인스턴스를 받을 수 있도록 합니다.
        private Form1 form1Instance;
        public Form2(Form1 form1)
        {
            InitializeComponent();
            form1Instance = form1;
        }

        public string product_name { get; private set; }
        public int robot_point { get; private set; }
        public int seq { get; private set; }
        public string middle_point { get; private set; }
        public string partial_inspection_mp_necessary { get; private set; }

        private void Save_Click(object sender, EventArgs e)
        {
            // ComboBox의 Text 값이 비어 있는지만 검사
            if (string.IsNullOrEmpty(comboBox1.Text))
            {
                MessageBox.Show("'product_name' 값을 입력해주세요.");
                return; // 메서드 종료
            }
            else if (string.IsNullOrEmpty(comboBox2.Text))
            {
                MessageBox.Show("'middle_point' 값을 입력해주세요.");
                return; // 메서드 종료
            }
            else if (string.IsNullOrEmpty(comboBox3.Text))
            {
                MessageBox.Show("'partial_inspection_mp_necessary' 값을 입력해주세요.");
                return; // 메서드 종료
            }

            // ComboBox에서 값을 가져와 속성에 저장
            product_name = comboBox1.Text;
            middle_point = comboBox2.Text;
            partial_inspection_mp_necessary = comboBox3.Text;

            // TextBox에서 입력받은 값을 int로 변환
            bool isSuccess1 = int.TryParse(textBox1.Text, out int result1);
            if (isSuccess1 && result1 >= 1)
            {
                robot_point = result1;
            }
            else
            {
                // 변환 실패 처리, 예: 사용자에게 경고 메시지 표시
                MessageBox.Show("'robot_point' 값이 올바르지 않습니다. 자연수 값을 입력해주세요.");
                return; // 메서드 종료
            }

            // TextBox에서 입력받은 값을 int로 변환
            bool isSuccess2 = int.TryParse(textBox2.Text, out int result2);
            if (isSuccess2 && result2 >= 1)
            {
                seq = result2;
            }
            else
            {
                // 변환 실패 처리, 예: 사용자에게 경고 메시지 표시
                MessageBox.Show("'seq' 값이 올바르지 않습니다. 자연수 값을 입력해주세요.");
                return; // 메서드 종료
            }

            SaveDataToTable(product_name, robot_point, seq, middle_point, partial_inspection_mp_necessary);

            form1Instance.LoadDataFromDatabase();
            this.Close();
        }

        private void SaveDataToTable(string productName, int robotPoint, int seq, string middlePoint, string partialInspectionMpNecessary)
        {
            string connString = form1Instance.connString;
            // 이제 connString 변수를 사용하여 데이터베이스 작업을 수행할 수 있습니다.

            try
            {
                // NpgsqlConnection 인스턴스 생성 및 데이터베이스 연결
                using (NpgsqlConnection conn = new NpgsqlConnection(connString))
                {
                    conn.Open();

                    // 먼저 robot_point 값과 product_name 값이 동시에 중복되는지 확인
                    string checkDuplicateSql = "SELECT COUNT(*) FROM public.list WHERE robot_point = @robotPoint AND product_name = @productName";

                    using (NpgsqlCommand checkCmd = new NpgsqlCommand(checkDuplicateSql, conn))
                    {
                        // 매개변수에 robot_point 값과 product_name 값 할당
                        checkCmd.Parameters.AddWithValue("@robotPoint", robotPoint);
                        checkCmd.Parameters.AddWithValue("@productName", productName);

                        int count = Convert.ToInt32(checkCmd.ExecuteScalar());

                        if (count > 0)
                        {
                            // robot_point 값과 product_name 값이 동시에 중복되면 사용자에게 알리고 메서드 종료
                            MessageBox.Show("오류: 'robot_point'와 'product_name' 값이 중복됩니다.");
                            return; // 메서드 종료
                        }
                    }

                    // seq 값 변경
                    try
                    {
                        // @seq 값과 정확히 일치하며, 동시에 product_name도 일치하는 데이터가 있는지 확인
                        string checkSql = @"
                            SELECT EXISTS (SELECT 1 FROM public.list
                            WHERE seq = @seq AND product_name = @productName)
                        ";

                        using (NpgsqlCommand checkCmd = new NpgsqlCommand(checkSql, conn))
                        {
                            checkCmd.Parameters.AddWithValue("@seq", seq);
                            checkCmd.Parameters.AddWithValue("@productName", productName);
                            bool exists = (bool)checkCmd.ExecuteScalar();

                            // 조건을 만족하는 데이터가 존재하면, product_name이 동일하고, seq 값이 @seq 값보다 같거나 큰 모든 데이터의 seq 값을 +1 증가
                            if (exists)
                            {
                                string updateSql = @"
                                    UPDATE public.list
                                    SET seq = seq + 1
                                    WHERE product_name = @productName AND seq >= @seq";
                                using (NpgsqlCommand updateCmd = new NpgsqlCommand(updateSql, conn))
                                {
                                    updateCmd.Parameters.AddWithValue("@productName", productName);
                                    updateCmd.Parameters.AddWithValue("@seq", seq);

                                    // 쿼리 실행
                                    int rowsAffected = updateCmd.ExecuteNonQuery();

                                    // 업데이트된 행의 수를 출력 (주석 해제)
                                    //MessageBox.Show($"{rowsAffected} 개의 행이 업데이트 되었습니다.");
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("오류가 발생했습니다: " + ex.Message);
                    }

                    //SaveDatatoTable
                    string sql = "INSERT INTO public.list (product_name, robot_point, seq, middle_point, partial_inspection_mp_necessary) VALUES (@productName, @robotPoint, @seq, @middlePoint, @partialInspectionMpNecessary)";

                    using (NpgsqlCommand cmd = new NpgsqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@productName", productName);
                        cmd.Parameters.AddWithValue("@robotPoint", robotPoint);
                        cmd.Parameters.AddWithValue("@seq", seq);
                        cmd.Parameters.AddWithValue("@middlePoint", middlePoint);
                        cmd.Parameters.AddWithValue("@partialInspectionMpNecessary", partialInspectionMpNecessary);

                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("데이터가 성공적으로 저장되었습니다.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("데이터 저장 중 오류가 발생했습니다: " + ex.Message);
            }
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

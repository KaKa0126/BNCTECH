using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace TESTCSS
{
    public partial class Form3 : Form
    {
        // Form3의 생성자를 수정하여 Form1의 인스턴스를 받을 수 있도록 합니다.
        private Form1 form1Instance;
        public Form3(Form1 form1)
        {
            InitializeComponent();
            form1Instance = form1;
        }

        public string product_name { get; private set; }
        public int robot_point { get; private set; }
        public int seq { get; private set; }
        public string middle_point { get; private set; }
        public string partial_inspection_mp_necessary { get; private set; }

        public Form3()
        {
            InitializeComponent();
        }

        private void Delete_Click(object sender, EventArgs e)
        {
            // ComboBox의 Text 값이 비어 있는지만 검사
            if (string.IsNullOrEmpty(comboBox1.Text))
            {
                MessageBox.Show("'product_name' 값을 입력해주세요.");
                return; // 메서드 종료
            }

            // ComboBox에서 값을 가져와 속성에 저장
            product_name = comboBox1.Text;

            // TextBox에서 입력받은 값을 int로 변환
            bool isSuccess3 = int.TryParse(textBox1.Text, out int result3);
            if (isSuccess3 && result3 >= 1)
            {
                robot_point = result3;
            }
            else
            {
                // 변환 실패 처리, 예: 사용자에게 경고 메시지 표시
                MessageBox.Show("'robot_point' 값이 올바르지 않습니다. 자연수 값을 입력해주세요.");
                return; // 메서드 종료
            }

            string connString = form1Instance.connString;
            // 이제 connString 변수를 사용하여 데이터베이스 작업을 수행할 수 있습니다.

            //produnct_name 값과 robot_point 값을 동시에 만족하는 데이터를 데이터베이스에서 삭제하는 코드
            try
            {
                using (NpgsqlConnection conn = new NpgsqlConnection(connString))
                {
                    conn.Open();

                    // seq 값 변경
                    // robot_point 값에 해당하는 데이터 행의 seq 값보다 큰 seq 값을 가진 모든 행에 대해 seq 값을 -1 하는 쿼리
                    string updateSql = @"
                        UPDATE public.list
                        SET seq = seq - 1
                        WHERE product_name IN
                        (SELECT product_name FROM public.list WHERE robot_point = @robotPoint) AND
                        seq > (SELECT seq FROM public.list WHERE robot_point = @robotPoint);
                    ";

                    using (NpgsqlCommand cmd = new NpgsqlCommand(updateSql, conn))
                    {
                        // 매개변수에 robot_point 값 할당
                        cmd.Parameters.AddWithValue("@robotPoint", robot_point);

                        // 쿼리 실행
                        int rowsAffected = cmd.ExecuteNonQuery();

                        // 업데이트된 행의 수를 사용자에게 알림
                        //MessageBox.Show($"{rowsAffected} 개의 데이터가 업데이트되었습니다.");
                    }

                    // DELETE 쿼리 준비
                    // 여기서는 product_name과 robot_point가 일치하는 데이터를 삭제합니다.
                    string sql = "DELETE FROM public.list WHERE product_name = @productName AND robot_point = @robotPoint";

                    using (NpgsqlCommand cmd = new NpgsqlCommand(sql, conn))
                    {
                        // 매개변수에 값 할당
                        cmd.Parameters.AddWithValue("@productName", product_name);
                        cmd.Parameters.AddWithValue("@robotPoint", robot_point);

                        // 쿼리 실행
                        int rowsAffected = cmd.ExecuteNonQuery();

                        // 삭제된 행의 수를 사용자에게 알림
                        MessageBox.Show($"{rowsAffected} 개의 데이터가 삭제되었습니다.");
                    }

                    form1Instance.LoadDataFromDatabase();
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                // 오류 처리
                MessageBox.Show($"데이터 삭제 중 오류가 발생했습니다: {ex.Message}");
            }
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

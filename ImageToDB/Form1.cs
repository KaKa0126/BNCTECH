using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AForge.Video;
using AForge.Video.DirectShow;
using Npgsql;
using NpgsqlTypes;

namespace TEST2
{
    public partial class Form1 : Form
    {
        private string lastFileName = null; // 최근에 확인한 파일명 저장 변수

        public Form1()
        {
            InitializeComponent();
        }

        VideoCaptureDevice videoCapture;
        FilterInfoCollection filterInfo;

        void StartCamera()
        {
            try
            {
                filterInfo = new FilterInfoCollection(FilterCategory.VideoInputDevice);
                videoCapture = new VideoCaptureDevice(filterInfo[0].MonikerString);
                videoCapture.NewFrame += new NewFrameEventHandler(Camera_On);
                videoCapture.Start();

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private void Camera_On(object sender, NewFrameEventArgs eventArgs)
        {
            Pic1.Image = (Bitmap)eventArgs.Frame.Clone();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                videoCapture.Stop();
            }
            catch
            {
                return;
            }
        }

        private void BtnStart_Click(object sender, EventArgs e)
        {
            StartCamera();
        }

        private void BtnCapture_Click(object sender, EventArgs e)
        {
            // Copy image from Pic1 to Pic2
            Pic2.Image = (Image)Pic1.Image.Clone();

            string saveName = DateTime.Now.ToString("yyMMddhhmmss_1");
            string year = DateTime.Now.ToString("yyyy");
            string month = DateTime.Now.ToString("MM");
            string day = DateTime.Now.ToString("dd");
            string folderPath1 = @"C:\Users\PJK_BNC-TECH\Desktop\folder1";
            string folderPath2 = @"D:\Robot\ai_output\" + year + @"\" + month + @"\" + day + @"\89500-DO221DFS";
            string folderPath3 = @"D:\Robot\ai_result\" + year + @"\" + month + @"\" + day + @"\89500-DO221DFS";

            // Ensure the folders exist before trying to save
            Directory.CreateDirectory(folderPath1);
            Directory.CreateDirectory(folderPath2);

            string fileName1 = Path.Combine(folderPath1, saveName + ".jpg");
            string fileName2 = Path.Combine(folderPath2, saveName + ".jpg");

            try
            {
                var bitmap = new Bitmap(Pic2.Width, Pic2.Height);
                Pic2.DrawToBitmap(bitmap, Pic2.ClientRectangle);

                // Save image to the first location
                bitmap.Save(fileName1, System.Drawing.Imaging.ImageFormat.Jpeg);

                // Copy the saved image to the second location
                File.Copy(fileName1, fileName2, true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error");
            }

            // Start monitoring the second folder
            StartFolderMonitoring(folderPath3);
            /*
            finally
            {
                // Dispose of the Bitmap object
                bitmap.Dispose();
            }
            */
        }

        private void StartFolderMonitoring(string folderPath)
        {
            // Create a new FileSystemWatcher
            FileSystemWatcher watcher = new FileSystemWatcher();
            watcher.Path = folderPath;

            // Subscribe to the Created event
            watcher.Created += (sender, e) =>
            {
                // Wait for a short time to ensure that the file creation is complete
                System.Threading.Thread.Sleep(100);

                // Check if the file still exists (it may have been deleted)
                if (File.Exists(e.FullPath))
                {
                    // Handle the newly created file
                    string newFileName = Path.GetFileName(e.FullPath);
                    string fileNamePart = newFileName.Substring(15, 5); // 일부 파일명 추출

                    // Update the lastFileName variable
                    lastFileName = newFileName;

                    // Use Invoke to update UI controls
                    Invoke((MethodInvoker)delegate
                    {
                        // Display fileNamePart in TextBox
                        TXT_Result.Text = fileNamePart;

                        // Load the image from the file path
                        Image originalImage = Image.FromFile(e.FullPath);

                        // Calculate the aspect ratio of the original image
                        float aspectRatio = (float)originalImage.Width / (float)originalImage.Height;

                        // Create a new Bitmap with the desired size (e.g., the size of the PictureBox)
                        int targetWidth = Pic4.Width;
                        int targetHeight = (int)(targetWidth / aspectRatio);
                        Bitmap resizedImage = new Bitmap(originalImage, new Size(targetWidth, targetHeight));

                        // Set the resized image to the PictureBox
                        Pic4.Image = resizedImage;
                    });

                    // Perform any additional actions with the new file
                    // Console.WriteLine($"New File Detected: {newFileName}, Partial Name: {fileNamePart}");
                }
            };

            // Enable the FileSystemWatcher
            watcher.EnableRaisingEvents = true;
        }

        private void BtnDBinsert_Click(object sender, EventArgs e)
        {
            string connString = "Host=localhost;Port=5432;Username=test;Password=test;Database=TEST2";

            string year = DateTime.Now.ToString("yyyy");
            string month = DateTime.Now.ToString("MM");
            string day = DateTime.Now.ToString("dd");
            string filePath = @"D:\Robot\ai_result\" + year + @"\" + month + @"\" + day + @"\89500-DO221DFS";

            //finding recentFileName 
            DirectoryInfo DI = new DirectoryInfo(filePath);

            // 디렉토리에서 파일 목록을 가져오고 날짜/시간 기준으로 정렬
            FileInfo[] files = DI.GetFiles().OrderByDescending(f => f.CreationTime).ToArray();

            // 'recentFileName' 변수에 가장 최근에 생성된 파일명 설정
            string recentFileName = files.Length > 0 ? files[0].Name : null;

            //finding result
            string fullPath = filePath + @"\" + recentFileName + ".jpg";
            string fileNameWithExtension = Path.GetFileName(fullPath);
            string resultPart = fileNameWithExtension.Substring(15, 5);  //15번째 부터, 5글자 추출

            // Declare MemoryStream, Image, and byte array
            using (MemoryStream ms = new MemoryStream())
            using (Image img = Pic2.Image)
            {
                try
                {
                    img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                    byte[] bytes = ms.ToArray();

                    // PostgreSQL db open
                    using (var conn = new NpgsqlConnection(connString))
                    {
                        conn.Open();

                        // DB INSERT statement
                        using (var command = new NpgsqlCommand())
                        {
                            command.Connection = conn;
                            command.CommandText = "INSERT INTO data VALUES(@created, @title, @result, @photo)";

                            // Adjust NpgsqlDbType according to the actual data types in your database
                            command.Parameters.AddWithValue("created", NpgsqlDbType.Timestamp, DateTime.Now);
                            command.Parameters.AddWithValue("title", NpgsqlDbType.Text, recentFileName);
                            command.Parameters.AddWithValue("result", NpgsqlDbType.Text, resultPart);
                            command.Parameters.AddWithValue("photo", NpgsqlDbType.Bytea, bytes);

                            command.ExecuteNonQuery();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
            // No need to explicitly close the connection; 'using' statement handles it
        }

        //디자인탭에서 선택한 날짜의 'title' 열의 결과값 조회
        private void BtnSerachingDB_Click(object sender, EventArgs e)
        {
            string connString = "Host=localhost;Port=5432;Username=test;Password=test;Database=TEST2";

            // 선택된 날짜 가져오기
            DateTime targetDate = dateTimePicker.Value.Date;

            using (var conn = new NpgsqlConnection(connString))
            {
                try
                {
                    conn.Open();

                    // 특정 날짜의 'title' 조회 쿼리
                    string selectQuery = "SELECT title FROM data WHERE created::date = @targetDate::date";

                    using (var selectCommand = new NpgsqlCommand(selectQuery, conn))
                    {
                        // 파라미터 추가
                        selectCommand.Parameters.AddWithValue("targetDate", targetDate);

                        // 'title' 값을 저장할 목록
                        List<string> titleList = new List<string>();

                        using (var reader = selectCommand.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                // 'title' 값을 읽어와 목록에 추가
                                string title = reader.GetString(reader.GetOrdinal("title"));
                                titleList.Add(title);
                            }
                        }

                        // 'titleList'를 사용하여 ComboBox에 항목 추가
                        comboBoxTitles.Items.Clear();
                        comboBoxTitles.Items.AddRange(titleList.ToArray());

                        // 선택한 결과를 처리하는 이벤트 핸들러 등록
                        comboBoxTitles.SelectedIndexChanged += ComboBoxTitles_SelectedIndexChanged;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
        }

        //이미지를 디자인 탭에서 표시
        private void ComboBoxTitles_SelectedIndexChanged(object sender, EventArgs e)
        {
            // ComboBox에서 선택된 'title' 값 처리
            string selectedTitle = comboBoxTitles.SelectedItem.ToString();
            //Console.WriteLine($"선택한 'title': {selectedTitle}");

            // 선택된 'title'에 해당하는 'photo' 열의 바이너리 데이터 가져오기
            byte[] photoBytes = GetPhotoData(selectedTitle);

            if (photoBytes != null && photoBytes.Length > 0)
            {
                // 바이너리 데이터를 이미지로 변환
                Image image = ByteArrayToImage(photoBytes);

                // 이미지를 PictureBox에 설정하여 디자인 탭에서 표시
                Pic3.Image = image;
            }
            else
            {
                Console.WriteLine("선택한 'title'에 대한 이미지가 없습니다.");
            }
        }

        //이미지 바이너리 데이터 가져오기
        private byte[] GetPhotoData(string title)
        {
            string connString = "Host=localhost;Port=5432;Username=test;Password=test;Database=TEST2";

            using (var conn = new NpgsqlConnection(connString))
            {
                try
                {
                    conn.Open();

                    // 'title'에 해당하는 'photo' 열의 바이너리 데이터 조회 쿼리
                    string selectQuery = "SELECT photo FROM data WHERE title = @title";

                    using (var selectCommand = new NpgsqlCommand(selectQuery, conn))
                    {
                        // 파라미터 추가
                        selectCommand.Parameters.AddWithValue("title", title);

                        // 바이너리 데이터 읽기
                        object result = selectCommand.ExecuteScalar();
                        if (result != null)
                        {
                            return (byte[])result;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }

            return null;
        }

        //바이너리 데이터를 이미지로 변환
        private Image ByteArrayToImage(byte[] byteArray)
        {
            using (MemoryStream ms = new MemoryStream(byteArray))
            {
                return Image.FromStream(ms);
            }
        }
    }
}

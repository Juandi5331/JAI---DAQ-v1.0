using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using iTextSharp.text;
using iTextSharp.text.pdf;
using ListViewItemForms = System.Windows.Forms.ListViewItem; // Alias para System.Windows.Forms.ListViewItem
using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.Advertisement;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using Windows.Devices.Enumeration;
using Windows.Storage.Streams;
using System.Windows.Forms.DataVisualization.Charting;
using WMPLib;
using System.Media;

namespace JAI___DAQ_v1._0
{
    public partial class Form3 : Form
    {
        private BluetoothLEAdvertisementWatcher watcher = null;
        private DeviceWatcher deviceWatcher = null;

        public BluetoothLEDevice bluetoothLEDevice = null;

        public GattDeviceServicesResult results { get; set; }

        public DeviceInformation device = null;

        public GattCharacteristic selectedCarachteristics = null;
        public GattDeviceService selectedService = null;

        public string JAIservice_id = "478a9671f67b4194b7b05a65cd5f98b0";

        public int isMon = 0;
        public int countan = 0;
        public bool sta = false;

        public string data;
        public int length = 0;
        public string data2;
        public int length2 = 0;
        public string data3;
        public int length3 = 0;
        public string data4;
        public int length4 = 0;
        public string data12;
        public int length12 = 0;
        public int tsava = 0;
        public int countshark = 3;
        public int score = 0;
        public int xf = 760;
        public int yf = 210;
        public int timegame = 45;
        public double[] rsp;
        public double[] psp;
        public double[] tsim;


        public double[] xsim;
        public double[] ysim;
        public double[] xv;
        public double[] yv;
        public double[] zv;
        public double[] xa;
        public double[] ya;
        public double[] za;
         


        public Form3()
        {
            InitializeComponent();
            rsp = new double[461];
            psp = new double[461];
            tsim = new double[461];
            xsim = new double[461];
            ysim = new double[461];
            xv = new double[461];
            yv = new double[461];
            zv = new double[461];
            xa = new double[461];
            ya = new double[461];
            za = new double[461];



            button1.Visible = false;
            button2.Visible = false;
            button3.Visible = false;
            button4.Visible = false;
            button5.Visible = false;
            button6.Visible = false;
            button7.Visible = false;
            button8.Visible = false;
            label14.Visible = false;
            label9.Visible = false;
            label10.Visible = false;
            label11.Visible = false;
            label6.Visible = false;
            label13.Visible = false;
            label12.Visible = false;
            listView1.Visible = false;
            Titulo.Visible = false;
            chart1.Visible = false;
            pictureBox3.Visible = false;
            pictureBox4.Visible = false;
            pictureBox5.Visible = false;
            pictureBox6.Visible = false;    
            pictureBox7.Visible = false;
        }


        private void timer1_Tick(object sender, EventArgs e)
        {
            if (length < data.Length)
            {
                label1.Text = label1.Text + data.ElementAt(length);
                length++;
            }
            else
            {
                timer1.Stop();
                button1.Visible = true;
                VoiceShark.StopS();
            }

        }

        private void Form3_Load(object sender, EventArgs e)
        {
            
            data = label1.Text;
            data2 = label2.Text;
            data3 = label3.Text;
            data4 = label8.Text;
            data12 = label14.Text;
            label1.Text = "";
            label2.Text = "";
            label3.Text = "";
            label4.Text = "";
            label8.Text = "";
            label14.Text = "";

            timer1.Start();

            VoiceShark.aname("C:\\Users\\spjua\\Documents\\Backup\\JAI - DAQ v1.0\\JAI - DAQ v1.0\\bin\\Debug\\sharkuno.wav");
            VoiceShark.StartS();

            label2.Visible = false;
            label3.Visible = false;


            ///////////////////BLE Connection/////////////////////////////////

            CheckForIllegalCrossThreadCalls = false;

        }

        #region Ble connection

        private void BLE_StartScanner()
        {
            listView1.Items.Clear();
            watcher = new BluetoothLEAdvertisementWatcher();
            watcher.ScanningMode = BluetoothLEScanningMode.Active;
            watcher.Received += OnAdvertisementReceived;

            watcher.SignalStrengthFilter.OutOfRangeTimeout = TimeSpan.FromMilliseconds(1000);
            watcher.SignalStrengthFilter.SamplingInterval = TimeSpan.FromMilliseconds(500);

            watcher.Start();

        }

        string name_buffer = "";
        string name_s = "";
        string Device_id = "";

        private void OnAdvertisementReceived(BluetoothLEAdvertisementWatcher sender, BluetoothLEAdvertisementReceivedEventArgs args)
        {
            name_buffer = args.Advertisement.LocalName;

            if (name_buffer != string.Empty)
            {
                name_s = name_buffer;
                Device_id = args.BluetoothAddress.ToString();

                string[] bilgiler = { name_s, Device_id };
                ListViewItemForms lst = new ListViewItemForms(bilgiler); // Usa alias para evitar ambigüedad

                if (name_s == "motionMonitor" && isMon == 0)
                {

                    listView1.Items.Add(lst);
                    isMon++;
                }

            }


        }

        string name = "";

        private async void BLE_Connect()
        {
            label4.Text = "Connecting ...";

            if (listView1.SelectedItems.Count > 0)
            {
                name = listView1.SelectedItems[0].SubItems[0].Text;
                string[] requestedProperties = { "System.Devices.Aep.DeviceAddress", "System.Devices.Aep.IsConnected" };

                deviceWatcher = DeviceInformation.CreateWatcher(
                    BluetoothLEDevice.GetDeviceSelectorFromPairingState(false),
                    requestedProperties,
                    DeviceInformationKind.AssociationEndpoint);

                deviceWatcher.Added += DeviceWatcher_Added;
                deviceWatcher.Updated += DeviceWatcher_Updated;
                deviceWatcher.Removed += DeviceWatcher_Removed;

                deviceWatcher.EnumerationCompleted += DeviceWatcher_EnumerationCompleted;
                deviceWatcher.Stopped += DeviceWatcher_Stopped;

                deviceWatcher.Start();

                while (true)
                {
                    if (device == null)
                    {
                        Thread.Sleep(200);

                    }
                    else
                    {
                        bluetoothLEDevice = await BluetoothLEDevice.FromIdAsync(device.Id);
                        results = await bluetoothLEDevice.GetGattServicesAsync();

                        if (results.Status == GattCommunicationStatus.Success)
                        {
                            var services = results.Services;


                            foreach (var service in services)

                            {
                                if (service.Uuid.ToString("N") == JAIservice_id)
                                {

                                    GattCharacteristicsResult characteristicsResult = await service.GetCharacteristicsAsync();

                                    if (characteristicsResult.Status == GattCommunicationStatus.Success)
                                    {
                                        var characteristics = characteristicsResult.Characteristics;

                                        foreach (var characteristic in characteristics)
                                        {
                                            GattCharacteristicProperties properties = characteristic.CharacteristicProperties;
                                            if (properties.HasFlag(GattCharacteristicProperties.Read) && characteristic.Uuid.ToString().Equals("430a5b62-c01a-4db5-8347-0565c672c459"))
                                            {
                                                GattCommunicationStatus status = await characteristic.WriteClientCharacteristicConfigurationDescriptorAsync(GattClientCharacteristicConfigurationDescriptorValue.Notify);

                                                if (status == GattCommunicationStatus.Success)
                                                {
                                                    label4.Text = "Connected";
                                                    characteristic.ValueChanged += Characteristic_ValueChanged;
                                                    selectedCarachteristics = characteristic;
                                                    this.WindowState = FormWindowState.Maximized;
                                                    button3.Visible = false;
                                                    button4.Visible = false;
                                                    button5.Visible = false;
                                                    button6.Visible = false;                                                
                                                    listView1.Visible = false;
                                                    label4.Visible = false;
                                                    Titulo.Visible = false;
                                                    label3.Visible = false;
                                                    chart1.Visible = true;
                                                    timer4.Start();
                                                    VoiceShark.aname("C:\\Users\\spjua\\Documents\\Backup\\JAI - DAQ v1.0\\JAI - DAQ v1.0\\bin\\Debug\\sharkcuatro.wav");
                                                    VoiceShark.StartS();

                                                    pictureBox4.Visible = true;

                                                }
                                            }
                                        }

                                    }

                                }
                            }
                        }

                    }
                }


            }

        }

        private void Characteristic_ValueChanged(GattCharacteristic sender, GattValueChangedEventArgs args)
        {
            DataReader reader = DataReader.FromBuffer(args.CharacteristicValue);
            byte[] byteArray = new byte[reader.UnconsumedBufferLength];

            reader.ReadBytes(byteArray);

           
            int xsl;
            int ysl;

            
            Random random = new Random();


            var str = byteArray;

            float[] donSava = ConvertByteArrayToFloatArray(byteArray);

           

            if (sta)
            {
                chart1.Series["Roll"].Points.AddXY(tsava, donSava[0]);
               
                chart1.Series["Pitch"].Points.AddXY(tsava, donSava[2]);


                
                tsim[tsava] = tsava;
                rsp[tsava] = (780/7)-(xf/7);
                psp[tsava] = (-170 / 3) + (yf / 3);



                double width = this.Width;
                double height = this.Height;
                                             
                
                pictureBox7.Location = new Point(xf, yf);
                pictureBox7.BringToFront();

                xsl = -7*(int)Math.Round(donSava[0])+780;
                ysl = 3*(int)Math.Round(donSava[2])+170;

                xsim[tsava] = xsl;
                ysim[tsava] = ysl;

                xv[tsava] = donSava[3];
                yv[tsava] = donSava[4];
                zv[tsava] = donSava[5];

                xa[tsava] =  donSava[6];
                ya[tsava] =  donSava[7];
                za[tsava] =  donSava[8];



                tsava++;

                if (xsl < 440)
                {
                    xsl = 440;
                }
                if (xsl > 1100)
                {
                    xsl = 1100;
                }
                if (ysl < 20)
                {
                    ysl = 20;
                }
                if (ysl > 340)
                {
                    ysl = 340;
                }

                pictureBox6.Location = new Point(xsl, ysl);

                if (Math.Abs(xf-xsl) < 50 && Math.Abs(yf - ysl) < 50)
                {
                    xf = random.Next(420,1100);
                    yf = random.Next(20,340);
                    pictureBox7.BringToFront();
                    score = score + 10;
                    label11.Text = score.ToString();
                                      
                }
                else { }
                

            }

        }

        public static float[] ConvertByteArrayToFloatArray(byte[] byteArray)
        {
            if (byteArray.Length != 36)
            {
                throw new ArgumentException("El byte array debe tener exactamente 12 bytes.");
            }

            float[] floatArray = new float[9];
            for (int i = 0; i < 9; i++)
            {
                floatArray[i] = BitConverter.ToSingle(byteArray, i * 4);
            }

            return floatArray;
        }

        private void DeviceWatcher_Stopped(DeviceWatcher sender, object args)
        {

        }

        private void DeviceWatcher_EnumerationCompleted(DeviceWatcher sender, object args)
        {

        }

        private void DeviceWatcher_Removed(DeviceWatcher sender, DeviceInformationUpdate args)
        {

        }

        private void DeviceWatcher_Updated(DeviceWatcher sender, DeviceInformationUpdate args)
        {

        }

        private void DeviceWatcher_Added(DeviceWatcher sender, DeviceInformation args)
        {
            if (args.Name == name)
            {
                device = args;
            }
        }

        #endregion


        private void button1_Click(object sender, EventArgs e)
        {
            label1.Visible = false;
            label2.Visible = true;
            button1.Visible = false;
            timer2.Start();
            VoiceShark.aname("C:\\Users\\spjua\\Documents\\Backup\\JAI - DAQ v1.0\\JAI - DAQ v1.0\\bin\\Debug\\sharkdos.wav");
            VoiceShark.StartS();
            pictureBox3.Visible = true;
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            if (length2 < data2.Length)
            {
                label2.Text = label2.Text + data2.ElementAt(length2);
                length2++;
            }
            else
            {
                timer2.Stop();
                button2.Visible = true;
                VoiceShark.StopS();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            pictureBox3.Visible = false;
            timer3.Start();
            label2.Visible = false;
            label3.Visible = true;
            button2.Visible = false;
            listView1.Visible = true;
            VoiceShark.aname("C:\\Users\\spjua\\Documents\\Backup\\JAI - DAQ v1.0\\JAI - DAQ v1.0\\bin\\Debug\\sharktres.wav");
            VoiceShark.StartS();

        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            if (length3 < data3.Length)
            {
                label3.Text = label3.Text + data3.ElementAt(length3);
                length3++;
                if (length3 > 80)
                {
                    button3.Visible = true;
                    button4.Visible = true;
                    Titulo.Visible = true;
                }
            }
            else
            {
                timer3.Stop();
                VoiceShark.StopS();
                button5.Visible = true;
                button6.Visible = true;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            BLE_StartScanner();
            label4.Text = "Scanning";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            watcher.Stop();
            watcher.ScanningMode = BluetoothLEScanningMode.Passive;
            watcher.Received += OnAdvertisementReceived;
            label4.Text = "Scanning Stopped";
        }

        private void button5_Click(object sender, EventArgs e)
        {
            BLE_Connect();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            selectedCarachteristics.ValueChanged -= Characteristic_ValueChanged;
            bluetoothLEDevice.Dispose();
            bluetoothLEDevice = null;
            label4.Text = "Disconnected";
        }

        private void timer4_Tick(object sender, EventArgs e)
        {
            if (length4 < data4.Length)
            {
                label8.Text = label8.Text + data4.ElementAt(length4);
                length4++;
            }
            else
            {
                timer4.Stop();
                VoiceShark.StopS();
                timer5.Start();
                pictureBox1.BringToFront();
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {

            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string pdfPath = Path.Combine(desktopPath, "Patient_report.pdf");

            // Crear documento PDF
            Document doc = new Document();
            PdfWriter pdfWriter = PdfWriter.GetInstance(doc, new FileStream(pdfPath, FileMode.Create));
            doc.Open();

            // Set font style for the whole document
            iTextSharp.text.Font tittleFont = FontFactory.GetFont("Arial", 24, 1);
            iTextSharp.text.Font normalFont = FontFactory.GetFont("Arial", 12, BaseColor.BLACK);
            iTextSharp.text.Font boldFont = FontFactory.GetFont("Arial", 12, 1);
            

            Paragraph title = new Paragraph("Patient's Report", tittleFont);
            title.Alignment = Element.ALIGN_CENTER; // Centra el título en el documento
            doc.Add(title);

            string[] dataPathl = File.ReadAllLines("dataPath.txt");

            Paragraph paragraph1 = new Paragraph();
            paragraph1.Add(new Chunk("Name: ", boldFont));
            paragraph1.Add(new Chunk(dataPathl[0], normalFont));
            paragraph1.KeepTogether = true;
            doc.Add(paragraph1);
            doc.Add(new Paragraph(" "));

            Paragraph paragraph2 = new Paragraph();
            paragraph2.Add(new Chunk("Surname: ", boldFont));
            paragraph2.Add(new Chunk(dataPathl[1], normalFont));
            paragraph2.KeepTogether = true;
            doc.Add(paragraph2);
            doc.Add(new Paragraph(" "));

            Paragraph paragraph3 = new Paragraph();
            paragraph3.Add(new Chunk("Age: ", boldFont));
            paragraph3.Add(new Chunk(dataPathl[2]+" years old", normalFont));
            paragraph3.KeepTogether = true;
            doc.Add(paragraph3);
            doc.Add(new Paragraph(" "));

            Paragraph paragraph4 = new Paragraph();
            paragraph4.Add(new Chunk("Weight: ", boldFont));
            paragraph4.Add(new Chunk(dataPathl[3] + " kg", normalFont));
            paragraph4.KeepTogether = true;
            doc.Add(paragraph4);
            doc.Add(new Paragraph(" "));

            Paragraph paragraph5 = new Paragraph();
            paragraph5.Add(new Chunk("Height: ", boldFont));
            paragraph5.Add(new Chunk(dataPathl[4] + " cm", normalFont));
            paragraph5.KeepTogether = true;
            doc.Add(paragraph5);
            doc.Add(new Paragraph(" "));

            Paragraph paragraph6 = new Paragraph();
            paragraph6.Add(new Chunk("City/Address: ", boldFont));
            paragraph6.Add(new Chunk(dataPathl[5], normalFont));
            paragraph6.Add(new Chunk("                                                         Score: ", boldFont));
            paragraph6.Add(new Chunk(score.ToString(), normalFont));
            paragraph6.KeepTogether = true;
            doc.Add(paragraph6);
            doc.Add(new Paragraph(" "));

            // Agregar texto del RichTextBox al PDF
            doc.Add(new Paragraph("Brief clinical history:", boldFont));
            
            Paragraph briefhis = new Paragraph(dataPathl[6]);
            briefhis.Alignment = Element.ALIGN_JUSTIFIED;
            doc.Add(briefhis);


            // Agregar imagen al PDF
            if (!string.IsNullOrEmpty(dataPathl[7]) && File.Exists(dataPathl[7]))
            {
                iTextSharp.text.Image pdfImage = iTextSharp.text.Image.GetInstance(dataPathl[7]);
                pdfImage.ScaleToFit(150f, 150f); // Ajustar tamaño de la imagen
                pdfImage.SetAbsolutePosition(doc.PageSize.Width - pdfImage.ScaledWidth-40, doc.PageSize.Height - pdfImage.ScaledHeight-55);
                doc.Add(pdfImage);
            }



            string footerImagePath = @"C:\Users\spjua\Desktop\Advanced Measuerments for Control Applications\Interface\FootPage.png";
            string backPath = @"C:\Users\spjua\Desktop\Advanced Measuerments for Control Applications\Interface\back.png";
            if (File.Exists(footerImagePath))
            {
                iTextSharp.text.Image footerImage = iTextSharp.text.Image.GetInstance(footerImagePath);
                footerImage.ScaleToFit(doc.PageSize.Width, footerImage.Height);
                footerImage.SetAbsolutePosition(0, 0);
                pdfWriter.DirectContent.AddImage(footerImage);
            }

            doc.Add(new Paragraph(" "));
            Paragraph paragraph10 = new Paragraph();
            paragraph10.Add(new Chunk("Roll and Pitch angles from Body [Degrees] vs Time [ds (deciseconds)]: ", boldFont));
            doc.Add(paragraph10);
            doc.Add(new Paragraph(" "));

            chart1.SaveImage("chart1.png", ChartImageFormat.Png);
            iTextSharp.text.Image chartImage1 = iTextSharp.text.Image.GetInstance("chart1.png");
            chartImage1.ScaleToFit(500f, 400f);
            doc.Add(chartImage1);

            doc.NewPage();

            Paragraph paragraph11 = new Paragraph();
            paragraph11.Add(new Chunk("X-Y Position of the center of mass [mm]: ", boldFont));
            doc.Add(paragraph11);
            doc.Add(new Paragraph(" "));

            chart2.SaveImage("chart2.png", ChartImageFormat.Png);
            iTextSharp.text.Image chartImage2 = iTextSharp.text.Image.GetInstance("chart2.png");
            chartImage2.ScaleToFit(260f, 180f);
            doc.Add(chartImage2);


            if (File.Exists(backPath))
            {
                iTextSharp.text.Image backImage = iTextSharp.text.Image.GetInstance(backPath);
                backImage.SetAbsolutePosition(280, 575);
                backImage.ScaleToFit(150f, 175f);
                pdfWriter.DirectContent.AddImage(backImage);
            }


            Paragraph paragraph12 = new Paragraph();
            paragraph12.Add(new Chunk("Angular speed around X-Y-Z axis [dps]: ", boldFont));
            doc.Add(paragraph12);
            doc.Add(new Paragraph(" "));

            chart3.SaveImage("chart3.png", ChartImageFormat.Png);
            iTextSharp.text.Image chartImage3 = iTextSharp.text.Image.GetInstance("chart3.png");
            chartImage3.ScaleToFit(400f, 300f);
            doc.Add(chartImage3);

            Paragraph paragraph13 = new Paragraph();
            paragraph13.Add(new Chunk("Angular acceleration around X-Y-Z axis [m/s^2]: ", boldFont));
            doc.Add(paragraph13);
            doc.Add(new Paragraph(" "));

            chart4.SaveImage("chart4.png", ChartImageFormat.Png);
            iTextSharp.text.Image chartImage4 = iTextSharp.text.Image.GetInstance("chart4.png");
            chartImage4.ScaleToFit(400f, 300f);
            doc.Add(chartImage4);



            if (File.Exists(footerImagePath))
            {
                iTextSharp.text.Image footerImage = iTextSharp.text.Image.GetInstance(footerImagePath);
                footerImage.ScaleToFit(doc.PageSize.Width, footerImage.Height);
                footerImage.SetAbsolutePosition(0, 0);
                pdfWriter.DirectContent.AddImage(footerImage);
            }

            doc.Close();

            MessageBox.Show("PDF creado con éxito en: " + pdfPath);

            Application.Exit();

        }

        private void timer5_Tick(object sender, EventArgs e)
        {
            Point currentLocation = pictureBox1.Location;
            int newx = currentLocation.X + 10;
            


            if (newx < 350)
            {
                pictureBox1.Location = new Point(newx, currentLocation.Y);

            }
            else
            {
                pictureBox1.Visible = false;
                pictureBox5.Visible = true;
                VoiceShark.aname("C:\\Users\\spjua\\Documents\\Backup\\JAI - DAQ v1.0\\JAI - DAQ v1.0\\bin\\Debug\\sharkcinco.wav");
                VoiceShark.StartS();
                pictureBox5.BringToFront();
                

                if (countan < 20)
                {
                    
                    countan++;
                    
                }
                else
                {
                    pictureBox5.Visible = false;
                    button8.Visible = true;
                   ;
                    button8.BringToFront();
                    label10.Visible = true;
                    label11.Visible = true;
                    label6.Visible = true;
                    label13.Visible = true;
                    label12.Visible = true;
                    timer5.Stop();
                }
            }



        }

        private void button8_Click(object sender, EventArgs e)
        {
            button8.Visible=false;
            timer6.Start();
            label9.Visible = true;
            label9.BringToFront();
            VoiceShark.StopS();
            pictureBox2.Visible = false;
            label8.Visible = false;
        }

        private void timer6_Tick(object sender, EventArgs e)
        {
            label9.Text = countshark.ToString(); 
            countshark--;


            if (countshark == 0)
            {

                sta = true;
                label9.Visible = false;
                pictureBox7.Visible = true;
                pictureBox7.BringToFront();
                pictureBox6.Visible = true;
                pictureBox6.BringToFront();
                timer7.Start();
               

            }

        }

        private void timer7_Tick(object sender, EventArgs e)
        {
            timegame--;
            label13.Text = timegame.ToString();

            if (timegame <= 0)
            {
                sta = false;

                for (int va = 0; va < 448; va++)
                {

                    chart1.Series["TargetRoll"].Points.AddXY(tsim[va], rsp[va]);
                    chart2.Series["XY - mm"].Points.AddXY(xsim[va]-430, ysim[va]-20);
                    chart3.Series["X-Ax-speed"].Points.AddXY(tsim[va], xv[va]);
                    chart3.Series["Y-Ax-speed"].Points.AddXY(tsim[va], yv[va]);
                    chart3.Series["Z-Ax-speed"].Points.AddXY(tsim[va], zv[va]);
                    chart4.Series["X-Ax-acce"].Points.AddXY(tsim[va], 9.81*xa[va]);
                    chart4.Series["Y-Ax-acce"].Points.AddXY(tsim[va], 9.81 * ya[va]);
                    chart4.Series["Z-Ax-acce"].Points.AddXY(tsim[va], 9.81 * za[va]);
                }

                pictureBox6.Visible= false;
                pictureBox7.Visible= false;
                timer7.Stop();
                pictureBox1.Location = new Point(12,170);
                pictureBox1.Visible = true;
                pictureBox2.Visible= true;
                label14.Visible = true;
                VoiceShark.aname("C:\\Users\\spjua\\Documents\\Backup\\JAI - DAQ v1.0\\JAI - DAQ v1.0\\bin\\Debug\\sharkseis.wav");
                VoiceShark.StartS();
                timer8.Start();

            }

        }

        private void timer8_Tick(object sender, EventArgs e)
        {
            if (length12 < data12.Length)
            {
                label14.Text = label14.Text + data12.ElementAt(length12);
                length12++;
            }
            else
            {
                timer8.Stop();
                VoiceShark.StopS();
                button7.Visible = true;
            }

        }


        public static class VoiceShark
        {
            public static SoundPlayer musicPlayer = new SoundPlayer();

            public static void aname(string filepath)
            {
                musicPlayer.SoundLocation = filepath;
            }

            public static void StopS()
            {
                musicPlayer.Stop();
            }

            public static void StartS()
            {
                musicPlayer.Play();
            }
        }



    }
}

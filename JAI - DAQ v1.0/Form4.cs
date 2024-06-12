using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Windows.Data.Pdf;
using System.Web.UI.WebControls;
using System.Windows.Markup;
using WMPLib;
using static JAI___DAQ_v1._0.Form2;


namespace JAI___DAQ_v1._0
{
    
    public partial class Form4 : Form

    {
        private string directorio;
       
        public Form4()
        {
            InitializeComponent();
            

        }

        private void button2_Click(object sender, EventArgs e)
        {

            if (this.pictureBox2.Image != null && textBox1.Text != "" && textBox2.Text != "" && numericUpDown1.Value != 0 && numericUpDown2.Value != 0 && numericUpDown3.Value != 0 && textBox6.Text != "" && richTextBox1.Text != "")
            {
                Form3 f3 = new Form3();
                f3.Show();
                Visible = false;
                MediaPlayerController.Stop();


                string[] dataPat = new string[8];

                dataPat[0] = textBox1.Text;
                dataPat[1] = textBox2.Text;
                dataPat[2] = numericUpDown1.Text;
                dataPat[3] = numericUpDown2.Text;
                dataPat[4] = numericUpDown3.Text;
                dataPat[5] = textBox6.Text;
                dataPat[6] = richTextBox1.Text;
                dataPat[7] = directorio ?? string.Empty;

                File.WriteAllLines("dataPath.txt", dataPat);


            }
            else
            {
                MessageBox.Show("There is missing information");
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    pictureBox2.Image = System.Drawing.Image.FromFile(openFileDialog.FileName);
                    pictureBox2.Tag = openFileDialog.FileName; // Guardar la ruta de la imagen
                    directorio = openFileDialog.FileName;
                }
            }
        }


    }
}

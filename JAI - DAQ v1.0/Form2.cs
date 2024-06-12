using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WMPLib;

namespace JAI___DAQ_v1._0
{
     public partial class Form2 : Form
    {
        
        public Form2()
        {
            InitializeComponent();
            MediaPlayerController.Start();
            this.Deactivate += new System.EventHandler(this.Form2_Deactivate);
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "Therapist" && textBox2.Text == "passcode")
            {
             
                Form4 f4 = new Form4();
                f4.Show();
                Visible = false;

            }
            else
            {
                MessageBox.Show("The Username or password are incorrect, please try again");
                textBox1.Clear();
                textBox2.Clear();
                textBox1.Focus();

            }
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            
        }


        private void Form2_Deactivate(object sender, EventArgs e)
        {
           // player.controls.stop();
        }

        public static class MediaPlayerController
        {
            public static WindowsMediaPlayer Player { get; private set; } = new WindowsMediaPlayer();

            static MediaPlayerController()
            {
                Player.URL = "A.m4a";
                Player.controls.play();
            }

            public static void Stop()
            {
                Player.controls.stop();
            }

            public static void Start()
            {
                Player.controls.play();
            }

        }


    }
}

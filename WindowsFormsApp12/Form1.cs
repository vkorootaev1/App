using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp12
{
    public partial class Form1 : Form
    {
        const string pass = "2110";
        public int t = 0;
        public Form1()
        {
            InitializeComponent();
            button1.Enabled = false;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            t++;
            if (t == 10)
            {
                timer1.Enabled = false;
                button1.Enabled = true;
                textBox1.Enabled = true;
                t = 0;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.TextLength == 0)
            {
                MessageBox.Show("Пароль не может быть пустым!", "Ошибка");
            }
            else
            {
                if (textBox1.Text == pass)
                {
                    this.Hide();
                    Form2 forms = new Form2(this);
                    forms.Show();
                }
                else
                {
                    MessageBox.Show("Пароль не верный! Повторите ввод через 10 секунд!", "Ошибка");
                    textBox1.Clear();
                    textBox1.Enabled = false;
                    button1.Enabled = false;
                    timer1.Enabled = true;
                }
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            textBox1.MaxLength = 6;
            if (textBox1.TextLength >= 0)
            {
                button1.Enabled = true;
            }
            else
            {
                char ch = e.KeyChar;
                if (!Char.IsDigit(ch) && ch != 8)
                {
                    e.Handled = true;
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.ControlBox = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void textBox1_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
        }
    }
}

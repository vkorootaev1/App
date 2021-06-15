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
    public partial class Form2 : Form
    {
        int n;
        int[,] arr;
        bool[] used;
        public Form2(Form1 f)
        {
            InitializeComponent();
            textBox3.ReadOnly = true;
            textBox4.ReadOnly = true;
            label2.Visible = false;
            textBox3.Visible = false;
            radioButton1.Enabled = false;
            radioButton2.Enabled = false;
            textBox1.Enabled = false;
            radioButton1.Checked = false;
            radioButton2.Checked = false;
            label3.Visible = false;
            textBox5.Visible = false;
            textBox5.ReadOnly = true;
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            this.ControlBox = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int count = 0;
           bool flag = true;
            int n1 = textBox2.Lines.Length;
            int n2 = textBox2.Lines[0].Split(' ').Length;
            if (n1 != n2)
            {
                flag = false;
                MessageBox.Show("Матрица смежности должна быть квадратной \n или введеное изначально количество вершин не совпадает с текущим!", "Ошибка");
            }
            else
            {
                int pr;
                n = n1;
                int p = 0;
                arr = new int[n, n];
                for (int i = 0; i < n; i++)
                {
                    string[] s = textBox2.Lines[i].Split(' ');
                    for (int j = 0; j < n; j++)
                    {
                        bool f = int.TryParse(s[j], out pr );
                        if ((pr == 0 || pr == 1) && f == true)
                        {
                            arr[i, j] = pr;
                        }
                        else
                        {
                            MessageBox.Show("Матрица смежности введена неправильно!", "Ошибка");
                            flag = false;
                        }
                    }
                }
                for(int i =0; i < n && flag == true;i++)
                {
                    for(int j = 0; j < n && flag == true;j++)
                    {
                        if(arr[i,j] != arr[j,i] || arr[p,p] != 0 )
                        {
                            MessageBox.Show("Матрица смежности введена неправильно!", "Ошибка");
                            flag = false;
                        }
                    }
                    p++;
                }
            }
            for (int i = 0; i < n && flag == true; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    textBox3.AppendText(arr[i, j].ToString() + "  ");
                }
                textBox3.AppendText("\n");
            }
            used = new bool[n];
            for(int i = 0; i < n;i++)
            {
                if (used[i] == false)
                {
                    Svaz(i, arr,used,n);
                    count++;
                }
            }
            if (count != 1 && !flag)
            {
                MessageBox.Show("Граф не связан! Исправьте матрицу смежности!", "Ошибка");
                flag = false;
                textBox3.Clear();
            }
            if (flag == true)
            {
                textBox3.Visible = true;
                label1.Visible = false;
                label2.Visible = true;
                textBox2.Visible = false;
                button1.Enabled = false;               
                textBox1.Enabled = true;
                textBox5.Visible = false;
            }
            if (flag)
            {
                label3.Visible = true;
                textBox5.Visible = true;
                int m = 0;
                for(int i = 0; i < n;i++)
                {
                    for(int j = 0;j < n;j++)
                    {
                        if (arr[i,j] == 1)
                        {
                            m++;
                        }
                    }
                }
                m = m / 2;
                int[,] arr_smeg = new int[n, m];
                for(int i = 0; i < n;i++)
                {
                    for(int j = 0; j < m;j++)
                    {
                        arr_smeg[i, j] = 0;
                    }
                }
                int r =0;
                for (int i = 0; i < n; i++)
                {
                        for (int j = 0; j < i; j++)
                        {
                            if (arr[i, j] == 1)
                            {
                                arr_smeg[i, r] = 1;
                                arr_smeg[j, r] = 1;
                                r++;
                            }
                        }
                }
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < m; j++)
                    {
                        textBox5.AppendText(arr_smeg[i, j].ToString() + "  ");
                    }
                    textBox5.AppendText("\n");
                }
            }
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            if (ch != 32 && ch != 8 && ch!= 48 && ch != 49 && ch!= 13 && textBox2.Text.Contains(','))
            {
                e.Handled = true;
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            if(!Char.IsDigit(ch) && ch != 8)
            {
                e.Handled = true;
            }
        }
        private void Obhod_vglubinu(int st, bool[] visit, int[,] arr) // метод обхода в глубину
        {
            textBox4.Text += st.ToString() + " ";
            visit[st] = true;
            for (int i = 0; i < n; i++)
            {
                if ((arr[st, i] != 0) && (!visit[i]))
                {
                    Obhod_vglubinu(i, visit, arr);
                }
            }
        }
        private void Obhod_vshirinu(int st, ref Queue<int> och, ref bool[] visit, int[,] arr) // метод обхода в ширину
        {
            visit[st] = true; //вершина посещена
            och.Enqueue(st);//помещаем вершину в пустую очередь(Enqueue()-метод класса Queue)
            while (och.Count != 0) // пока число эл-ов в очереди(метод Сount) не равно нулю - пока очередь не пуста
            {
                st = och.Peek();// берет вершину из начала очереди, НО НЕ УДАЛЯЕТ
                och.Dequeue(); //удаляет вершину из начала очереди
                textBox4.Text += st.ToString() + " ";
                for (int i = 0; i < n; i++) //перебираем все вершины, связные с st
                {
                    //если есть связь между вершинами графа и вершина не пройдена
                    if (arr[st, i] == 1)
                    {
                        if (!visit[i]) //если вершина не посещена
                        {
                            visit[i] = true; //отмечаем вершину i как пройденную
                            och.Enqueue(i); //и добавляем вершину в очередь
                        }
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox1.Enabled = true;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            
            if (textBox1.Text != "")
            {
                radioButton1.Checked = false;
                radioButton2.Checked = false;
                radioButton1.Enabled = true;
                radioButton2.Enabled = true;
            }
        }
        private void Svaz(int s, int [,] arr,bool [] used, int n)
        {
            used[s] = true;
            for(int i =0; i < n;i++)
            {
                if (used[i] == false && arr[s,i] == 1)
                {
                    Svaz(i, arr,used,n);
                }
            }
        }


        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked == true)
            {
                textBox4.Clear();
                int st = int.Parse(textBox1.Text);
                if (st >= 0 && st <= n - 1)
                {
                    bool [] visit1 = new bool[n];
                    for (int i = 0; i < n; i++)
                    {
                        visit1[i] = false;
                    }
                    Obhod_vglubinu(st, visit1, arr);
                }
                else
                {
                    radioButton1.Checked = false;
                    radioButton2.Checked = false;
                    MessageBox.Show("Введенной вершины нет! Измените ее! ", "Ошибка!");
                    radioButton1.Enabled = false;
                    radioButton2.Enabled = false;
                    textBox1.Clear();
                }
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if(radioButton2.Checked == true)
            {
                textBox4.Clear();
                int st = int.Parse(textBox1.Text);
                if (st >= 0 && st <= n - 1)
                {
                    bool[] visit2 = new bool[n];
                    for (int i = 0; i < n; i++)
                    {
                        visit2[i] = false;
                    }
                    Queue<int> och = new Queue<int>();
                    Obhod_vshirinu(st, ref och, ref visit2, arr);
                }
                else
                {
                    radioButton1.Checked = false;
                    radioButton2.Checked = false;
                    MessageBox.Show("Введенной вершины нет! Измените ее! ", "Ошибка!");
                    radioButton1.Enabled = false;
                    radioButton2.Enabled = false;
                    textBox1.Clear();
                }
            }
        }
    }
}

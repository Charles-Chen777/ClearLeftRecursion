using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LeftDigui
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        List<string> NotFinal = new List<string>() { "A","B","C","D","E","F","G","H","I","J","K","L","M","N","O","P","Q","R","S","T","U","V","W","X","Y","Z"};
        List<string> Final = new List<string>() { "id", "num", ")", "(", ";", "*", "/", "+", "-", "mod", "ε", "a", "b", "c", "d", "e", "f","g","h","i","j","k","l","m","n","o","p","q","r","s","t","u","v","w","x","y","z" };


        public Dictionary<string, string> TuidaoCollection = new Dictionary<string, string>();//推导字典
        public Dictionary<string, string> FirstCollection = new Dictionary<string, string>();//First字典
        public Dictionary<string, string> FollowCollection = new Dictionary<string, string>();//Follow字典

        string S = string.Empty;//文法开始符号

        private void Form2_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < listBox1.Items.Count; i++)
            {
                string[] temp = listBox1.Items[i].ToString().Split('→');
                TuidaoCollection.Add(temp[0], temp[1]);
            }

            string txt = string.Empty;
            foreach (var item in TuidaoCollection)
            {
                txt += item.Key + ";";
            }

            textBox1.Text = txt.Substring(0, txt.Length - 1);
            S = txt.Substring(0, 1);
            //DataTable da = new DataTable();
            //DataRow dr = new DataRow();

            //for (int i = 0; i < 3; i++)
            //{
            //    int index = this.dataGridView1.Rows.Add();
            //    this.dataGridView1.Rows[index].Cells[0].Value = "S->a";
            //    this.dataGridView1.Rows[index].Cells[1].Value = "";
            //    this.dataGridView1.Rows[index].Cells[2].Value = "";
            //    this.dataGridView1.Rows[index].Cells[3].Value = "";
            //    this.dataGridView1.Rows[index].Cells[4].Value = "aa";
            //    this.dataGridView1.Rows[index].Cells[4].Value = "bb";
            //    this.dataGridView1.Rows[index].Cells[4].Value = "";
            //}


            //dataGridView1.DataSource = da;
           
            //string jihe = textBox1.Text;
            //string[] temp = jihe.Split(';');
            //int index11 = 0;
            //for (int i = 0; i < dataGridView1.Rows.Count; i++)
            //{
            //    dataGridView1.Rows[i].HeaderCell.Value = temp[index11];
            //    index11++;
            //} 
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string[] Finalitem = this.textBox1.Text.Split(';');//语法涉及的所有非终结符;

            for (int k = Finalitem.Length - 1; k >= 0; k--)
            {
                string[] temp = TuidaoCollection[Finalitem[k]].Split('|');//获取当前终结符的所有推导式
                string tempFirst = string.Empty;
                for (int j = 0; j < temp.Length; j++)
                {
                    string Check = string.Empty;
                    if (Final.Contains(temp[j]))//整体是终结符
                    {
                        tempFirst += temp[j] + ",";
                    }
                    else
                    {
                        Check = FindFirstFinalHelp(temp[j]);

                        if (Check == "hit")//遇到的第一个是非终结符
                        {
                            tempFirst += FirstCollection[temp[j].Substring(0, 1)];
                        }
                        else
                        {
                            tempFirst += Check + ",";// 第一个字符是终结符
                        }
                    }
                }
                FirstCollection.Add(Finalitem[k], tempFirst);
            }

            //结果制表
            string result = string.Empty;
            foreach (var item in FirstCollection)
            {
                result += "First(" + item.Key + ")=" + "\t" + "{ ";
                result += item.Value.Substring(0, item.Value.Length-1) + " }";
                result += "\r\n";
            }
            textBox2.Text = result;
        }

        //找到第一个终结符的帮助方法
        public string FindFirstFinalHelp(string temp)
        {
            for(int i=0;i<temp.Length;i++)
            {
                if(NotFinal.Contains(temp[i].ToString()))
                {
                    if(i==0)
                    {
                        break;
                    }
                    if(i==1)
                    {
                        return temp[0].ToString();
                    }
                    else
                    {
                        return temp.Substring(0, i);
                    }              
                }
            }
            return "hit";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string[] Finalitem = this.textBox1.Text.Split(';');//语法涉及的所有非终结符;

            FollowCollection.Add(Finalitem[0], "#,");//文法开始符号以输入结束标记结束
            //从文法开始符号开始分析
            for (int k = 1; k < Finalitem.Length; k++)
            {
                //如果是E`(E的化简推到)
                if (Finalitem[k].Contains("`"))
                {
                    FollowCollection.Add(Finalitem[k], FollowCollection[Finalitem[k].Replace("`", "")]);
                }
                else
                {
                    //string[] temp = TuidaoCollection[Finalitem[k]].Split('|');//获取当前终结符的所有推导式
                    string tempFollow = string.Empty;
                    //for (int j = 0; j < temp.Length; j++)
                    //{
                    //    FindFollowFinal(temp[j].ToCharArray());
                    //}
                    tempFollow += FindFollowFinalHelp(Finalitem[k]);

                    FollowCollection.Add(Finalitem[k], tempFollow);
                }

            }

            //结果制表
            string result = string.Empty;
            foreach (var item in FollowCollection)
            {

                string[] processs = item.Value.Split(',');
                string realvalue = string.Empty;
                for (int i = 0; i < processs.Length; i++)
                {
                    if (!realvalue.Contains(processs[i]) && processs[i] != "ε")//去重去空
                    {
                        realvalue += processs[i] + ",";
                    }
                }
                result += "Follow(" + item.Key + ")=" + "\t" + "{ ";
                result += realvalue.Substring(0, realvalue.Length - 1) + " }";
                result += "\r\n";
            }
            textBox3.Text = result;
        }

        public string FindFollowFinalHelp(string temp)
        {
            string follow=string.Empty;
            foreach (var item in TuidaoCollection)
            {
                string check = temp + "`";
                string nowitemvalue = item.Value.Replace(check, "X");

                int index = nowitemvalue.IndexOf(temp);
                if (index != -1)//含有E
                {
                    string now = nowitemvalue[index + 1].ToString();
                    if (Final.Contains(now))//如果他后面是终结符--直接返回
                    {
                        follow += now+",";
                    }
                    else//如果不是，返回该非终结符的First集合
                    {
                        string nowfinal = string.Empty;
                        if (nowitemvalue[index + 2].ToString() == "`")
                        {
                            nowfinal = now + "`";
                            follow += FirstCollection[nowfinal];
                            
                        }
                        else
                        {
                            nowfinal = now;
                            follow += FirstCollection[nowfinal];
                        }

                        //检测该非终结符的导出式子是否含有ε
                        if(TuidaoCollection[nowfinal].Contains("ε"))
                        {
                            follow += FollowCollection[nowfinal];
                        }
                        
                    }
                }
            }

            return follow;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            List<Dictionary<string, string>> DictionaryList = new List<Dictionary<string, string>>();
            for(int i=0;i<6;i++)
            {
                Dictionary<string, string> temp = new Dictionary<string, string>();//推导字典
                DictionaryList.Add(temp);
            }
            

        }
    }
}

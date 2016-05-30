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

        List<string> Stack = new List<string>();//检测用户输入所用栈
        List<string> UserData = new List<string>();//用户输入栈

        Dictionary<string, Dictionary<string, string>> DictionaryList = new Dictionary<string, Dictionary<string, string>>();//顶级文法字典

        string result = string.Empty;//推导过程记录
        int resultindex = 1;//推导过程步骤

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
                    if (Final.Contains(temp[j]) ||temp[j]== "ε")//整体是终结符
                    {
                        tempFirst += temp[j] + ",";
                    }
                    else
                    {
                        Check = FindFirstFinalHelp(temp[j]);

                        if (Check == "hit")//遇到的第一个是非终结符
                        {
                            string nextfirst=FirstCollection[temp[j].Substring(0, 1)];
                            if (!tempFirst.Contains(nextfirst))
                            {
                                tempFirst += nextfirst;
                            }
                            
                            if(Final.Contains(temp[j].Substring(1, 1)))
                            {
                                tempFirst += temp[j].Substring(1, 1) + ",";
                            }
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

                List<int> indexall=new List<int>();
                for(int i=0;i<item.Value.Length;i++)
                {
                    if(item.Value[i].ToString()==temp)
                    {
                        indexall.Add(i);
                    }
                }
                for (int i = 0; i < indexall.Count; i++)
                {
                    //int index = nowitemvalue.IndexOf(temp);
                    if (indexall[i] != -1)//含有E
                    {
                        string now = nowitemvalue[indexall[i] + 1].ToString();
                        if (Final.Contains(now))//如果他后面是终结符--直接返回
                        {
                            follow += now + ",";
                        }
                        else//如果不是，返回该非终结符的First集合
                        {
                            string nowfinal = string.Empty;
                            if (nowitemvalue[indexall[i] + 2].ToString() == "`")
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
                            if (TuidaoCollection[nowfinal].Contains("ε"))
                            {
                                follow += FollowCollection[nowfinal];
                            }

                        }
                    }
                }
            }

            return follow;
        }

        private void button3_Click(object sender, EventArgs e)
        {

            //List<Dictionary<string, Dictionary<string, string>>> DictionaryList = new List<Dictionary<string, Dictionary<string, string>>>();

            textBox5.Text = "";
            DictionaryList.Clear();
            Stack.Clear();
            UserData.Clear();
            resultindex = 1;
            if (DictionaryList.Count == 0)
            {
                string[] NotFinalitem = this.textBox1.Text.Split(';');//语法涉及的所有非终结符;
                List<string> Finalitem = new List<string>();////语法涉及的所有终结符;

                #region 填充语法涉及的所有终结符Finalitem;
                foreach (var item in FirstCollection)
                {
                    string[] processs = item.Value.Split(',');
                    for (int i = 0; i < processs.Length; i++)
                    {
                        if (!Finalitem.Contains(processs[i]) && processs[i] != "ε" && processs[i] != "")//去重去空
                        {
                            Finalitem.Add(processs[i]);
                        }
                    }
                }

                foreach (var item in FollowCollection)
                {
                    string[] processs = item.Value.Split(',');
                    for (int i = 0; i < processs.Length; i++)
                    {
                        if (!Finalitem.Contains(processs[i]) && processs[i] != "ε" && processs[i] != "")//去重去空
                        {
                            Finalitem.Add(processs[i]);
                        }
                    }
                }

                #endregion

                for (int i = 0; i < NotFinalitem.Length; i++)
                {
                    //Dictionary<string, Dictionary<string, string>> temp_top = new Dictionary<string, Dictionary<string, string>>();
                    Dictionary<string, string> temp = new Dictionary<string, string>();
                    for (int j = 0; j < Finalitem.Count; j++)
                    {
                        #region First集合造表
                        if (FirstCollection[NotFinalitem[i]].Contains(Finalitem[j]))
                        {

                            string[] value = TuidaoCollection[NotFinalitem[i]].Split('|');
                            //处理
                            for (int u = 0; u < value.Length;u++ )
                            {
                                string tttt = value[u];
                                for(int p=0;p<tttt.Length;p++)
                                {
                                    if(NotFinal.Contains(tttt[p].ToString())&&TuidaoCollection[tttt[p].ToString()]=="ε")
                                    {
                                        value[u]=value[u].Replace(tttt[p].ToString(), "");
                                    }
                                }
                            }
                                for (int k = 0; k < value.Length; k++)
                                {
                                    if (value[k][0].ToString() == Finalitem[j][0].ToString())//恰好匹配第一个终结符
                                    {
                                        temp.Add(Finalitem[j], value[k]);
                                        break;
                                    }
                                    if (value.Length == 1)//只能导出一个式子
                                    {
                                        temp.Add(Finalitem[j], value[k]);
                                        break;
                                    }
                                    if (value.Length == 2 && value[1] == "ε")//导出两个式子但第二个为空
                                    {
                                        temp.Add(Finalitem[j], value[k]);
                                        break;
                                    }
                                }
                        }
                        #endregion

                        //Follow集合造表
                        else if (FollowCollection[NotFinalitem[i]].Contains(Finalitem[j]))
                        {
                            temp.Add(Finalitem[j], "ε");
                        }
                    }
                    DictionaryList.Add(NotFinalitem[i], temp);
                    //DictionaryList.Add(temp_top);
                }
            }
            

            //推导用户输入开始；
            
            result += "Step" + "\t" + "Stack" + "\t\t" + "UData" + "\r\n"; 

            Stack.Add("#");//压入#
            Stack.Add(S);//压入文法开始符号
            string txt = textBox4.Text;
            txt += "#";
            //string[] UserDatatemp = textBox4.Text.Split(',');
            
            for(int i=0;i<txt.Length;i++)
            {
                UserData.Add(txt[i].ToString());
            }
            PrintResult();
            int Index = 0;
            

            while(UserData.Count!=1)
            {
                string nowtop = gettop();//获取栈顶
                if(nowtop=="#"&&UserData[Index]=="#")
                {
                    break;//当栈顶为#号时验证完毕；符合规范
                }
                else
                {
                    if(nowtop==UserData[Index])//栈顶元素和用户当前输入一致
                    {
                        UserData.Remove(nowtop);
                        pop(nowtop);
                    }
                    else if (nowtop == "ε" || UserData[Index]=="#")//遇到空
                    {
                        pop(nowtop);
                    }
                    else
                    {
                        try
                        {
                            Dictionary<string, string> temp = DictionaryList[nowtop];//获得栈顶非终结符的表格字典
                            pop(nowtop);//pop栈顶
                            string check = temp[UserData[Index]];
                            push(check);//反向压栈字典查到的推导
                        }
                        catch
                        {
                            MessageBox.Show("查表遇到异常，用户输入不符合规范");
                            break;
                        }
                    }
                }
                PrintResult();
            }

            if (UserData.Count==1&&gettop()!="#")
            {
                pop(gettop());
                PrintResult();
            }
            textBox5.Text = result;
            if (gettop() == "#" && UserData[Index] == "#")
            {
                MessageBox.Show("符合规范！");

            }
            else
            {
                MessageBox.Show("不符合规范");
            }
            
        }

        //反向压栈
        public void push(string temp)
        {
            if (Final.Contains(temp))
            {
                Stack.Add(temp);//压入纯终结符
            }
            else
            {
                for (int i = temp.Length - 1; i >= 0; i--)
                {
                    if (temp[i].ToString()=="`")
                    {
                        Stack.Add(temp[i-1].ToString()+"`");//压入E`
                        i--;
                    }
                    else
                    {
                        Stack.Add(temp[i].ToString());
                    }
                    
                }
            }

        }
        //出栈
        public void pop(string temp)
        {
            Stack.Remove(temp);
        }
        //获取栈顶
        public string gettop()
        {
            return Stack[Stack.Count-1];
        }

        //输出结果
        public void PrintResult()
        {
            result += "(" + resultindex + ")" + "\t";
            for(int i=0;i<Stack.Count;i++)
            {
                result += Stack[i];
            }
            result += "\t\t";
            for (int i = 0; i < UserData.Count; i++)
            {
                result += UserData[i];
            }
            result += "\r\n";
            resultindex++;
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }
}

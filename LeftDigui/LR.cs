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
    public partial class LR : Form
    {
        List<string> NotFinal = new List<string>() { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
        List<string> Final = new List<string>() { "id", "num", ")", "(", ";", "*", "/", "+", "-", "mod", "ε", "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z" };


        public Dictionary<string, string> TuidaoCollection = new Dictionary<string, string>();//推导字典
        public Dictionary<string, string> FirstCollection = new Dictionary<string, string>();//First字典
        public Dictionary<string, string> FollowCollection = new Dictionary<string, string>();//Follow字典

        List<string> Stack = new List<string>();//检测用户输入所用栈
        List<string> UserData = new List<string>();//用户输入栈

        Dictionary<string, Dictionary<string, string>> DictionaryList = new Dictionary<string, Dictionary<string, string>>();//顶级文法字典

        string result = string.Empty;//推导过程记录
        int resultindex = 1;//推导过程步骤

        string S = string.Empty;//文法开始符号

        List<Status> StatusList = new List<Status>();//状态集;
        int StatusNameIndex = 0;
        Dictionary<Status, Dictionary<string, Status>> StatusDictionary = new Dictionary<Status, Dictionary<string, Status>>();

        Helpler Helpler = new Helpler();

        List<string> HasProcessed = new List<string>();
        public LR()
        {
            InitializeComponent();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();

            foreach (var item in TuidaoCollection)
            {
                string temp = item.Key;
                if (temp == S)
                {
                    listBox1.Items.Add(temp + "`" + "→" + temp);
                    textBox1.Text = temp + "`"+";"+textBox1.Text;
                }
                string[] tuidao = TuidaoCollection[temp].Split('|');
                for (int i = 0; i < tuidao.Length; i++)
                {
                    listBox1.Items.Add(temp + "→" + tuidao[i]);
                }
            }
            TuidaoCollection.Add(S + "`", S);
        }

        private void LR_Load(object sender, EventArgs e)
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
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string[] Finalitem = this.textBox1.Text.Split(';');//语法涉及的所有非终结符;
            Helpler.GetFirstCollection(ref Finalitem, ref TuidaoCollection, ref FirstCollection);

            //结果制表
            string result = string.Empty;
            foreach (var item in FirstCollection)
            {
                result += "First(" + item.Key + ")=" + "\t" + "{ ";
                result += item.Value.Substring(0, item.Value.Length - 1) + " }";
                result += "\r\n";
            }
            textBox2.Text = result;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string[] Finalitem = this.textBox1.Text.Split(';');//语法涉及的所有非终结符;
            Helpler.GetFollowCollection(ref Finalitem, ref TuidaoCollection, ref FollowCollection, ref FirstCollection);

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

        private void button3_Click(object sender, EventArgs e)
        {
            string startadd = S + "`";
            string now = startadd;
            HasProcessed.Add(now);
            List<string> starttuidao = new List<string>();
            for (int i = 0; ; i++)
            {
                try
                {
                    starttuidao.Add(now + "→" + "." + TuidaoCollection[now]);
                    now = TuidaoCollection[now][0].ToString();
                    HasProcessed.Add(now);
                }
                catch
                {
                    break;
                }
            }

            Status Start = new Status(starttuidao, GetStatusName(), true, false);
            StatusList.Add(Start);//添加到状态列表
            for (int i = 0; i < StatusList.Count; i++)
            {
                if (StatusList[i].Cancontinue == true)
                {
                    ProcessStatus(StatusList[i]);
                }
            }

            string stop = string.Empty;
        }
            

        public void ProcessStatus(Status temp)
        {
            List<string> Tuidaoji = new List<string>();
            List<string> Temp = temp.Tuidaoji;
            Dictionary<string, Status> SSS = new Dictionary<string, Status>();
            List<string> Others2=new List<string>();
            string After2 = string.Empty;
            string After = string.Empty;
            bool CanOutside = true;
            for(int i=0;i<Temp.Count;i++)
            {
                After = GetcharAfterpot(Temp[i]);
                List<string> Others = GetOthers(After);
                if(Others.Count==0)
                {
                    string txt = ProcessStr(Temp[i].ToCharArray());
                    //Others.Add(txt);
                    int Indexofpot = GetIndexofpot(txt);
                    //string txt2 = txt.Replace("`", "");                   
                    if (Indexofpot == txt.Length - 1)
                    {
                        Others.Add(txt);
                        Status NewStatus = new Status(Others, GetStatusName(), false, true);
                        SSS.Add(After, NewStatus);
                        StatusList.Add(NewStatus);//添加到状态列表
                    }
                    else
                    {
                        After2 = GetcharAfterpot(txt);
                        //if(HasProcessed.IndexOf(After2)!=-1)
                        //{
                        List<string> TempOthers2 = GetOthers(After2);
                        for (int ii = 0; ii < TempOthers2.Count;ii++ )
                        {
                            Others2.Add(TempOthers2[ii]);//对于其他的推导，同意加到Others2
                        }
                        Others2.Add(txt);

                        if (Final.Contains(After2)&&HasOtherJump(temp, After2))//没有其他的当前符号的跳转，则立马加入
                        {                         
                            CanOutside=false;
                            Status NewSSS = new Status(Others2, GetStatusName(), true, false);
                            SSS.Add(After, NewSSS);

                            StatusList.Add(NewSSS);//添加到状态列表
                            StatusDictionary.Add(temp, SSS);//添加到状态字典
                            Others2.Clear();
                        }
                    }
                }
                else//对于已经处理过的集合符号，直接后移，加入Others2
                {
                    string txt = ProcessStr(Temp[i].ToCharArray());
                    Others2.Add(txt);
                }        
            }

            if(CanOutside==true)
            {
                Status NewSSS = new Status(Others2, GetStatusName(), true, false);
                SSS.Add(After, NewSSS);

                StatusList.Add(NewSSS);//添加到状态列表
                StatusDictionary.Add(temp, SSS);//添加到状态字典
            }

        }

        public bool HasOtherJump(Status temp,string After)
        {
            for(int i=0;i<temp.Tuidaoji.Count;i++)
            {
                int index = temp.Tuidaoji[i].IndexOf(".");
                if (temp.Tuidaoji[i].Substring(index, temp.Tuidaoji[i].Length - index).Contains(After))
                {
                    return false;
                }
            }
            return true;
        }

        public string ProcessStr(char[] temp)
        {
            string txt = string.Empty;

            for(int i=0;i<temp.Length;i++)
            {
                if(temp[i].ToString()==".")
                {
                    txt += temp[i + 1];
                    txt += ".";
                    i += 1;
                }
                else
                {
                    txt += temp[i];
                }            
            }
            return txt;
        }
        public List<string> GetOthers(string temp)
        {
            string now = temp;
            List<string> starttuidao = new List<string>();
            if (HasProcessed.Contains(now) || Final.Contains(temp))
            {
                return starttuidao;
            }
            else
            {
                for (int i = 0; i < TuidaoCollection.Count; i++)
                {
                    if (TuidaoCollection[now].Contains('|') || Final.Contains(TuidaoCollection[now]))
                    {
                        string[] multy = TuidaoCollection[now].Split('|');
                        for (int j = 0; j < multy.Length; j++)
                        {
                            starttuidao.Add(now + "→" + "." + multy[j]);
                        }
                        HasProcessed.Add(now);
                        break;
                    }
                    else
                    {
                        starttuidao.Add(now + "→" + "." + TuidaoCollection[now]);
                        now = TuidaoCollection[now][0].ToString();
                        HasProcessed.Add(now);
                    }

                }
            }
            return starttuidao;
        }

        public string GetStatusName()
        {
            string temp= "I" + StatusNameIndex.ToString();
            StatusNameIndex++;
            return temp;
        }

        public int GetIndexofpot(string Tuidao)
        {
            return Tuidao.IndexOf(".");
        }

        public string GetcharAfterpot(string Tuidao)
        {
            int index = GetIndexofpot(Tuidao);
            if(index==Tuidao.Length)
            {
                return "#";
            }
            else
            {
                return Tuidao[index + 1].ToString();
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeftDigui
{
    public class Helpler
    {

        List<string> NotFinal = new List<string>() { "A","B","C","D","E","F","G","H","I","J","K","L","M","N","O","P","Q","R","S","T","U","V","W","X","Y","Z"};
        List<string> Final = new List<string>() { "id", "num", ")", "(", ";", "*", "/", "+", "-", "mod", "ε", "a", "b", "c", "d", "e", "f","g","h","i","j","k","l","m","n","o","p","q","r","s","t","u","v","w","x","y","z" };
        public void GetFirstCollection(ref string[] Finalitem, ref Dictionary<string, string> TuidaoCollection, ref Dictionary<string, string> FirstCollection)
        {
            //string[] Finalitem = this.textBox1.Text.Split(';');//语法涉及的所有非终结符;

            for (int k = Finalitem.Length - 1; k >= 0; k--)
            {
                string[] temp = TuidaoCollection[Finalitem[k]].Split('|');//获取当前终结符的所有推导式
                string tempFirst = string.Empty;
                for (int j = 0; j < temp.Length; j++)
                {
                    string Check = string.Empty;
                    if (Final.Contains(temp[j]) || temp[j] == "ε")//整体是终结符
                    {
                        tempFirst += temp[j] + ",";
                    }
                    else
                    {
                        Check = FindFirstFinalHelp(temp[j]);

                        if (Check == "hit")//遇到的第一个是非终结符
                        {
                            if (temp[j].Substring(0, 1) == Finalitem[k])//含有直接左递归
                            {
                                if (temp[j].Length>1)
                                {
                                    if (NotFinal.Contains(temp[j].Substring(1, 1)))
                                    {
                                        string nextfirst = FirstCollection[temp[j].Substring(0, 1)];
                                        if (!tempFirst.Contains(nextfirst))
                                        {
                                            tempFirst += nextfirst;
                                        }
                                    }
                                    else
                                    {
                                        if (!tempFirst.Contains(temp[j].Substring(1, 1)))
                                        {
                                            tempFirst += temp[j].Substring(1, 1) + ",";
                                        }                                 
                                    }
                                }

                            }
                            else
                            {
                                string nextfirst = FirstCollection[temp[j].Substring(0, 1)];
                                if (!tempFirst.Contains(nextfirst))
                                {
                                    tempFirst += nextfirst;
                                }
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

        public void GetFollowCollection(ref string[] Finalitem, ref Dictionary<string, string> TuidaoCollection, ref Dictionary<string, string> FollowCollection, ref Dictionary<string, string> FirstCollection)
        {
            //从文法开始符号开始分析
            for (int k = 0; k < Finalitem.Length; k++)
            {

                if (!TuidaoCollection[Finalitem[k]].Contains(Finalitem[k]))
                {
                    if (!CheckItemInCollection(Finalitem[k], TuidaoCollection))
                    {
                        FollowCollection.Add(Finalitem[k], "#");
                        continue;
                    }

                }

                //string[] temp = TuidaoCollection[Finalitem[k]].Split('|');//获取当前终结符的所有推导式
                string tempFollow = string.Empty;
                //for (int j = 0; j < temp.Length; j++)
                //{
                //    FindFollowFinal(temp[j].ToCharArray());
                //}
                tempFollow += FindFollowFinalHelp(Finalitem[k], TuidaoCollection, FirstCollection, FollowCollection);

                FollowCollection.Add(Finalitem[k], tempFollow);


            }
        }

        public bool CheckItemInCollection(string temp, Dictionary<string, string> TuidaoCollection)
        {
            foreach (var item in TuidaoCollection)
            {
                if(item.Value.Contains(temp))
                {
                    return true;
                }
            }
            return false;
        }

        public string FindFollowFinalHelp(string temp, Dictionary<string, string> TuidaoCollection, Dictionary<string, string> FirstCollection, Dictionary<string, string> FollowCollection)
        {
            string follow = string.Empty;
            foreach (var item in TuidaoCollection)
            {
                string check = temp + "`";
                string nowitemvalue = item.Value.Replace(check, "X");

                List<int> indexall = new List<int>();
                for (int i = 0; i < item.Value.Length; i++)
                {
                    if (item.Value[i].ToString() == temp)
                    {
                        indexall.Add(i);
                    }
                }
                for (int i = 0; i < indexall.Count; i++)
                {
                    //int index = nowitemvalue.IndexOf(temp);
                    if (indexall[i] != -1)//含有E
                    {
                        if(nowitemvalue.Length==1)
                        {
                            follow += "#";
                        }
                        else
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
            }

            return follow;
        }
    }


    public class Status
    {
        public List<string> Tuidaoji;
        public string StatusName;
        public bool Cancontinue;
        public bool HasGuiyue;

        public Status(List<string> Tuidaoji, string StatusName, bool Cancontinue, bool HasGuiyue)
        {
            this.Tuidaoji = Tuidaoji;
            this.StatusName = StatusName;
            this.Cancontinue = Cancontinue;
            this.HasGuiyue = HasGuiyue;
        }

        public int GetIndexofpot(string Tuidao)
        {
            return Tuidao.IndexOf(".");
        }

    }
}

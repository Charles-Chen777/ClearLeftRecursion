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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.radioButton1.Select();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string Start = this.textBox7.Text;
            string Exp = this.textBox6.Text;
            ClearLeftRecursion(Start, Exp);

        }

        /// <summary>
        /// 算法1 消除直接左递归
        /// </summary>
        public void ClearLeftRecursion(string Start,string Exp)
        {
            string[] temp = Exp.Split('|');//以|分割推导结果；
            if (temp.Length == 0)
            {
                MessageBox.Show("表达式有误");
                return;
            }
            List<string> Alpha = new List<string>();//Aalpha集合
            List<string> Beta = new List<string>();//beta集合
            for (int i = 0; i < temp.Length; i++)
            {
                if (temp[i].Contains(Start))
                {
                    Alpha.Add(temp[i].Replace(Start, ""));//Aalpha集合
                }
                else
                {
                    if (temp[i] == "ε")
                    {
                        Beta.Add("");//beta集合
                    }
                    else
                    {
                        Beta.Add(temp[i]);//beta集合
                    }
                    
                }
            }

            string newStart = Start + "`";

            string result = Start + " → ";

            for (int i = 0; i < Beta.Count; i++)
            {
                if (i + 1 != Beta.Count)
                {
                    result += Beta[i] + newStart + " | ";
                }
                else
                {
                    result += Beta[i] + newStart;
                }
            }

            result += "\r\n";

            result += newStart + " → ";

            for (int i = 0; i < Alpha.Count; i++)
            {
                result += Alpha[i] + newStart + " | ";
            }
            result += "ε";

            this.textBox5.Text = result;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string Start1=string.Empty;
            string Exp1=string.Empty;
            string Start2=string.Empty;
            string Exp2=string.Empty;
            if (this.radioButton1.Checked)
            {
                Start1 = textBox1.Text;
                Exp1 = textBox3.Text;
                Start2 = textBox2.Text;
                Exp2 = textBox4.Text;
            }
            else
            {
                Start1 = textBox2.Text;
                Exp1 = textBox4.Text;
                Start2 = textBox1.Text;
                Exp2 = textBox3.Text;
            }


            string[] temp1 = Exp1.Split('|');
            string[] temp2 = Exp2.Split('|');

            //string newExp = Start2 + "→";
            string newExp = string.Empty;

            for(int i=0;i<temp2.Length;i++)
            {
                if(temp2[i].Contains(Start1))
                {
                    for(int j=0;j<temp1.Length;j++)
                    {
                        if (temp1[j]!="ε")
                        {
                            newExp += temp2[i].Replace(Start1, temp1[j]) + "|";
                        }
                        else
                        {
                            newExp += temp2[i].Replace(Start1, "") + "|";
                        }
                        
                    }
                }
                else
                {
                    if (i + 1 != temp2.Length)
                    {
                        newExp += temp2[i] + "|";
                    }
                    else
                    {
                        newExp += temp2[i];
                    }              
                }
            }

            ClearLeftRecursion(Start2, newExp);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string Start1 = string.Empty;
            string Exp1 = string.Empty;
            string Start2 = string.Empty;
            string Exp2 = string.Empty;
            string Start3 = string.Empty;
            string Exp3=string.Empty;



                Start1 = textBox1.Text;
                Exp1 = textBox3.Text;
                Start2 = textBox2.Text;
                Exp2 = textBox4.Text;
                Start3 = textBox8.Text;
                Exp3 = textBox9.Text;


            string[] temp1 = Exp1.Split('|');
            string[] temp2 = Exp2.Split('|');
            string[] temp3 = Exp3.Split('|');

            //string newExp = Start2 + "→";
            string newExp = string.Empty;

            string nnExp = string.Empty;

            for (int i = 0; i < temp1.Length; i++)
            {
                if (temp1[i].Contains(Start2))
                {
                    for (int j = 0; j < temp2.Length; j++)
                    {
                        if (temp2[j] != "ε")
                        {
                            if (j + 1 == temp2.Length)
                            {
                                newExp += temp1[i].Replace(Start2, temp2[j]);
                            }
                            else
                            {
                                newExp += temp1[i].Replace(Start2, temp2[j]) + "|";
                            }
                            
                        }
                        else
                        {
                            newExp += temp1[i].Replace(Start2, "") + "|";
                        }

                    }
                }
                else
                {
                    if (i + 1 == temp1.Length)
                    {
                        newExp += temp1[i];
                    }
                    else
                    {
                        newExp += temp1[i]+ "|";
                    }
                }

                string[] tempnewExp = newExp.Split('|');
                
                for (int k = 0; k < tempnewExp.Length;k++ )
                {

                    if (tempnewExp[k].Contains(Start3))
                    {
                        for (int j = 0; j < temp3.Length; j++)
                        {
                            if (temp3[j] != "ε")
                            {
                                if (k + 1 == tempnewExp.Length)
                                {
                                    nnExp += tempnewExp[k].Replace(Start3, temp3[j]) ;
                                }
                                else
                                {
                                    nnExp += tempnewExp[k].Replace(Start3, temp3[j]) + "|";
                                }
                                

                            }
                            else
                            {
                                nnExp += tempnewExp[k].Replace(Start3, "") + "|";
                            }

                        }
                    }
                    else
                    {
                        if (k + 1 == temp1.Length)
                        {
                            nnExp += tempnewExp[k] + "|";
                        }
                        else
                        {
                            nnExp += tempnewExp[k];
                        }
                    }
                }

            }

            ClearLeftRecursion(Start1, nnExp);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string Start1 = textBox11.Text;
            string Exp1 = textBox10.Text;
            string Start2 = textBox13.Text;
            string Exp2 = textBox12.Text;

            string[] temp1 = Exp1.Split('|');
            string[] temp2 = Exp2.Split('|');


            //string[] subtemp = GetAllSub(temp1[0]);
            string result=ClearLeftYinzi(temp1, Start1);

            string next=ClearLeftYinzi(temp2, Start2);
            if (next=="")
            {
                result += "\r\n";

                result += Start2 + " → " + Exp2;
            }

            this.textBox5.Text = result;

        }


        public string[] GetAllSub(string temp)
        {
            string[] result = new string[temp.Length];
            for(int i=0;i<temp.Length;i++)
            {
                result[i] = temp.Substring(0, i + 1);
            }

            return result;
        }


        public string ClearLeftYinzi(string[] temp1, string Start1)
        {
            string[] subtemp = GetAllSub(temp1[0]);
            string longestsub = string.Empty;
            for (int i = 1; i < temp1.Length; i++)
            {
                for (int j = 0; j < subtemp.Length; j++)
                {
                    if (temp1[i].Contains(subtemp[j]))
                    {
                        longestsub = subtemp[j];
                        continue;
                    }
                }
            }

            if(longestsub=="")
            {
                return "";
            }
            List<string> Beta = new List<string>();//Beta集合
            List<string> Gama = new List<string>();//Gama集合

            for (int i = 0; i < temp1.Length; i++)
            {
                if (temp1[i].Contains(longestsub))
                {


                    if (temp1[i] == longestsub)
                    {
                        Beta.Add("ε");
                    }
                    else
                    {
                        Beta.Add(temp1[i].Replace(longestsub, ""));
                    }
                }
                else
                {
                    Gama.Add(temp1[i]);
                }
            }



            string result = Start1 + " → ";

            for (int i = 0; i < Gama.Count; i++)
            {
                if (i + 1 != Gama.Count)
                {
                    result += longestsub + Start1 + "`" + " | ";
                }
                else
                {
                    result += longestsub + Start1 + "`";
                }
            }

            result += "\r\n";

            result += Start1 + "`" + " → ";

            for (int i = 0; i < Beta.Count; i++)
            {
                if (i + 1 != Beta.Count)
                {
                    result += Beta[i] + " | ";
                }
                else
                {
                    result += Beta[i];
                }
            }

            return result;

            //this.textBox5.Text = result;
        }
    }
}

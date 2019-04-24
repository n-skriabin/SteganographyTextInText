using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SteganographyText
{
    public partial class Form1 : Form
    {
        private Dictionary<char, char> AlphabetDictionary;  //<eng(1), ru(0)>

        public Form1()
        {
            InitializeComponent();
            AlphabetDictionary = new Dictionary<char, char>();

            AlphabetDictionary.Add('a', 'а');
            AlphabetDictionary.Add('o', 'о');
            AlphabetDictionary.Add('p', 'р');
            AlphabetDictionary.Add('y', 'у');
            AlphabetDictionary.Add('c', 'с');
            AlphabetDictionary.Add('e', 'е');
            AlphabetDictionary.Add('x', 'х');

            AlphabetDictionary.Add('A', 'А');
            AlphabetDictionary.Add('O', 'О');
            AlphabetDictionary.Add('P', 'Р');
            AlphabetDictionary.Add('H', 'Н');
            AlphabetDictionary.Add('C', 'С');
            AlphabetDictionary.Add('E', 'Е');
            AlphabetDictionary.Add('B', 'В');
            AlphabetDictionary.Add('M', 'М');
            AlphabetDictionary.Add('X', 'Х');
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox_container.Text != string.Empty) {

                var stringBitArrayMessage = ToBinaryString(Encoding.Unicode, textBox_message.Text);
                int messageCounter = 0;
                var stringContainer = textBox_container.Text;
                var resultCharArrayContainer = textBox_container.Text.ToCharArray();

                for (int i = 0; i < stringContainer.Length - 1; i++)
                {
                    if (AlphabetDictionary.TryGetValue(stringContainer[i], out char value1) && stringBitArrayMessage[messageCounter] == '0')
                    {
                        resultCharArrayContainer[i] = value1;
                        messageCounter++;
                    } else if (AlphabetDictionary.TryGetValue(stringContainer[i], out char value2))
                    {
                        messageCounter++;
                    }

                    if (messageCounter == stringBitArrayMessage.Length)
                    {
                        for (int j = i; j < stringContainer.Length - 1; j++)
                        {
                            if (stringContainer[j] == ' ') //not spicific space
                            {
                                resultCharArrayContainer[j] = ' '; //spicific space (EN_SPACE, 8194)
                                messageCounter++;
                                break;
                            }
                        }
                        break;
                    }
                }

                if (messageCounter < stringBitArrayMessage.Length)
                {
                    MessageBox.Show("Container is very small.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    textBox_result.Text = new string(resultCharArrayContainer);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var sourceMessage = textBox_container.Text;
            string stringBitArrayMessage = string.Empty;

            if (sourceMessage!= string.Empty)
            {
                for (int i = 0; i < sourceMessage.Length; i++)
                {
                    if (sourceMessage[i] == ' ')
                    {
                        var residual = stringBitArrayMessage.Length % 8;

                        if (residual != 0)
                        {
                            stringBitArrayMessage = stringBitArrayMessage.Substring(0, stringBitArrayMessage.Length - residual);
                        }
                        break;
                    }

                    if (AlphabetDictionary.TryGetValue(sourceMessage[i], out char value))
                    {
                        stringBitArrayMessage += 1;
                        continue;
                    }
                    var test = AlphabetDictionary.FirstOrDefault(x => x.Value == sourceMessage[i]).Key;
                    if (AlphabetDictionary.FirstOrDefault(x => x.Value == sourceMessage[i]).Key != '\0')
                    {
                        stringBitArrayMessage += 0;
                    }
                }

                var resultString = BinaryToString(stringBitArrayMessage);
                textBox_result.Text = resultString;
            }
            else
            {
                MessageBox.Show("Container is empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        static string ToBinaryString(Encoding encoding, string text)
        {
            return string.Join("", encoding.GetBytes(text).Select(n => Convert.ToString(n, 2).PadLeft(8, '0')));
        }

        public static string BinaryToString(string data)
        {
            List<Byte> byteList = new List<Byte>();

            for (int i = 0; i < data.Length; i += 8)
            {
                byteList.Add(Convert.ToByte(data.Substring(i, 8), 2));
            }
            var result = Encoding.Unicode.GetString(byteList.ToArray());
            return result;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }        
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace Calculator__Framework_
{

    public partial class Form1 : Form
    {

        List<string> term_input = new List<string>();
        List<@operator> op_input = new List<@operator>();
        List<Button> buttons = new List<Button>();
        List<Button> numbers_buttons = new List<Button>();

        int control = 0;

        public Form1()
        {
            InitializeComponent();
            buttons.AddRange(new List<Button>() { button1, button12, button22, button15, button16, button17, button18 });
            numbers_buttons.AddRange(new List<Button>() { button14, button13, button3, button4, button5, button6, button7, button8, button9, button10, button11, button21 });
        }

        private void Write_number(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            cleaner();
            equation.Text += button.Text;
        }

        private void Main_operator(object sender, EventArgs e)
        {
            if (equation.Text == string.Empty)
            {
                return;   
            }
            else 
            {
                Button button = (Button)sender;
                term_input.Add(equation.Text);
                show.Text += equation.Text;
                show.Text += " " + button.Text;
                op_input.Add(new @operator { input = char.Parse(button.Text), priority = @operator.Priority(char.Parse(button.Text)) });

                equation.Clear();
            }
        }

        private void counter(object sender, EventArgs e)
        {
            Button button = (Button)sender;

            if (term_input.Count == 0)
            {
                return;
            }
            else
            {
                if (term_input.Contains("mod"))
                {
                    term_input.Add(equation.Text);
                    show.Text += equation.Text + " ";
                    double temp1 = double.Parse(term_input[0]);
                    double temp2 = double.Parse(term_input[2]);
                    double result = temp1 % temp2;
                    equation.Text = Convert.ToString(result);
                    show.Text += " = " + Convert.ToString(result);
                    foreach (var x in buttons)
                    {
                        x.Enabled = true;
                    }
                    control++;
                    equation.Clear();
                    term_input.Clear();
                    return;
                }

                if (term_input.Contains("!"))
                {
                    if (Convert.ToInt32(term_input[0]) > 25)
                    {
                        show.Text = "Cannot calculate permutations of integers bigger than 25!";
                        return;
                    }
                    else
                    {
                        long permutation = 1;
                        int element = Convert.ToInt32(term_input[0]);
                        while (element > 1)
                        {
                            permutation = permutation * element;
                            element--;
                        }
                        equation.Text = Convert.ToString(permutation);
                        show.Text += "= " + Convert.ToString(permutation);
                        foreach (var x in buttons)
                        {
                            x.Enabled = true;
                        }
                        foreach (var x in numbers_buttons)
                        {
                            x.Enabled = true;
                        }
                        control++;
                        term_input.Clear();
                        equation.Clear();
                        return; }
                }
                else
                {
                    term_input.Add(equation.Text);
                    show.Text += " " + equation.Text;
                    equation.Clear();
                    string result_temp = "";
                    do
                    {
                        int index = order_finder();
                        result_temp = calculator(term_input[index], op_input[index].input, term_input[index + 1]);
                        equation.Text = result_temp;
                        term_input[index] = result_temp;
                        term_input.RemoveAt(index + 1);
                        op_input.RemoveAt(index);
                    } while (term_input.Count > 1);

                    equation.Text = result_temp;
                    show.Text += " = " + result_temp;
                    control++;
                    term_input.Clear();
                    equation.Clear();
                    return;
                }
            }
        }

        private void Modulo(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            term_input.Clear();
            term_input.Add(equation.Text);
            show.Text = equation.Text + " mod ";
            term_input.Add("mod");
            equation.Clear();
            foreach (var x in buttons)
            {
                x.Enabled = false;
            }
        }

        private void Clear(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            term_input.Clear();
            op_input.Clear();
            show.Clear();
            equation.Clear();

            foreach (var x in buttons)
            {
                x.Enabled = true;
            }
            foreach (var x in numbers_buttons)
            {
                x.Enabled = true;
            }
        }

        private void negative_number(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            if (equation.Text == string.Empty)
            {
                equation.Text += "-";
            }
        }

        private void permutation(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            term_input.Clear();
            double temp = double.Parse(equation.Text);
            int perm = (int)temp;
            term_input.Add(Convert.ToString(perm));
            show.Text = equation.Text + "! ";
            term_input.Add("!");
            equation.Clear();
            foreach (var x in buttons)
            {
                x.Enabled = false;
            }
            foreach (var x in numbers_buttons)
            {
                x.Enabled = false;
            }
        }

        private void point(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            if (!equation.Text.Contains("."))
            {
                equation.Text += ".";
            }
        }

        //Method
        void cleaner()
        {
            if (control != 0)
            {
                show.Clear();
                equation.Clear();
                control = 0;
            }
        }

        //Method
        public int order_finder()
        {
            int indexer = 0;
            for (var i = 0; i < op_input.Count; i++)
            {
                if (op_input[i].priority > op_input[indexer].priority)
                    indexer = i;
            }

            return indexer;
        }

        //Method
        private string calculator(string first_term, char op, string second_term)
        {
            double x = double.Parse(first_term);
            double y = double.Parse(second_term);

            switch (op)
            {
                case '^':
                    return Math.Pow(x, y).ToString();
                case '/':
                    return (x / y).ToString();
                case '*':
                    return (x * y).ToString();
                case '-':
                    return (x - y).ToString();
                default:
                    return (x + y).ToString();
            }
        }

    }


    public class @operator
    {
        public char input { get; set; }
        public int priority { get; set; }


        //Method
        public static int Priority(char op)
        {
            int priority = 0;
            if (op == '*' || op == '/')
                priority++;

            if (op == '^')
                priority += 2;

            return priority;
        }


    }

}

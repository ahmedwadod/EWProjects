using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NKH.MindSqualls;
using NKH.MindSqualls.MotorControl;

namespace MazeBot
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        McNxtBrick nxt;
        McNxtMotorSync motors;

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        enum USSDirection
        {
            Right = 90, Left = -90, Straight = 0
        };

        USSDirection currentDir = USSDirection.Straight;
        void USS_Direct(USSDirection dir)
        {
            if (!nxt.IsMotorControlRunning())
                nxt.StartMotorControl();
            switch (dir)
            {
                case USSDirection.Straight:
                    if(currentDir == USSDirection.Right)
                    {
                        nxt.MotorA = new McNxtMotor();
                        nxt.MotorA.Run(-100, 90);
                    }
                    if (currentDir == USSDirection.Left)
                    {
                        nxt.MotorA = new McNxtMotor();
                        nxt.MotorA.Run(100, 90);
                    }
                    currentDir = USSDirection.Straight;
                    break;

                case USSDirection.Right:
                    if (currentDir == USSDirection.Straight)
                    {
                        nxt.MotorA = new McNxtMotor();
                        nxt.MotorA.Run(100, 90);
                    }
                    if (currentDir == USSDirection.Left)
                    {
                        nxt.MotorA = new McNxtMotor();
                        nxt.MotorA.Run(100, 180);
                    }
                    currentDir = USSDirection.Right;
                    break;

                case USSDirection.Left:
                    if (currentDir == USSDirection.Straight)
                    {
                        nxt.MotorA = new McNxtMotor();
                        nxt.MotorA.Run(-100, 90);
                    }
                    if (currentDir == USSDirection.Right)
                    {
                        nxt.MotorA = new McNxtMotor();
                        nxt.MotorA.Run(-100, 180);
                    }
                    currentDir = USSDirection.Left;
                    break;
            }
            System.Threading.Thread.Sleep(900);
        }

        bool MoveState { get
            {
                if (((NxtUltrasonicSensor)nxt.Sensor4).DistanceCm < 20)
                    return false;
                else
                    return true;
            } }


        bool jump = false;
        uint MA = 0;
        void MainLoop()
        {
            
            if (!nxt.IsMotorControlRunning())
                nxt.StartMotorControl();

            if (MoveState && !jump)
            {
                if (currentDir != USSDirection.Straight)
                    USS_Direct(USSDirection.Straight);
                motors.Run(100, Convert.ToByte(Convert.ToInt16(textBox2.Text)), 0);
                MA++;
                System.Threading.Thread.Sleep(1000);
                return;
            }

            USS_Direct(USSDirection.Right);
            if (MoveState)
            {
                motors.Run(100, Convert.ToByte(Convert.ToInt16(textBox5.Text)), Convert.ToSByte(Convert.ToInt16(textBox4.Text)));
                System.Threading.Thread.Sleep(1000);
                USS_Direct(USSDirection.Straight);
                jump = false;
                return;
            }
            else
            {
                USS_Direct(USSDirection.Left);
                if (MoveState)
                {
                    motors.Run(100, Convert.ToByte(Convert.ToInt16(textBox5.Text)), Convert.ToSByte(Convert.ToInt16(textBox3.Text)));
                    System.Threading.Thread.Sleep(1000);
                    USS_Direct(USSDirection.Straight);
                    jump = false;
                    return;
                }
                else
                {
                    jump = true;
                    for (uint i = MA; i > 0; i--)
                    {
                        motors.Run(-100, 229, 0);
                        System.Threading.Thread.Sleep(800);
                    }
                    return;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            nxt = new McNxtBrick(NxtCommLinkType.Bluetooth, Convert.ToByte(Convert.ToInt16(textBox1.Text)));
            nxt.Sensor4 = new NxtUltrasonicSensor();
            nxt.Sensor4.PollInterval = 50;
            nxt.Connect();
            if (!nxt.IsConnected)
            {
                label1.Text = "Connection Error!";
                return;
            }
            else
            {
                nxt.MotorB = new McNxtMotor();
                nxt.MotorC = new McNxtMotor();
                motors = new McNxtMotorSync(nxt.MotorB, nxt.MotorC);
                ((NxtUltrasonicSensor)nxt.Sensor4).PollInterval = 20;
                ((NxtUltrasonicSensor)nxt.Sensor4).Poll();
                label1.Text = "Connected";
                button1.Enabled = false;
            }

            while (true)
            {
                MainLoop();
            }
        }
    }
}

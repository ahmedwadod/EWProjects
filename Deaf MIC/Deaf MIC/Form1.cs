using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Speech.Recognition;

namespace Deaf_MIC
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        SpeechRecognitionEngine engine = new SpeechRecognitionEngine();

        private void Form1_Load(object sender, EventArgs e)
        {
            Grammar gm = new DictationGrammar();
            engine.LoadGrammar(gm);
            engine.SpeechRecognized += Engine_SpeechDetected;
        }

        private void Engine_SpeechDetected(object sender, SpeechRecognizedEventArgs e)
        {
            StatusLabel.Text = "Processing";
            engine.RecognizeAsyncStop();
            string[] words = e.Result.Text.ToLower().Split(' ');
            foreach (string word in words)
            {
                foreach (char c in word)
                {
                    switch (c)
                    {
                        case 'a':
                            output("a.jpg");
                            break;
                        case 'b':
                            output("b.jpg");
                            break;
                        case 'c':
                            output("c.jpg");
                            break;
                        case 'd':
                            output("d.jpg");
                            break;
                        case 'e':
                            output("e.jpg");
                            break;
                        case 'f':
                            output("f.jpg");
                            break;
                        case 'g':
                            output("g.jpg");
                            break;
                        case 'h':
                            output("h.jpg");
                            break;
                        case 'i':
                            output("i.jpg");
                            break;
                        case 'j':
                            output("j.jpg");
                            break;
                        case 'k':
                            output("k.jpg");
                            break;
                        case 'l':
                            output("l.jpg");
                            break;
                        case 'm':
                            output("m.jpg");
                            break;
                        case 'n':
                            output("n.jpg");
                            break;
                        case 'o':
                            output("o.jpg");
                            break;
                        case 'p':
                            output("p.jpg");
                            break;
                        case 'q':
                            output("q.jpg");
                            break;
                        case 'r':
                            output("r.jpg");
                            break;
                        case 's':
                            output("s.jpg");
                            break;
                        case 't':
                            output("t.jpg");
                            break;
                        case 'u':
                            output("u.jpg");
                            break;
                        case 'v':
                            output("v.jpg");
                            break;
                        case 'w':
                            output("w.jpg");
                            break;
                        case 'x':
                            output("x.jpg");
                            break;
                        case 'y':
                            output("y.jpg");
                            break;
                        case 'z':
                            output("z.jpg");
                            break;
                    }

                }

                output("null");
                StatusLabel.Text = "Pantomiming";
            }
            timer1.Start();
        }

        List<string> files = new List<string>();
        void output(string file)
        {
            files.Add(file);
        }

        private void Engine_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            
        }

        bool stop = false;
        private void StartStopButtton_Click(object sender, EventArgs e)
        {
            if(stop == false)
            {
                StartStopButtton.Text = "Stop";
                engine.SetInputToDefaultAudioDevice();
                engine.RecognizeAsync(RecognizeMode.Multiple);
                stop = true;
                StatusLabel.Text = "Listening";
            }
            else
            {
                StartStopButtton.Text = "Start";
                engine.RecognizeAsyncStop();
                stop = false;
            }
        }

        int i = 0;
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (i < files.Count)
            {
                pictureBox1.Image = (files[i] != "null") ? Image.FromFile(files[i]) : null;
                i++;
            }
            else
            {
                i = 0;
                files = new List<string>();
                timer1.Stop();
                engine.RecognizeAsync(RecognizeMode.Multiple);
                StatusLabel.Text = "Listening";
            }
        }
    }
}

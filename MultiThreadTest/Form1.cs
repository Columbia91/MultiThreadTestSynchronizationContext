using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MultiThreadTest
{
    public partial class MainForm : Form
    {
        private Worker _worker;

        public SynchronizationContext _context { get; private set; }

        public MainForm()
        {
            InitializeComponent();

            startButton.Click += StartButton_Click;
            stopButton.Click += StopButton_Click;
            Load += MainForm_Load;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            _context = SynchronizationContext.Current;
        }

        private void StopButton_Click(object sender, EventArgs e)
        {
            if (_worker != null)
                _worker.Cancel();
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            _worker = new Worker();
            _worker.ProcessChanged += Worker_ProcessChanged;
            _worker.WorkCompleted += Worker_WorkCompleted;

            startButton.Enabled = false;

            Thread thread = new Thread(_worker.Work);
            thread.Start(_context);
        }
        
        private void Worker_WorkCompleted(bool cancelled)
        {
            string message = cancelled ? "Процесс отменен" : "Процесс завершен!";
            MessageBox.Show(message);
            startButton.Enabled = true;
        }

        private void Worker_ProcessChanged(int progress)
        {
            progressBar.Value = progress + 1; // чтобы при нажатии на стоп прогрессбар незамедлительно останавливался
            progressBar.Value = progress;
        }
    }
}

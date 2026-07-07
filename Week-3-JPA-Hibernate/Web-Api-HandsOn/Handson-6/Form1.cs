using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Confluent.Kafka;

namespace WinFormsKafkaChat
{
    public partial class Form1 : Form
    {
        private const string BootstrapServers = "localhost:9092";
        private const string TopicName = "local-chat-stream";
        
        private IProducer<Null, string>? _producer;
        private CancellationTokenSource? _cts;

        // Visual Form Controls Layout Blueprint Components
        private TextBox txtUsername = null!;
        private TextBox txtMessage = null!;
        private ListBox lstChatBox = null!;
        private Button btnSend = null!;
        private Button btnConnect = null!;

        public Form1()
        {
            InitializeComponentLayoutStructures();
        }

        private void InitializeComponentLayoutStructures()
        {
            this.Text = "Distributed WinForms Kafka Chat Platform Component Client";
            this.Size = new System.Drawing.Size(520, 480);

            txtUsername = new TextBox { Location = new System.Drawing.Point(20, 20), Width = 120, Text = "Client_User" };
            btnConnect = new Button { Location = new System.Drawing.Point(150, 18), Text = "Initialize Hub Connection", Width = 150 };
            lstChatBox = new ListBox { Location = new System.Drawing.Point(20, 60), Width = 460, Height = 300 };
            txtMessage = new TextBox { Location = new System.Drawing.Point(20, 380), Width = 350 };
            btnSend = new Button { Location = new System.Drawing.Point(380, 378), Text = "Send", Width = 100, Enabled = false };

            btnConnect.Click += BtnConnect_Click;
            btnSend.Click += BtnSend_Click;

            this.Controls.AddRange(new Control[] { txtUsername, btnConnect, lstChatBox, txtMessage, btnSend });
        }

        private void BtnConnect_Click(object? sender, EventArgs e)
        {
            var producerConfig = new ProducerConfig { BootstrapServers = BootstrapServers };
            _producer = new ProducerBuilder<Null, string>(producerConfig).Build();

            _cts = new CancellationTokenSource();
            
            // Spin up an isolated process loop capturing events from alternative network locations
            Task.Run(() => ListenToKafkaBrokerChannel(_cts.Token));

            btnConnect.Enabled = false;
            txtUsername.ReadOnly = true;
            btnSend.Enabled = true;
            this.Text += $" - Authenticated as ({txtUsername.Text})";
        }

        private async void BtnSend_Click(object? sender, EventArgs e)
        {
            if (_producer != null && !string.IsNullOrWhiteSpace(txtMessage.Text))
            {
                string structuredMsg = $"[{DateTime.Now:HH:mm:ss}] {txtUsername.Text}: {txtMessage.Text}";
                
                await _producer.ProduceAsync(TopicName, new Message<Null, string> { Value = structuredMsg });
                txtMessage.Clear();
            }
        }

        private void ListenToKafkaBrokerChannel(CancellationToken token)
        {
            var consumerConfig = new ConsumerConfig
            {
                BootstrapServers = BootstrapServers,
                GroupId = $"winforms-client-group-{Guid.NewGuid()}", // Unique per window instance instance replication loop
                AutoOffsetReset = AutoOffsetReset.Latest
            };

            using var consumer = new ConsumerBuilder<Ignore, string>(consumerConfig).Build();
            consumer.Subscribe(TopicName);

            try
            {
                while (!token.IsCancellationRequested)
                {
                    var result = consumer.Consume(token);
                    
                    // Safely invoke list box record changes back across threads to prevent cross-threading UI locks
                    this.Invoke(new Action(() => {
                        lstChatBox.Items.Add(result.Message.Value);
                        lstChatBox.TopIndex = lstChatBox.Items.Count - 1; // Auto scroll down to incoming item indices
                    }));
                }
            }
            catch (OperationCanceledException) { consumer.Close(); }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            _cts?.Cancel();
            _producer?.Dispose();
            base.OnFormClosing(e);
        }
    }
}
using PeerToPeerServer.Services;
using PeerToPeerServer.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PeerToPeerServer
{
    public partial class Main : Form
    {
        private ServiceHost host;
        private Log logging;

        public Main()
        {
            InitializeComponent();
            buttonStopServer.Enabled = false;
            logging = Log.GetInstance();
            logging.InfoChanged += onLogInfoChange;
            logging.PeersChanged += onConnectedPeersChange;
        }

        private void buttonStartServer_Click(object sender, EventArgs e)
        {
            host = new ServiceHost(typeof(SuperPeerService));
            host.Open();

            // server is successfully running
            buttonStopServer.Enabled = true;
            buttonStartServer.Enabled = false;
            logging.LogI("Info: *** Server is running ***" + Environment.NewLine);
        }

        private void buttonStopServer_Click(object sender, EventArgs e)
        {
            if (host != null)
            {
                if (host.State != CommunicationState.Closed)
                {
                    host.Close();
                }
            }

            // server is successfully stopped
            buttonStopServer.Enabled = false;
            buttonStartServer.Enabled = true;
            logging.LogI("Info: *** Server stoped ***" + Environment.NewLine);
        }

        private void onLogInfoChange(object sender, LogEventArg<String> e) {
            textBoxLogInfo.Text += e.Data;
        }

        private void onConnectedPeersChange(object sender, LogEventArg<List<String>> e)
        {
            listBoxConnectedPeers.DataSource = e.Data;
        }
    }
}

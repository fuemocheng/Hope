using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;

//临时房间
public class RoomManager : InstanceBase<RoomManager>
{
    Thread searchThread;

    public delegate void SearchComplete(List<string> hosts);
    public bool isHost = false;
    public SearchComplete OnSearchComplete;

    private List<string> hosts = new List<string>();

    public override void Init()
    {
    }

    public void CreateRoom()
    {
        isHost = true;

        RoomServerManager.Instance.Host("192.168.133.97", 9998);
    }

    public void SearchHost(SearchComplete complete)
    {
        searchThread = new Thread(new ThreadStart(SearchHostThread));
        searchThread.Start();

        OnSearchComplete = complete;
    }

    public override void DoUpdate()
    {
    }

    private void SearchHostThread()
    {
        var hostname = Dns.GetHostName();
        var ipEntry = Dns.GetHostEntry(hostname);

        string ip = null;
        for (int i = 0; i < ipEntry.AddressList.Length; i++)
        {
            if (ipEntry.AddressList[i].AddressFamily == AddressFamily.InterNetwork)
            {
                ip = ipEntry.AddressList[i].ToString();
                break;
            }
        }

        if (string.IsNullOrEmpty(ip))
            return;

        var gateway = ip.Substring(0, ip.LastIndexOf('.') + 1);
        var hostslist = new List<string>();

        CleanOtherHost();

        for (int i = 0; i <= 255; i++)
        {
            try
            {
                var ping = new System.Net.NetworkInformation.Ping();
                var ipadrr = gateway + i;
                var reply = ping.Send(ipadrr, 5);
                if (reply.Status == IPStatus.Success)
                {
                    hostslist.Add(ipadrr);
                }
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
            }
        }

        for (int i = 0; i < hostslist.Count; i++)
        {
            var request = (HttpWebRequest)WebRequest.Create("http://" + hostslist[i] + "/");
            request.Method = "GET";
            request.Timeout = 1000;
            request.KeepAlive = false;

            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    Stream responseStream = response.GetResponseStream();
                    string responseStr = new StreamReader(responseStream).ReadToEnd();
                    responseStream.Close();

                    hosts.Add(hostslist[i]);
                }
            }
            catch/* (WebException e)*/
            {

            }
        }

        DIDAMain.Instance.RunOnMainThread(() =>
        {
            lock (hosts)
            {
                OnSearchComplete(hosts);
            }
        });
    }

    public override void DoDestroy()
    {
        CleanOtherHost();

        if (searchThread != null)
            searchThread.Abort();
    }

    public void CleanOtherHost()
    {
    }
}
using System;
using System.Net.Sockets;
using UnityEngine;

public class ClientTCP
{
    private static TcpClient clientSocket;
    private static NetworkStream myStream;
    private static byte[] receiveBuffer;
    public static ClientManager clientManager = GameObject.Find("ClientManager").GetComponent<ClientManager>();
  

    public static void InitializeClientSocket(string address, int port)
    {
        clientSocket = new TcpClient();
        clientSocket.ReceiveBufferSize = 4096;
        clientSocket.SendBufferSize = 4096;
        receiveBuffer = new byte[4096 * 2];
        clientSocket.BeginConnect(address, port, new AsyncCallback(ClientConnectCallback), clientSocket);
    }

    private static void ClientConnectCallback(IAsyncResult result)
    {
        clientSocket.EndConnect(result);
        if (clientSocket.Connected == false)
        {
            return;
        }
        else
        {
            myStream = clientSocket.GetStream();
            myStream.BeginRead(receiveBuffer, 0, 4096 * 2, ReceiveCallback, null);
        }
    }
    private static void ReceiveCallback(IAsyncResult result)
    {
        try
        {
            int readBytes = myStream.EndRead(result);
            if (readBytes <= 0)
            {
                return;
            }

            byte[] newBytes = new byte[readBytes];
            Buffer.BlockCopy(receiveBuffer, 0, newBytes, 0, readBytes);

            UnityThread.executeInUpdate(() =>
            {
                ClientHandleData.HandleData(newBytes);
            });

            myStream.BeginRead(receiveBuffer, 0, 4096 * 2, ReceiveCallback, null);
        }
        catch (Exception)
        {
            throw;
        }
    }
    private static void SendData(byte[] data)
    {
        ByteBuffer buffer = new ByteBuffer();
        buffer.WriteInteger((data.GetUpperBound(0) - data.GetLowerBound(0)) + 1);
        buffer.WriteBytes(data);
        myStream.Write(buffer.ToArray(), 0, buffer.ToArray().Length);
        buffer.Dispose();
    }

    public static void PACKAGE_Login(string username, string password)
    {
        ByteBuffer buffer = new ByteBuffer();
        buffer.WriteInteger((int)ClientPackages.CLogin);
        buffer.WriteString(username);
        buffer.WriteString(password);
        SendData(buffer.ToArray());
    }
    public static void PACKAGE_SearchOpponent()
    {
        ByteBuffer buffer = new ByteBuffer();
        buffer.WriteInteger((int)ClientPackages.CSearchOpponent);
        buffer.WriteString(clientManager.PlayerUsername);

        SendData(buffer.ToArray());
    }

    #region Match Packages
    public static void PACKAGE_Match_SetReady(int matchID)
    {
        ByteBuffer buffer = new ByteBuffer();
        buffer.WriteInteger((int)ClientPackages.CSetReady);
        buffer.WriteInteger(matchID);
        SendData(buffer.ToArray());
    }
    public static void PACKAGE_Match_ToggleCard(int matchID, int selectedCardPosition)
    {
        ByteBuffer buffer = new ByteBuffer();
        buffer.WriteInteger((int)ClientPackages.CSendToggleCard);
        buffer.WriteInteger(matchID);
        buffer.WriteInteger(selectedCardPosition);
        SendData(buffer.ToArray());
    }
    public static void PACKAGE_Match_Shot(int matchID)
    {
        ByteBuffer buffer = new ByteBuffer();
        buffer.WriteInteger((int)ClientPackages.CShot);
        buffer.WriteInteger(matchID);

        SendData(buffer.ToArray());
    }
    public static void PACKAGE_Match_RequestRestartMatch(int matchID)
    {
        ByteBuffer buffer = new ByteBuffer();
        buffer.WriteInteger((int)ClientPackages.CRestartMatch);
        buffer.WriteInteger(matchID);

        SendData(buffer.ToArray());
    }
    #endregion


}


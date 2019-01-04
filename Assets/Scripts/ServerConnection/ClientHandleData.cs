using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class ClientHandleData
{
    private static ByteBuffer playerBuffer;
    public delegate void Packet_(byte[] data);
    public static Dictionary<int, Packet_> packetListener;
    private static int pLength;

    public static void InitializePacketListener()
    {
        packetListener = new Dictionary<int, Packet_>();
        packetListener.Add((int)ServerPackages.SLoadMenu, HandleLoadMenu);
        packetListener.Add((int)ServerPackages.SLoadMatch, HandleLoadMatch);
        packetListener.Add((int)ServerPackages.SSendCards, HandleSendedCards);
    }

    public static void HandleData(byte[] data)
    {
        //Copying our packet information into a temporary array to edit and peek it
        byte[] buffer = (byte[])data.Clone();

        //Checking if the connected player which sent this package has an instance of the bytebuffer
        // in order to read out the information of the byte[]buffer
        if (playerBuffer == null)
        {
            //if there is no instance, then create a new one
            playerBuffer = new ByteBuffer();
        }

        //Reading out the package from the player in order to check which package it actually is.
        playerBuffer.WriteBytes(buffer);

        // Checking if the received package is empty, if so then do not continue executing this code
        if (playerBuffer.Count() == 0)
        {
            playerBuffer.Clear();
            return;
        }

        //Checking if the package actually contains information
        if (playerBuffer.Length() >= 4)
        {
            //Read out  the full package length
            pLength = playerBuffer.ReadInteger(false);

            if (pLength <= 0)
            {
                //if there is no package or is invalid then close this method
                playerBuffer.Clear();
                return;
            }
        }

        while (pLength > 0 & pLength <= playerBuffer.Length() - 4)
        {
            //if (pLength <= playerBuffer.Length() - 4){
            playerBuffer.ReadInteger();
            data = playerBuffer.ReadBytes(pLength);
            HandleDataPackages(data);
            //}

            pLength = 0;
            if (playerBuffer.Length() >= 4)
            {
                pLength = playerBuffer.ReadInteger(false);
            }

            if (pLength <= 0)
            {
                //if there is no package or is invalid then close this method
                playerBuffer.Clear();
                return;
            }

            if (pLength <= 1)
            {
                playerBuffer.Clear();
            }
        }
    }

    private static void HandleDataPackages(byte[] data)
    {
        ByteBuffer buffer = new ByteBuffer();
        buffer.WriteBytes(data);
        int packageID = buffer.ReadInteger();

        Packet_ packet;
        if (packetListener.TryGetValue(packageID, out packet))
        {
            packet.Invoke(data);
        }
    }

    private static void HandleLoadMenu(byte[] data)
    {

        ByteBuffer buffer = new ByteBuffer();
        buffer.WriteBytes(data);
        int packageID = buffer.ReadInteger();
        string playerUsername = buffer.ReadString();
        ClientManager.playerUsername = playerUsername;
        ClientManager.LoadMenu();
    }

    private static void HandleLoadMatch(byte[] data)
    {
        ByteBuffer buffer = new ByteBuffer();
        buffer.WriteBytes(data);
        int packageID = buffer.ReadInteger();
        int matchID = buffer.ReadInteger();

        //Change Architecture in future
        string enemyUsername = buffer.ReadString();
        ClientManager.enemyUsername = enemyUsername;

        ClientManager.LoadMatch(matchID);
    }

    private static void HandleSendedCards(byte[] data)
    {
        
        ByteBuffer buffer = new ByteBuffer();
        buffer.WriteBytes(data);
        int packageID = buffer.ReadInteger();
        for (int i = 0; i < 4; i++)
        {
            string cardName = buffer.ReadString();
            Card.CardTypes cardType = (Card.CardTypes)buffer.ReadInteger();
            int damage = buffer.ReadInteger();
            PlayerMatchManager.PrintCard(cardName, cardType, damage, ClientManager.playerUsername);
        }
        
        
    }

}


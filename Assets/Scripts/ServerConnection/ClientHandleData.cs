using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class ClientHandleData
{
    private static ByteBuffer playerBuffer;
    public delegate void Packet_(byte[] data);
    public static Dictionary<int, Packet_> packetListener;
    private static int pLength;

    public static ClientManager clientManager = GameObject.Find("ClientManager").GetComponent<ClientManager>();
    public static MatchManager matchManager;

    public static void InitializePacketListener()
    {
        packetListener = new Dictionary<int, Packet_>
        {
            {(int) ServerPackages.SLoadMenu, HandleLoadMenu},
            {(int) ServerPackages.SLoadMatch, HandleLoadMatch},
            {(int) ServerPackages.SSendMatchCards, Handle_Match_SendedCards},
            {(int) ServerPackages.SStartRound, Handle_Match_StartRound},
            {(int) ServerPackages.SShowCards, Handle_Match_ShowCards},
            {(int) ServerPackages.SSendAllCardsData, Handle_Client_AllCardsData},
            {(int) ServerPackages.SShowResult, Handle_Match_ShowResult},
            {(int) ServerPackages.SFinishGame, Handle_Match_FinishGame},
            {(int) ServerPackages.SConfirmToggleCard, Handle_Match_ConfirmToggleCard}

        };
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

    #region SceneLoading Handling
    private static void HandleLoadMenu(byte[] data)
    {
        clientManager.LoadMenu(data);
    }
    private static void HandleLoadMatch(byte[] data)
    {
        clientManager.LoadMatch(data);
    }
    #endregion

    #region Data Saving/Loading Handling
    private static void Handle_Client_AllCardsData(byte[] data)
    {
        clientManager.SaveAllCardsData(data);
    }
    #endregion

    #region Match Packages Hadnling
    private static void Handle_Match_StartRound(byte[] data)
    {
        ByteBuffer buffer = new ByteBuffer();
        buffer.WriteBytes(data);
        matchManager.StartRound(buffer);
    }
    private static void Handle_Match_SendedCards(byte[] data)
    {
        var buffer = new ByteBuffer();
        buffer.WriteBytes(data);
        matchManager.HandleSendedCards(buffer);
    }
    private static void Handle_Match_ShowCards(byte[] data)
    {
        var buffer = new ByteBuffer();
        buffer.WriteBytes(data);
        matchManager.ShowCards(buffer);
    }
    private static void Handle_Match_ShowResult(byte[] data)
    {
        var buffer = new ByteBuffer();
        buffer.WriteBytes(data);
        matchManager.ShowResult(buffer);
    }
    private static void Handle_Match_ConfirmToggleCard(byte[] data)
    {
        ByteBuffer buffer = new ByteBuffer();
        buffer.WriteBytes(data);
        buffer.ReadInteger(); //PackageID
        matchManager.ConfirmToggleCard(buffer.ReadInteger());//Read cardPos
    }
    private static void Handle_Match_FinishGame(byte[] data)
    {
        ByteBuffer buffer = new ByteBuffer();
        buffer.WriteBytes(data);
        buffer.ReadInteger(); //Read Package ID
        matchManager.FinishGame(buffer);
    }
    #endregion

}


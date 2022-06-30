using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomData
{
    //Atributes:
    public string roomID;
    public string roomStatus; //Os status sao: waiting, playing, ended, full
    public int roomCapacity;
    public int numPlayersRoom;
    public string turn; //Um turno eh composto por duas fases: HU-historia de Usuario e A-Avaliacao
    public string date;
    //players
    //rounds

    //Constructor:
    public RoomData(int roomCapacity)
    {
        this.roomID = "defaultRoomID";
        this.roomStatus = "waiting";
        this.roomCapacity = roomCapacity;
        this.numPlayersRoom = 0;
        this.turn = "HU1";
        this.date = "default";
    }

    public RoomData(int roomCapacity, string date)
    {
        this.roomID = "defaultRoomID";
        this.roomStatus = "waiting";
        this.roomCapacity = roomCapacity;
        this.numPlayersRoom = 0;
        this.turn = "HU1";
        this.date = date;
    }

    public RoomData(string roomID, int roomCapacity)
    {
        this.roomID = roomID;
        this.roomStatus = "waiting";
        this.roomCapacity = roomCapacity;
        this.numPlayersRoom = 0;
        this.turn = "HU1";
        this.date = "default";
    }

    //Constructor:
    public RoomData(string roomID, string status, int roomCapacity)
    {
        this.roomID = roomID;
        this.roomStatus = status;
        this.roomCapacity = roomCapacity;
        this.numPlayersRoom = 0;
        this.turn = "HU1";
        this.date = "default";
    }

    public RoomData(IDictionary<string, object> dict)
    {
        roomID = dict["roomID"].ToString();
        roomStatus = dict["roomStatus"].ToString(); //Os status sao: waiting, playing, full, ended
        roomCapacity = Convert.ToInt32(dict["roomCapacity"]);
        numPlayersRoom = 0;
        this.turn = dict["turn"].ToString();
        this.date = dict["date"].ToString();
    }
}
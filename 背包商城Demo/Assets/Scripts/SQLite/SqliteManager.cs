using Mono.Data.Sqlite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


public class SqliteManager : MonoBehaviour
{
    public static SqliteManager instance;
    private void Awake()
    {
        instance = this;
        this.CreateSQL();                    //����sql��
        OpenSQLaAndConnect();                //����sqlite
    }
    private SqliteConnection connection;     

    private SqliteCommand command;           //sqlite����

    private SqliteDataReader reader;         //��ȡ���ݶ���

    public string sqlName;                   //sqlite������

    public bool canAddItem = true;                  //���������Ƿ�������ӵ���
    
    //�������ݿ��ļ�
    public void CreateSQL()
    {
        if (!File.Exists(Application.streamingAssetsPath + "/" + this.sqlName))
        {
            this.connection = new SqliteConnection("data source=" + Application.streamingAssetsPath + "/" + this.sqlName);
            this.connection.Open();
            this.CreateSQLTable(
                "PlayerID1",
                "CREATE TABLE IF NOT EXISTS PlayerID1( " +
                "ItemID INT PRIMARY KEY," +
                "ItemCount INT NOT NULL)",
                null,
                null
            );
            this.CreateSQLTable(
                "PlayerID1WeaponCreate",
                "CREATE TABLE IF NOT EXISTS PlayerID1WeaponCreate( " +
                "WeaponID INT PRIMARY KEY)",
                null,
                null
            );
            InsertOrUpdateItem("1", 1, 2);
            InsertOrUpdateItem("1", 2, 3);
            InsertOrUpdateItem("1", 3, 2);
            InsertOrUpdateItem("1", 4, 20);
            InsertOrUpdateItem("1", 5, 30);
            InsertOrUpdateItem("1", 6, 40);
            InsertOrUpdateItem("1", 7, 50);
            InsertOrUpdateItem("1", 8, 20);

            for (int i = 1; i < 4; i++)
            {
                string query = "INSERT INTO PlayerID1WeaponCreate (WeaponID) VALUES(@WeaponID)";
                SqliteCommand command = new SqliteCommand(query, connection);
                command.Parameters.AddWithValue("@WeaponID", i);
                command.ExecuteNonQuery();
            }

            

            this.connection.Close();
            return;
        }

    }
    //�����ݿ�
    public void OpenSQLaAndConnect()
    {
        this.connection = new SqliteConnection("data source=" + Application.streamingAssetsPath + "/" + this.sqlName);
        this.connection.Open();
    }

    //����sqlite�������SqliteDataReader
    public SqliteDataReader ExecuteSQLCommand(string queryString)
    {
        this.command = this.connection.CreateCommand();
        this.command.CommandText = queryString;
        this.reader = this.command.ExecuteReader();
        return this.reader;
    }
    /// <summary>
    /// ͨ������SQL��䣬�����ݿ��д���һ����������е��е����ֺͶ�Ӧ����������
    /// </summary>
    public SqliteDataReader CreateSQLTable(string tableName, string commandStr = null, string[] columnNames = null, string[] dataTypes = null)
    {

        return ExecuteSQLCommand(commandStr);
    }
    //�̳ǹ�����½��
    public void UpdateCoins(string playerID,int itemID ,int count)
    {
        if (!DataModle.instance.CheckFullBag())
        {
            Debug.Log("��米��װ���£��������ˣ�");
            return;
        }
        if (connection.State == System.Data.ConnectionState.Open)
        {
            OpenSQLaAndConnect();
        }
        int coins = SelectItemPrice(itemID) * count;

        string query = "UPDATE Player SET Coins = Coins + @coins WHERE PlayerID = @playerID";
        SqliteCommand command = new SqliteCommand(query, connection);
        command.Parameters.AddWithValue("@coins", coins);
        command.Parameters.AddWithValue("@playerID", playerID);
        int exenum = command.ExecuteNonQuery();
        
    }
    //���뱳������
    public void InsertOrUpdateItem(string playerID, int itemID, int itemCount)
    {
        if (itemCount > 0)
        {
            if (!DataModle.instance.CheckFullBag())
            {
                return;
            }
       
        }
        if (connection.State == System.Data.ConnectionState.Open)
        {
            OpenSQLaAndConnect();
        }

        string query = "UPDATE PlayerID" + playerID + " SET ItemCount = ItemCount + @ItemCount WHERE ItemID = @ItemID";
        SqliteCommand command = new SqliteCommand(query, connection);
        command.Parameters.AddWithValue("@itemID", itemID);
        command.Parameters.AddWithValue("@itemCount", itemCount);
        int exenum = command.ExecuteNonQuery();

        if (exenum == 0)
        {
            query = "INSERT INTO PlayerID" + playerID + " VALUES(@itemID, @itemCount)";
            command = new SqliteCommand(query, connection);
            command.Parameters.AddWithValue("@itemID", itemID);
            command.Parameters.AddWithValue("@itemCount", itemCount);
            command.ExecuteNonQuery();
        }
        if (itemCount > 0)
            return;
        SqliteDataReader reader = SelectItem(playerID);
        int count = 0;
        //��ȡsqlite�����ӵ�е����ݣ���ϱ��ص��������ļ�
        while (reader.Read())
        {

            if (itemID == (int)reader["ItemID"])
            {
                count = (int)reader["ItemCount"];
            }
        }
        if (count <= 0)
        {
            DelectItem(playerID, itemID);
        }
    }
    //�������ӵ�еĽ��
    public int SelectPlayerCoins()
    {
        string query = "SELECT Coins FROM Player WHERE PlayerID = " + Config.PLAYER_ID;
        int coins = 0;
        SqliteDataReader sd = ExecuteSQLCommand(query);
        while (sd.Read())
        {

            coins = (int)sd["Coins"];
        }
        return coins;
    }
    //���ҵ��ߵ���
    public int SelectItemPrice(int itemID)
    {
        string query = "SELECT Price FROM PlayerID"+ Config.PLAYER_ID + "Shop WHERE ItemID = " + itemID;
        int prices = 0;
        SqliteDataReader sd = ExecuteSQLCommand(query);
        while (sd.Read())
        {

            prices = (int)sd["Price"];
        }
        return prices;
    }
    //�������е���
    public SqliteDataReader SelectItem(string PlayerID)
    {

        string query = "SELECT * FROM PlayerID" + PlayerID;
        
        ExecuteSQLCommand(query);
        
        return reader;
    }
    //��ѯ�̳����е���
    public SqliteDataReader SelectShopItem(string PlayerID)
    {

        string query = "SELECT * FROM PlayerID" + PlayerID + "Shop";

        ExecuteSQLCommand(query);

        return reader;
    }
    //ɾȥһ������
    public void DelectItem(string playerID, int itemID)
    {
        string query = "DELETE FROM PlayerID" + playerID + " WHERE ItemID = @ItemID";
        SqliteCommand command = new SqliteCommand(query, connection);
        command.Parameters.AddWithValue("@itemID", itemID);
        command.ExecuteNonQuery();
    }

    /// <summary>
    /// �ر����ݿ�����,ע����һ���ǳ���Ҫ�����ÿ�β��Խ�����ʱ�򶼵��ùر����ݿ�����
    /// �����ִ����һ������ε���֮�󣬻ᱨ�����ݿⱻ������ÿ�δ򿪶��ǳ�����
    /// </summary>
    public void CloseSQLConnection()
    {
        if (this.command != null)
        {
            this.command.Cancel();
        }

        if (this.reader != null)
        {
            this.reader.Close();
        }

        if (this.connection != null)
        {
            this.connection.Close();

        }
        this.command = null;
        this.reader = null;
        this.connection = null;
        Debug.Log("�Ѿ��Ͽ����ݿ�����");
    }
    //���˳�Ӧ�ó����ʱ��ر����ݿ�����
    private void OnApplicationQuit()
    {
        //�������˳�ʱ�ر����ݿ����ӣ���Ȼ���ظ������ݿ�����ɿ���
        this.CloseSQLConnection();
        Debug.Log("�����˳�");
    }


}

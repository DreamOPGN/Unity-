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
        this.CreateSQL();                    //创建sql库
        OpenSQLaAndConnect();                //连接sqlite
    }
    private SqliteConnection connection;     

    private SqliteCommand command;           //sqlite命令

    private SqliteDataReader reader;         //读取数据定义

    public string sqlName;                   //sqlite的名字

    public bool canAddItem = true;                  //用来控制是否可以增加道具
    
    //创建数据库文件
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
    //打开数据库
    public void OpenSQLaAndConnect()
    {
        this.connection = new SqliteConnection("data source=" + Application.streamingAssetsPath + "/" + this.sqlName);
        this.connection.Open();
    }

    //传入sqlite命令并返回SqliteDataReader
    public SqliteDataReader ExecuteSQLCommand(string queryString)
    {
        this.command = this.connection.CreateCommand();
        this.command.CommandText = queryString;
        this.reader = this.command.ExecuteReader();
        return this.reader;
    }
    /// <summary>
    /// 通过调用SQL语句，在数据库中创建一个表，定义表中的行的名字和对应的数据类型
    /// </summary>
    public SqliteDataReader CreateSQLTable(string tableName, string commandStr = null, string[] columnNames = null, string[] dataTypes = null)
    {

        return ExecuteSQLCommand(commandStr);
    }
    //商城购买更新金币
    public void UpdateCoins(string playerID,int itemID ,int count)
    {
        if (!DataModle.instance.CheckFullBag())
        {
            Debug.Log("大哥背包装不下，别再买了！");
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
    //插入背包数据
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
        //获取sqlite玩家所拥有的数据，结合本地道具配置文件
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
    //查找玩家拥有的金币
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
    //查找道具单价
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
    //查找所有道具
    public SqliteDataReader SelectItem(string PlayerID)
    {

        string query = "SELECT * FROM PlayerID" + PlayerID;
        
        ExecuteSQLCommand(query);
        
        return reader;
    }
    //查询商城所有道具
    public SqliteDataReader SelectShopItem(string PlayerID)
    {

        string query = "SELECT * FROM PlayerID" + PlayerID + "Shop";

        ExecuteSQLCommand(query);

        return reader;
    }
    //删去一行数据
    public void DelectItem(string playerID, int itemID)
    {
        string query = "DELETE FROM PlayerID" + playerID + " WHERE ItemID = @ItemID";
        SqliteCommand command = new SqliteCommand(query, connection);
        command.Parameters.AddWithValue("@itemID", itemID);
        command.ExecuteNonQuery();
    }

    /// <summary>
    /// 关闭数据库连接,注意这一步非常重要，最好每次测试结束的时候都调用关闭数据库连接
    /// 如果不执行这一步，多次调用之后，会报错，数据库被锁定，每次打开都非常缓慢
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
        Debug.Log("已经断开数据库连接");
    }
    //当退出应用程序的时候关闭数据库连接
    private void OnApplicationQuit()
    {
        //当程序退出时关闭数据库连接，不然会重复打开数据卡，造成卡顿
        this.CloseSQLConnection();
        Debug.Log("程序退出");
    }


}

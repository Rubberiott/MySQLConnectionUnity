using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using UnityEngine;

public class TestConnection : MonoBehaviour
{
    //mysql://root:ztNhIfaoLiDnlHLgrCetLaUfYOTRfkeQ@monorail.proxy.rlwy.net:13001/railway
    //Server=monorail.proxy.rlwy.net;Database=railway;User=root;Password=ztNhIfaoLiDnlHLgrCetLaUfYOTRfkeQ;Port=13001;
    [Header("UserData")]
    public string _name;
    public string _lastname;
    public int _age;

    private string connectionString;
    private MySqlConnection connection;

    private void Start()
    {
        // Configura la cadena de conexion
        connectionString = "Server=monorail.proxy.rlwy.net;Database=railway;User=root;Password=ztNhIfaoLiDnlHLgrCetLaUfYOTRfkeQ;Port=13001;";
        ConnectToDatabase();
    }

    private void ConnectToDatabase()
    {
        try
        {
            connection = new MySqlConnection(connectionString);
            connection.Open();
            Debug.Log("Conexion exitosa a la base de datos.");
        }
        catch (Exception e)
        {
            Debug.LogError("Error al conectar a la base de datos: " + e.Message);
        }
    }
    public void UploadData()
    {
        InsertData(_name, _lastname, _age);
    }
    public void DownloadData()
    {
        List<UserPoints> users = GetAllData();
        foreach (UserPoints user in users)
        {
            Debug.Log($"ID: {user.Id}, Nombre: {user.Nom}, Apellido: {user.Cognom}, Edad: {user.Edat}");
        }
    }
    public void InsertData(string nombre, string apellido, int edad)
    {
        try
        {
            string query = "INSERT INTO UserPoints (Nom, Cognom, Edat) VALUES (@nombre, @apellido, @edad)";
            MySqlCommand cmd = new MySqlCommand(query, connection);

            cmd.Parameters.AddWithValue("@nombre", nombre);
            cmd.Parameters.AddWithValue("@apellido", apellido);
            cmd.Parameters.AddWithValue("@edad", edad);

            cmd.ExecuteNonQuery();
            Debug.Log("Datos insertados correctamente.");
        }
        catch (Exception e)
        {
            Debug.LogError("Error al insertar datos: " + e.Message);
        }
    }
    public List<UserPoints> GetAllData()
    {
        List<UserPoints> users = new List<UserPoints>();

        try
        {
            string query = "SELECT * FROM UserPoints";
            MySqlCommand cmd = new MySqlCommand(query, connection);
            MySqlDataReader dataReader = cmd.ExecuteReader();

            while (dataReader.Read())
            {
                UserPoints user = new UserPoints();
                user.Id = dataReader.GetInt32("id");
                user.Nom = dataReader.GetString("Nom");
                user.Cognom = dataReader.GetString("Cognom");
                user.Edat = dataReader.GetInt32("Edat");
                users.Add(user);
            }

            dataReader.Close();
            Debug.Log("Datos recibidos correctamente.");
        }
        catch (Exception e)
        {
            Debug.LogError("Error al recibir datos: " + e.Message);
        }

        return users;
    }
    private void OnDestroy()
    {
        if (connection != null)
        {
            connection.Close();
        }
    }
}
public class UserPoints
{
    public int Id { get; set; }
    public string Nom { get; set; }
    public string Cognom { get; set; }
    public int Edat { get; set; }
}
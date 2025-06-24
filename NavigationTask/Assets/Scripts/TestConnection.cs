using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Npgsql;
using System; 

public class TestConnection : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        connect();    
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void connect()
    {
        string connectionString = "Host=ec2-3-219-137-162.compute-1.amazonaws.com;Port=5432;Username=okelgitzfhzrgh;Password=81a78dd1293dab211a3f2df579d4e8d5addbd29f50d1142fb04f0064be4c83b7;Database=d6phbbtpiecvhi;SslMode=Require;Trust Server Certificate=true";

        try
        {
            using (var conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();
                Debug.Log("Conection successful");
                conn.Close();
            }
        }
        catch (Exception ex)
        {
            Debug.Log($"Error: {ex.Message}");
        }
    }
}



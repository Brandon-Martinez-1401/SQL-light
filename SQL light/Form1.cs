using System.Data.SQLite;
namespace SQL_light
{
    public partial class Form1 : Form
    {
        //Agregamos la base de datos SQLite
        string path = "data_table.db";
        string cs = @"URI=file:" + Application.StartupPath + "\\data_table.db";
        //la base de datos se encuentra en la carpeta Debug
        SQLiteConnection con;
        SQLiteCommand cmd;
        SQLiteDataReader dr;


        public Form1()
        {
            InitializeComponent();

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            create_db();
            data_show();

        }
        private void data_show()
        {
            var con = new SQLiteConnection(cs);
            con.Open();

            string stm = "SELECT * FROM data_table";
            var cmd = new SQLiteCommand(stm, con);
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                dataGridView1.Rows.Insert(0, dr.GetString(0), dr.GetString(1));
            }
        }
        private void create_db()
        {
            if (!File.Exists(path))
            {
                SQLiteConnection.CreateFile(path);
                using (var sqlite = new SQLiteConnection(@"Data Source=" + path))
                {
                    sqlite.Open();
                    string sql = "CREATE TABLE data_table(name varchar(20), id varchar(20))";
                    SQLiteCommand command = new SQLiteCommand(sql, sqlite);
                    command.ExecuteNonQuery();
                }

            }
            else
            {
                MessageBox.Show("La base de datos no se puede crear");
                return;
            }
        }

        private void btnInsertar_Click(object sender, EventArgs e)
        {
            var con = new SQLiteConnection(cs);
            con.Open();
            var cmd = new SQLiteCommand(con);
            try
            {
                cmd.CommandText = "INSERT INTO test (name, id) VALUES(@name, @id)";
                string NAME = txbNombre.Text;
                string ID = txbID.Text;
                cmd.Parameters.AddWithValue("@name", NAME);
                cmd.Parameters.AddWithValue("@id", ID);

                dataGridView1.ColumnCount = 2;
                dataGridView1.Columns[0].Name = "Nombre";
                dataGridView1.Columns[1].Name = "ID";
                string[] row = new string[] { NAME, ID };
                dataGridView1.Rows.Add(row);
                cmd.ExecuteNonQuery();
            }
            
            catch (Exception)
            {
                MessageBox.Show("No se pudieron insertar los datos");
                return;
            }
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            var con = new SQLiteConnection(cs);
            con.Open();
            var cmd = new SQLiteCommand(con);
            try
            {
                cmd.CommandText = "UPDATE test SET id=@id WHERE name=@name";
                cmd.Prepare();
                cmd.Parameters.AddWithValue("@name", txbNombre.Text);
                cmd.Parameters.AddWithValue("@id", txbID.Text);
                cmd.ExecuteNonQuery();
                data_show();
            }
            catch (Exception)
            {
                MessageBox.Show("No se pudieron actualizar los datos");
                return;
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            var con = new SQLiteConnection(cs);
            con.Open();
            var cmd = new SQLiteCommand(con);
            try
            {
                cmd.CommandText = "DELETE FROM test WHERE name=@name";
                cmd.Prepare();
                cmd.Parameters.AddWithValue("@name", txbNombre.Text);
                cmd.ExecuteNonQuery();
                dataGridView1.Rows.Clear();
                data_show();
            }
            catch (Exception)
            {
                MessageBox.Show("No se pudieron eliminar los datos");
                return;
            }
        }
    }
}

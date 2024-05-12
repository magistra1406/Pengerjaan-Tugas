
using Npgsql;
using PercobaanAPI_2048.Entities;
using PercobaanAPI_2048.Utils;

namespace PercobaanApi1.Repositories
{
    public class RegionRepository
    {
        private DbUtil dbUtil;
        public RegionRepository(DbUtil dbUtil)
        {
            this.dbUtil = dbUtil;
        }

        public List<Region> findAll()
        {
            List<Region> regions = new List<Region>();
            string sql = "SELECT * FROM users.regions";
            try
            {
                NpgsqlCommand cmd = dbUtil.GetNpgsqlCommand(sql);
                NpgsqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Region region = new Region();
                    region.region_id = reader.GetInt32(0);
                    region.name = reader.GetString(1);
                    regions.Add(region);
                }
                cmd.Dispose();
                dbUtil.closeConnection();
            }
            catch (Exception e)
            {
                dbUtil.closeConnection();
                throw new Exception(e.Message);
            }
            return regions;
        }

        public Region findById(int region_id)
        {
            string sql = "SELECT * FROM users.regions WHERE id = @id";
            try
            {
                NpgsqlCommand cmd = dbUtil.GetNpgsqlCommand(sql);
                cmd.Parameters.AddWithValue("@id", region_id);
                NpgsqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    Region region = new Region();
                    region.region_id = reader.GetInt32(0);
                    region.name = reader.GetString(1);
                    cmd.Dispose();
                    dbUtil.closeConnection();
                    return region;
                }
                cmd.Dispose();
                dbUtil.closeConnection();
            }
            catch (Exception e)
            {
                dbUtil.closeConnection();
                throw new Exception(e.Message);
            }
            return null;
        }

        public Region create(Region entity)
        {
            string sql = "INSERT INTO users.regions (name) VALUES (@name) RETURNING id";
            try
            {
                NpgsqlCommand cmd = dbUtil.GetNpgsqlCommand(sql);
                cmd.Parameters.AddWithValue("@name", entity.name);
                entity.region_id = (int)cmd.ExecuteScalar();
                cmd.Dispose();
                dbUtil.closeConnection();
                return entity;
            }
            catch (Exception e)
            {
                dbUtil.closeConnection();
                throw new Exception(e.Message);
            }
            return null;
        }

        public Region update(Region entity)
        {
            string sql = "UPDATE users.regions SET name = @name WHERE id = @id";
            try
            {
                NpgsqlCommand cmd = dbUtil.GetNpgsqlCommand(sql);
                cmd.Parameters.AddWithValue("@name", entity.name);
                cmd.Parameters.AddWithValue("@id", entity.region_id);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                dbUtil.closeConnection();
                return entity;
            }
            catch (Exception e)
            {
                dbUtil.closeConnection();
                throw new Exception(e.Message);
            }
            return null;
        }

        public Region delete(Region entity)
        {
            string sql = "DELETE FROM users.regions WHERE id = @id";
            try
            {
                NpgsqlCommand cmd = dbUtil.GetNpgsqlCommand(sql);
                cmd.Parameters.AddWithValue("@id", entity.region_id);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                dbUtil.closeConnection();
                return entity;
            }
            catch (Exception e)
            {
                dbUtil.closeConnection();
                throw new Exception(e.Message);
            }
            return null;
        }
    }
}
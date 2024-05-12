namespace PercobaanAPI_2048.Entities
{
    public class User 
    {
        public int id_person {get; set;}
        public string name {get; set;}
        public string address {get; set;}
        public string? email {get; set;}
        public string? password {get; set;}
        public string? token {get; set;}

    }
}
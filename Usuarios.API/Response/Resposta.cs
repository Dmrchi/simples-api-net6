namespace Usuarios.API.Response
{
    public class Resposta<T>
    {
        public string Titulo { get; set; }
        public bool Sucesso { get; set; }
        public int Status { get; set; }
        public List<T> Dados { get; set; }
        public T Objeto { get; set; }
        //public bool Vazio => Dados.Count() == 0 ? true : false;    
    }
}

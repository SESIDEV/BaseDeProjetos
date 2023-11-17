public class EmpresasFunilDTO
{
    public int Id { get; set; }
    public string Nome { get; set; }
}

public class EmpresasFunilComUniqueDTO : EmpresasFunilDTO
{
    public string EmpresaUnique { get; set; }
}

public class UsuariosFunilDTO
{
    public string Id { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
}
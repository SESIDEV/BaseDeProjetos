using BaseDeProjetos.Helpers;
using BaseDeProjetos.Models;
using System.Collections.Generic;

public class EmpresasViewModel
{
    public List<Empresa> Empresas { get; set; }
    public Pager Pager { get; set; }
}
﻿// Classe Bean simples para armazenar a receita do período
using System;
using System.ComponentModel.DataAnnotations;

namespace BaseDeProjetos.Models
{
    public class IndicadoresFinanceiros
    {
        [Key]
        public int Id { get; set; }
        public DateTime Data { get; set; }
        public decimal Receita { get; set; }
        public decimal Despeita { get; set; }
        public decimal Investimento { get; set; }
    }
}

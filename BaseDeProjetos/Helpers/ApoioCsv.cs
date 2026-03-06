using BaseDeProjetos.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace BaseDeProjetos.Helpers
{
    public static class ApoioCsv
    {
        public static List<PessoaModel> LerPessoasDeCsv(string filePath, bool hasHeader = true)
        {
            var pessoas = new List<PessoaModel>();
            if (!File.Exists(filePath)) return pessoas;

            using var sr = new StreamReader(filePath);
            string line;
            bool primeira = true;

            while ((line = sr.ReadLine()) != null)
            {
                if (primeira && hasHeader)
                {
                    primeira = false;
                    continue;
                }

                if (string.IsNullOrWhiteSpace(line)) continue;

                // Tenta separar por vírgula ou ponto-e-vírgula, trata aspas simples/duplas
                var parts = line.Split(',');
                if (parts.Length == 1)
                {
                    parts = line.Split(';');
                }

                string Clean(string s) => (s ?? "").Trim().Trim('"').Trim('\'');

                var nome = parts.Length > 0 ? Clean(parts[0]) : string.Empty;
                var cargo = parts.Length > 1 ? Clean(parts[1]) : string.Empty;
                var casa = parts.Length > 2 ? Clean(parts[2]) : string.Empty;
                var situacao = parts.Length > 3 ? Clean(parts[3]) : string.Empty;

                pessoas.Add(new PessoaModel
                {
                    Nome = nome,
                    Cargo = cargo,
                    Casa = casa,
                    Situacao = situacao
                });

            }

            return pessoas;
        }
    }
}
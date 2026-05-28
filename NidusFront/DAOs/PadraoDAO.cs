using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using NidusFront.Models;

namespace NidusFront.DAOs
{
    // Restrição: T deve ser uma classe que herda de PadraoViewModel
    public abstract class PadraoDAO<T> where T : PadraoViewModel
    {
        // Métodos abstratos que cada classe filha SERÁ OBRIGADA a implementar customizadamente
        public abstract void Insert(T model);
        public abstract void Update(T model);
        public abstract void Delete(int id);
        public abstract T Select(int id);
        public abstract List<T> List();

        /// <summary>
        /// Método auxiliar protegido para mapear os parâmetros do SQL, 
        /// essencial para cumprir a regra de proteção contra SQL Injection.
        /// </summary>
        protected abstract SqlParameter[] CreateParameters(T model);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LYA1_Lexico3
{
    public class Token
    {
        public enum Tipos
        {
            Identificador,Numero,Caracter,
            asignacion,finSentencia,opLogico,opRelacional,
            opTermino, incTermino,opFactor,incFactor,opTernario,cadena,comentario,
            llaveInicio, llaveFin    

        }
        private string contenido;
        private Tipos  clasificacion;
        public Token()
        {
            contenido = "";
            clasificacion = Tipos.Identificador;
        }
        public void setContenido(string contenido)
        {
            this.contenido = contenido;
        }
        public void setClasificacion(Tipos clasificacion)
        {
            this.clasificacion = clasificacion;
        }
        public string getContenido()
        {
            return this.contenido;
        }
        public Tipos getClasificacion()
        {
            return this.clasificacion;
        }
    }
}